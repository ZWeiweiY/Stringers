import uvicorn
import asyncio
import numpy as np
import cv2
import json
import argparse

from fastapi import FastAPI, WebSocket
from fastapi.middleware.cors import CORSMiddleware

# from inference.hand_detection import HandDetection
from inference.face_detection import FaceDetection

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

active_connections = set()

# hand_detector = HandDetection()
face_detector = FaceDetection()

@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    active_connections.add(websocket)

    async def process_frame_and_send(websocket, frame_np):
        try:

            # Hands Inference
            # hand_detector.process(frame_np)
            # result = hand_detector.results
            # print(f"[{time.time()}]: {result}")

            # Face Inference
            face_detector.process(frame_np)
            

            # await websocket.send_text("Image received")
            response = {
                "Yaw": face_detector.face_yaw_score, 
                "Pitch": face_detector.face_pitch_score, 
                "Roll": face_detector.face_roll_score, 
                "mouth_open": face_detector.mouth_open_score,
                "mouth_pucker": face_detector.mouth_pucker_score,
                "left_eye_blink": face_detector.left_eye_blink_score,
                "right_eye_blink": face_detector.right_eye_blink_score,
                "mouth_roll": face_detector.mouth_roll_score,
                "landmarks" : face_detector.face_landmarks,
            }
            # print(response)
            response_bytes = json.dumps(response).encode("utf-8")
            await websocket.send_bytes(response_bytes)

            # response = {"message": "Image received"}
            # await websocket.send_json(response)
        except Exception as e:
            print(f"Processing Error: {e}")


    try:
        async for frame in websocket.iter_bytes():
            nparr = np.frombuffer(frame, np.uint8)
            frame_np = cv2.flip(cv2.imdecode(nparr, cv2.IMREAD_COLOR), 1)
            asyncio.create_task(process_frame_and_send(websocket, frame_np))
 
            # Debug Window

            # Hands
            # if hand_detector.result_frame is not None:
            #     cv2.imshow("frame", hand_detector.result_frame)
            #     cv2.waitKey(1)
            
            # Face
            # if face_detector.result_frame is not None:
            #     cv2.imshow("frame", face_detector.result_frame)
            #     cv2.waitKey(1)

        # while True:
           
        #     data = await websocket.receive_bytes()

        #     # Echo the message back
        #     await websocket.send_text(f"Server received: {data}")
            
        #     # Broadcast to all other clients if needed
        #     # for connection in active_connections:
        #     #     if connection != websocket:
        #     #         await connection.send_text(f"Broadcast: {data}")
                    
    except Exception as e:
        print(f"Websocket Error: {e}")
    finally:
        active_connections.remove(websocket)
        print("Websocket Connection closed")
        

@app.get("/")
def read_root():
    return {"message": "Hello! Stringers"}

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--port", type=int, default=12345)
    args = parser.parse_args()

    uvicorn.run(app, host="0.0.0.0", port=args.port)