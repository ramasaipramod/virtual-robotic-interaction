import torch
import torch.nn as nn
import torchvision.transforms as transforms
import numpy as np
import matplotlib.pyplot as plt
from scipy.signal import spectrogram
from PIL import Image
import io

class SelfAttention(nn.Module):
    def __init__(self, channel):
        super(SelfAttention, self).__init__()
        self.query_conv = nn.Conv2d(channel, channel // 8, kernel_size=1)
        self.key_conv = nn.Conv2d(channel, channel // 8, kernel_size=1)
        self.value_conv = nn.Conv2d(channel, channel, kernel_size=1)
        self.softmax = nn.Softmax(dim=-1)
        self.gamma = nn.Parameter(torch.zeros(1))

    def forward(self, x):
        batch, channel, height, width = x.size()
        
        query = self.query_conv(x).view(batch, -1, height * width).permute(0, 2, 1)
        key = self.key_conv(x).view(batch, -1, height * width)
        energy = torch.bmm(query, key)
        attention = self.softmax(energy)
        
        value = self.value_conv(x).view(batch, -1, height * width)
        out = torch.bmm(value, attention.permute(0, 2, 1))
        out = out.view(batch, channel, height, width)
        
        out = self.gamma * out + x
        return out

class NETWithAttentionAndClassifier(nn.Module):
    def __init__(self, pretrained_model, num_classes, channel):
        super(NETWithAttentionAndClassifier, self).__init__()
        self.resnet = nn.Sequential(*list(pretrained_model.children())[:-2])
        self.attention = SelfAttention(channel)
        self.avgpool = nn.AdaptiveAvgPool2d((1, 1))
        self.fc = nn.Linear(channel, num_classes)

    def forward(self, x):
        x = self.resnet(x)
        x = self.attention(x)
        x = self.avgpool(x)
        x = torch.flatten(x, 1)
        x = self.fc(x)
        return x

def generate_spectrogram_from_signal(signal, 
                                     time=None, 
                                     window_length=128, 
                                     overlap=120, 
                                     dft_length=256, 
                                     rescale_size=(512, 512)):
    """
    Generate a spectrogram from a signal.
    
    Args:
        signal (list or np.ndarray): Input signal data
        time (list or np.ndarray, optional): Corresponding time values
        window_length (int): Length of the window for STFT
        overlap (int): Number of overlapping samples between windows
        dft_length (int): Length of Discrete Fourier Transform
        rescale_size (tuple): Size to resize the spectrogram image
    
    Returns:
        torch.Tensor: Processed spectrogram image tensor
    """
    # Convert signal to numpy array if it's a list
    signal = np.array(signal)
    
    # If no time provided, create a linear time array
    if time is None:
        time = np.linspace(0, len(signal), len(signal))
    
    # Create Hamming window
    hamming_window = np.hamming(window_length)
    
    # Compute sampling frequency
    fs = len(time) / time[-1]
    
    # Compute spectrogram
    frequencies, times, Sxx = spectrogram(
        signal,
        fs=fs,
        window=hamming_window,
        noverlap=overlap,
        nfft=dft_length,
        scaling='spectrum'
    )
    
    # Create spectrogram plot
    plt.figure(figsize=(6, 5))
    plt.pcolormesh(times, frequencies, 10 * np.log10(Sxx), shading='gouraud', cmap='jet')
    plt.xticks([])
    plt.yticks([])
    plt.colorbar().remove()
    
    # Save to buffer
    buffer = io.BytesIO()
    plt.savefig(buffer, format="png", bbox_inches='tight', pad_inches=0)
    buffer.seek(0)
    plt.close()
    
    # Open and resize image
    img = Image.open(buffer)
    img_resized = img.resize(rescale_size, Image.Resampling.LANCZOS)
    
    # Apply transforms
    transform = transforms.Compose([
        transforms.ToTensor(),
        transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))
    ])
    
    return transform(img_resized)

def classify_signal(signal, model, time=None):

    model.eval()
    # Generate spectrogram
    spectrogram_tensor = generate_spectrogram_from_signal(signal, time)
    
    # Add batch dimension
    spectrogram_tensor = spectrogram_tensor.unsqueeze(0)
    
    # Perform inference
    with torch.no_grad():
        outputs = model(spectrogram_tensor)
    
    return outputs

if __name__ == "__main__":
    pass