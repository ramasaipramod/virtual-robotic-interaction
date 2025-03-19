# Virtual Robotic Interaction with Multiple Sensors

This project implements a real-time gesture control system for robotic applications using multimodal sensing (EMG signals and video). The system processes both EMG and video data to enable gesture-based control of a robotic arm in a virtual environment.

### Project Structure
.
├── CamGest/
│   ├── GestureClassifier/
│   │   ├── default.hdf5
│   │   └── default.tflite
│   └── HandGesture/
│       ├── keypoint_classification.ipynb
│       ├── app.py
│       ├── requirements.txt
│       └── model/
│           └── keypoint_classifier/
│               ├── keypoint.csv
│               ├── keypoint_classifier.tflite
│               ├── keypoint_classifier_label.csv
│               └── keypoint_classifier.py
├── EmgGest/
│   ├── Data/
│   │   ├── data.csv
│   │   └── rawdata.csv
│   ├── EmgSim/
│   │   ├── app.js
│   │   ├── index.html
│   │   ├── style.css
│   │   └── Classification/
│   │       └── EMGClassification.ipynb
│   └── Server/
│       ├── app.py
│       └── utils.py
└── UnityProject/
    ├── Assets/
    ├── ProjectSettings/
    └── UserSettings/

### Components

1. Camera based gesture recognition
- Located in CamGest/
- Uses trained models in GestureClassifier/ for gesture classification
- HandGesture/app.py initiates camera feed and sends classifications to Unity via sockets
- Training pipeline available in keypoint_classification.ipynb

2. EMG-based Gesture Recognition
- Located in EmgGest/
- EMG Signal Simulation (EmgSim)
  - Web interface for EMG signal simulation
    index.html: Main interface
    app.js: Client-side logic
    style.css: Styling
    Classification/EMGClassification.ipynb: EMG signal classification notebook
- Server
  - Main processing backend for EMG signals
  - app.py: Server implementation
  - utils.py: Contains preprocessing utilities and models for stacked spectrograms
- Data
  - data.csv: Processed simulation data
  - rawdata.csv: Raw EMG simulation data

3. Unity Implementation (UnityProject)
- Contains the 3D environment and robotic arm implementation
- Assets/: Contains all project assets, scripts, and prefabs
- ProjectSettings/: Unity project configuration files
- UserSettings/: User-specific Unity settings

### Unity Setup

1. Open Unity Hub and add the UnityProject folder
2. Install required Unity version (specified in ProjectSettings)
3. Open the project and ensure all socket connections are properly configured
4. Play mode can be entered to test robotic arm responses to gestures

### Setup for Camera based Gesture Classification

1. Install Python dependencies
cd CamGest/HandGesture
pip install -r requirements.txt

2. Run the application
python app.py

### Setup for EMG based Gesture Classification

1. Install Python dependencies
cd Server
pip install -r requirements.txt

2. Start the Server
python app.py

### Model Training

1. Hand Gesture Recognition:
- Use keypoint_classification.ipynb to train on new gesture data
- Trained models are saved to GestureClassifier/

2. EMG Classification:
- Use EMGClassification.ipynb for EMG signal classification
- Trained model is saved in same folder

### Usage

- EMG Signal Simulation:
  - Open the EMG simulator in a web browser
  - Select desired gesture from available options
  - Simulate EMG signal for chosen gesture and labeled

- Camera-based Gesture Recognition:
  - Run the gesture recognition system
  - Perform gestures in front of the camera
  - System will classify gestures in real-time

- Integration with Unity:
  - Any of the EMG or camera-based classification is sent to Unity
  - Unity environment receives classifications via socket connections
  - Robotic arm responds to recognized gestures
