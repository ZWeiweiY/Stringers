import time
import numpy as np
import mediapipe as mp
import cv2
import copy

from mediapipe.tasks import python
from mediapipe.tasks.python import vision
from mediapipe.python._framework_bindings import timestamp
from mediapipe import solutions
from mediapipe.framework.formats import landmark_pb2
from mediapipe.python.solutions import drawing_utils as mp_drawing


Timestamp = timestamp.Timestamp

MODEL_PATH = 'models/hand_landmarker.task'

class HandDetection:
    def __init__(
            self, 
            running_mode=vision.RunningMode.LIVE_STREAM, 
            num_hands: int = 2,
            min_hand_detection_confidence: float = 0.25,
            min_hand_presence_confidence: float = 0.4,
            min_tracking_confidence: float = 0.45,
            ):
        self.result_callback = self.get_mp_results
        self.base_options = python.BaseOptions(
            model_asset_path=MODEL_PATH,
            # delegate=python.BaseOptions.Delegate.GPU
            )
        
        self.options = vision.HandLandmarkerOptions(
            running_mode=running_mode,
            min_hand_detection_confidence=min_hand_detection_confidence,
            min_hand_presence_confidence=min_hand_presence_confidence,
            min_tracking_confidence=min_tracking_confidence,
            num_hands=num_hands,
            base_options=self.base_options,
            result_callback=self.result_callback
        )
        self.detector = vision.HandLandmarker.create_from_options(self.options)
        self.left_hand_detected : bool = False
        self.right_hand_detected : bool = False
        self.result_frame = None

    def process(self, image: np.ndarray):
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=image)
        timestamp_ms = Timestamp.from_seconds(time.time()).microseconds()
        return self.detector.detect_async(mp_image, timestamp_ms)
    
    def get_mp_results(self, result, output_image: mp.Image, timestamp_ms: int):
        np_output_img = output_image.numpy_view()
        self.update_hand_detection_status(result.handedness)
        self.result_frame = self.draw_landmarks_on_image(np_output_img, result)
        print(f"Inference Time: {time.time() - timestamp_ms}")



    # Local Methods

    def update_hand_detection_status(self, handedness_results):
        if(len(handedness_results) == 2):
            for hand in handedness_results:
                if(hand[0].index == 0):
                    self.right_hand_detected = True
                if(hand[0].index == 1):
                    self.left_hand_detected = True
        elif(len(handedness_results) == 1):
            if(handedness_results[0][0].index == 0):
                self.right_hand_detected = True
                self.left_hand_detected = False
            if(handedness_results[0][0].index == 1):
                self.left_hand_detected = True
                self.right_hand_detected = False
        else:
            self.right_hand_detected = False
            self.left_hand_detected = False

        # Debug Print
        # if(self.right_hand_detected):
        #     print("Right Hand Detected")
        # if(self.left_hand_detected):
        #     print("Left Hand Detected \n")

    # Debug Usage

    def draw_landmarks_on_image(self, np_image, results):

        hand_landmarks_results = results.hand_landmarks

        # Loop through the detected hands to visualize.
        if(hand_landmarks_results is not None):
            annotated_image = copy.deepcopy(np_image)
            height, width, _ = annotated_image.shape
            # print(len(hand_landmarks_results))

            if(len(hand_landmarks_results) >= 1):
                hand_landmarks_result = hand_landmarks_results[0]

                # Draw the hand landmarks.
                hand_landmarks_proto = landmark_pb2.NormalizedLandmarkList()
                hand_landmarks_proto.landmark.extend([
                    landmark_pb2.NormalizedLandmark(x=landmark.x, y=landmark.y, z=landmark.z) for landmark in hand_landmarks_result
                ])
                mp_drawing.draw_landmarks(
                    annotated_image,
                    hand_landmarks_proto,
                    solutions.hands.HAND_CONNECTIONS,
                    solutions.drawing_styles.get_default_hand_landmarks_style(),
                    solutions.drawing_styles.get_default_hand_connections_style()
                )
                # x_coords = [hand_landmark_result[i].x for i in range(21)]
                # y_coords = [hand_landmark_result[i].y for i in range(21)]
            if len(hand_landmarks_results) == 2:

                hand_landmarks_result = hand_landmarks_results[1]

                # Draw the hand landmarks.
                hand_landmarks_proto = landmark_pb2.NormalizedLandmarkList()
                hand_landmarks_proto.landmark.extend([
                    landmark_pb2.NormalizedLandmark(x=landmark.x, y=landmark.y, z=landmark.z) for landmark in hand_landmarks_result
                ])
                mp_drawing.draw_landmarks(
                    annotated_image,
                    hand_landmarks_proto,
                    solutions.hands.HAND_CONNECTIONS,
                    solutions.drawing_styles.get_default_hand_landmarks_style(),
                    solutions.drawing_styles.get_default_hand_connections_style()
                )
                # x_coords = [hand_landmark_result[i].x for i in range(21)]
                # y_coords = [hand_landmark_result[i].y for i in range(21)]
            return annotated_image
        
        # x_coordinates = [landmark.x for landmark in hand_landmarks_results]
        # y_coordinates = [landmark.y for landmark in hand_landmarks_results]
        # text_x = int(min(x_coordinates) * width)
        # text_y = int(min(y_coordinates) * height) - 10

        return np_image


    