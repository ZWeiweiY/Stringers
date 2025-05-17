# Stringers FastAPI Server

A FastAPI server that performs real-time face and hand detection using stringers models. The server supports both GPU and CPU configurations, and uses WebSocket connections for streaming video processing.

## Features

- Real-time face detection and landmark tracking
- Hand detection
- WebSocket support for streaming video processing
- Support for both GPU and CPU inference
- Configurable server ports
- Docker containerization

## Prerequisites

- Docker
- For GPU support:
  - NVIDIA GPU with updated drivers
  - NVIDIA Container Toolkit

## Project Structure

```
.
├── main.py                # FastAPI server main file
├── requirements-cpu.txt   # Python dependencies for CPU-only setup
├── requirements-gpu.txt   # Python dependencies for GPU-accelerated setup
├── Dockerfile-gpu        # Dockerfile for GPU support
├── Dockerfile-cpu        # Dockerfile for CPU-only
├── models/               # Directory for model files
│   ├── face_landmarker.task
│   └── hand_landmarker.task
└── inference/            # Directory for implementation files
    ├── face_detection.py    # Face detection implementation
    └── hand_detection.py    # Hand detection implementation
```

## Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/etc-stringers/server.git
cd server
```

### 2. GPU Setup (Optional)

If you plan to use GPU acceleration, install NVIDIA Container Toolkit:

```bash
# For Ubuntu/WSL
distribution=$(. /etc/os-release;echo $ID$VERSION_ID)
curl -s -L https://nvidia.github.io/nvidia-docker/gpgkey | sudo apt-key add -
curl -s -L https://nvidia.github.io/nvidia-docker/$distribution/nvidia-docker.list | sudo tee /etc/apt/sources.list.d/nvidia-docker.list
sudo apt-get update
sudo apt-get install -y nvidia-container-toolkit
```

Verify GPU setup:
```bash
nvidia-smi
```

## Running the Server

### GPU Version

1. Build the Docker image:
```bash
docker build -f Dockerfile-gpu -t stringers-server-gpu .
```

2. Run the container:
```bash
# Default port (12345)
docker run --gpus all -p 12345:12345 stringers-server-gpu

# Custom port (e.g., 8080)
docker run --gpus all -e SERVER_PORT=8080 -p 8080:8080 stringers-server-gpu
```

### CPU Version

1. Build the Docker image:
```bash
docker build -f Dockerfile-cpu -t stringers-server-cpu .
```

2. Run the container:
```bash
# Default port (12345)
docker run -p 12345:12345 stringers-server-cpu

# Custom port (e.g., 8080)
docker run -e SERVER_PORT=8080 -p 8080:8080 stringers-server-cpu
```

### Development Mode

For development with live code updates:

```bash
# GPU Version
docker run --gpus all -p 12345:12345 -v $(pwd):/app stringers-server-gpu

# CPU Version
docker run -p 12345:12345 -v $(pwd):/app stringers-server-cpu
```

## API Endpoints

- HTTP endpoint: `http://localhost:<port>/`
- WebSocket endpoint: `ws://localhost:<port>/ws`

## Dependency Differences
This project includes two seperate requirement files:
- `requirements-cpu.txt`: Uses TensorFlow CPU-only version
- `requirements-gpu.txt`: Includes CUDA dependencies and full TensorFlow with GPU support

The appropriate Dockerfile (`Dockerfile-cpu` or `Dockerfile-gpu`) will use the corresponding requirements file.

## Docker Commands Reference

```bash
# List running containers
docker ps

# Stop a container
docker stop <container_id>

# View logs
docker logs <container_id>

# Enter container shell
docker exec -it <container_id> bash

# Remove container
docker rm <container_id>

# Remove image
docker rmi stringers-server-gpu  # or stringers-server-cpu
```

## Troubleshooting

1. **Port Already in Use**
```bash
# Find process using the port
sudo lsof -i :<port>

# Kill the process
sudo kill -9 <PID>
```

2. **GPU Not Detected**
- Verify NVIDIA drivers are installed: `nvidia-smi`
- Check NVIDIA Container Toolkit installation
- Ensure `--gpus all` flag is used when running the container

3. **Model Loading Errors**
- Check that the models directory exists and contains:
    - face_landmarker.task
    - hand_landmarker.task
- Verify file permissions

4. **GPU-specific Issues**
- If you see CUDA-related errors, make sure your GPU drivers are compatible with CUDA 12.3
- For older GPUs, you may need to modify the requirements-gpu.txt to use appropriate CUDA versions

## Environment Variables

- `SERVER_PORT`: Port number for the server (default: 12345)

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

MIT

## Contact

Team: etc-stringers@lists.andrew.cmu.edu

Producer/Programmer: yuanhenq@andrew.cmu.edu, zhiweiy@andrew.cmu.edu

