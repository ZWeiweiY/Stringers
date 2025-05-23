# Stringers

**A face-controlled space navigation game developed at Carnegie Mellon University's Entertainment Technology Center (ETC).**

[Watch the Demo](https://drive.google.com/file/d/1_7z6-q4TgWScHqgK1ZwvlIRsdobYqFCD/view?usp=drive_link)

---

## Overview

**Stringers** is an experimental research-driven game that transforms facial gestures into real-time space navigation mechanics. Developed by a team of six graduate students at CMU's Entertainment Technology Center in collaboration with the CMU Physical Intelligence Lab, the game serves as a tool to study motor skill acquisition through innovative, gesture-based interactions.

As the programmer, I engineered the machine learning pipeline, real-time server-client communication, facial gesture tracking, and data collection systems—all centered around a unique gameplay experience where you play as a ghost flying through a cosmic maze, powered only by your face.

---

## Features

### 🧠 Real-Time Facial Gesture Control

- Utilizes **Google MediaPipe** for facial landmark detection and blendshape output.
- Calculates **3D head pose (yaw, pitch, roll)** using OpenCV’s `solvePnP` for precise spatial navigation.
- Maps **expressions** (e.g., mouth open, eye blinks, lip pucker) to in-game actions such as:
  - 🚀 Navigation
  - 🔫 Shooting meteors
  - 💨 Dashing through gaps
  - 🌀 Shrinking through pipes

### 🔄 Live ML Model Inference & Communication

- FastAPI backend processes webcam frames from Unity in real-time.
- WebSocket communication enables low-latency streaming of detection data (pose + expressions).
- Server-hosted models run live inference and return structured pose/gesture data to the client.

### 👻 Expressive Character Mirroring

- Ghost avatar mirrors the player's face with responsive blendshape-driven animations.
- Enhances immersion and emotional connection through facial expression syncing.
- A favorite feature among playtesters for its humor, novelty, and responsiveness.

### 🎮 Dynamic Unity Gameplay System

- Event-driven architecture using Unity’s **ScriptableObjects**.
- Modular gesture-to-action mapping allows rapid tuning of gameplay mechanics.
- Head tilts and facial triggers are used for fine-grained control, making each interaction meaningful.

### 📊 Data Collection Pipeline

- Logs all head rotations and facial expression activations with **confidence scores** and **timestamps**.
- Merges **game state data** (e.g., level, position, success/failure) with **gesture data**.
- Configurable for different research experiments (adjust sampling rate, event types).
- Enables in-depth analysis of player performance and motor learning progression.

---

## Tech Stack

- **Unity (C#)** – Game development and real-time avatar control
- **FastAPI (Python)** – Backend inference server
- **MediaPipe + OpenCV** – Face tracking and pose estimation
- **WebSockets** – Bi-directional, low-latency communication

---

## Deployment

Originally intended as a browser-based experience, **Stringers** pivoted mid-project toward lab-focused deployment to support controlled research conditions. The current deployment supports:

- 🎯 Standalone desktop build (Unity)
- 🧠 FastAPI inference server (Python)
- 🧪 Local lab setup with webcam access and reliable network conditions


---

## Research Integration

This project was designed not just for fun, but as a **scientific research tool**. It has been used for:

- Studying non-traditional input methods for motor skill acquisition.
- Logging facial motor performance data in a controlled game environment.
- Exploring the effectiveness of expressive feedback in user-avatar interactions.

---

## Acknowledgments

Developed by **Team Stringers**  
Carnegie Mellon University – Entertainment Technology Center  
In collaboration with the **CMU Physical Intelligence Lab**

Special thanks to our faculty advisors, playtesters, and research partners for their support and insight.

---

## License

This project is licensed under the Apache v2 license. See the [LICENSE](LICENSE) file for details.

---

## Learn More

- 🎥 [Gameplay Demo](https://drive.google.com/file/d/1_7z6-q4TgWScHqgK1ZwvlIRsdobYqFCD/view?usp=drive_link)
- 🌐 [Project Website](https://projects.etc.cmu.edu/stringers)
- 📄 [ETC Project Page](https://projects.etc.cmu.edu/stringers)

---
