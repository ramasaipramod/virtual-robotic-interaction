from flask import Flask, request, jsonify 
from flask_cors import CORS   
import socket
import pandas as pd
import time
import torch
import torchvision.models as models
from utils import classify_signal, NETWithAttentionAndClassifier

app = Flask(__name__)

datapath = r"..\Data\data.csv"
df = pd.read_csv(datapath)
last_column_name = df.columns[-1]


# Load pre-trained ResNet model
pretrained_resnet = models.resnet18(pretrained=True)

# Create the custom model with attention
model = NETWithAttentionAndClassifier(
    pretrained_model=pretrained_resnet, 
    num_classes=5,  
    channel=512     
)

# Load the trained weights
model.load_state_dict(torch.load('EmgSim\Classification\model.pth'))


CORS(app, resources={r"/*": {"origins": "*"}})

def emg_signal(class_number):
    filtered_df = df[df[last_column_name] == class_number]
    if not filtered_df.empty:
        random_sample = filtered_df.sample(n=1).iloc[0]
        return random_sample

def func(signal):
    prediction = classify_signal(signal)
    return prediction 

# Create a socket connection to Unity (assuming Unity is listening on localhost:12345)
UNITY_IP = '127.0.0.1'
UNITY_PORT = 12345

def send_to_unity(prediction):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((UNITY_IP, UNITY_PORT))
        s.sendall(str(prediction).encode())
        print("Sending ",prediction," to unity")
        s.close()

@app.route('/process_number', methods=['POST'])
def process_number():
    number = request.json.get('number')
    signal = emg_signal(number).to_list() 
    prediction = func(signal)

    # Split signal into 5 channels
    try:
        send_to_unity(prediction)
    except Exception as e:
        print(e)
    channels = {f'channel_{i+1}': signal[i*10000+2:(i+1)*10000+2] for i in range(5)}  # Assuming signal has 5 components

    return jsonify({'status': 'success', 'prediction': prediction, 'signal': channels})

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)
