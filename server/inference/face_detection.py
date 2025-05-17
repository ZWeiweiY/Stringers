import time
import numpy as np
import cv2
import mediapipe as mp
import copy

from mediapipe.tasks import python
from mediapipe.tasks.python import vision
from mediapipe.python._framework_bindings import timestamp
from mediapipe import solutions
from mediapipe.framework.formats import landmark_pb2
from mediapipe.python.solutions import drawing_utils as mp_drawing
import matplotlib.pyplot as plt

Timestamp = timestamp.Timestamp

MODEL_PATH = 'models/face_landmarker.task'

class FaceDetection:
    def __init__(
            self, 
            running_mode=vision.RunningMode.LIVE_STREAM,
            num_faces: int = 2, 
            min_face_detection_confidence: float = 0.5,
            min_face_presence_confidence: float = 0.5,
            min_tracking_confidence: float = 0.5,
            output_face_blendshapes: bool = True,
            ):
        self.result_callback = self.get_mp_results
        self.base_options = python.BaseOptions(
            model_asset_path=MODEL_PATH,
            # delegate=python.BaseOptions.Delegate.GPU
            )
        
        self.options = vision.FaceLandmarkerOptions(
            running_mode=running_mode,
            num_faces=num_faces,
            min_face_detection_confidence=min_face_detection_confidence,
            min_face_presence_confidence=min_face_presence_confidence,
            min_tracking_confidence=min_tracking_confidence,
            base_options=self.base_options,
            result_callback=self.result_callback,
            output_face_blendshapes=output_face_blendshapes,
        )
        self.detector = vision.FaceLandmarker.create_from_options(self.options)

        self.result_frame = None
        self.face_yaw_score = 0
        self.face_pitch_score = 0
        self.face_roll_score = 0
        self.mouth_open_score = 0
        self.mouth_pucker_score = 0
        self.left_eye_blink_score = 0
        self.right_eye_blink_score = 0
        self.mouth_roll_score = 0
        self.face_landmarks = None

    def process(self, image: np.ndarray):
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=image)
        timestamp_ms = Timestamp.from_seconds(time.time()).microseconds()
        return self.detector.detect_async(mp_image, timestamp_ms)
    
    def get_mp_results(self, result, output_image: mp.Image, timestamp_ms: int):
        np_output_img = output_image.numpy_view()
        self.result_frame = self.draw_landmarks_on_image(np_output_img, result)
        # print(result)

        if result.face_landmarks:
            self.face_yaw_score = self.get_face_rotation(result)['Yaw']
            self.face_pitch_score = self.get_face_rotation(result)['Pitch']
            self.face_roll_score = self.get_face_rotation(result)['Roll']
            self.face_landmarks = self.get_face_landmarks(result)

        if result.face_blendshapes:
            self.mouth_open_score = self.get_mouth_open_blendshape(result)[result.face_blendshapes[0][25].category_name]
            self.mouth_pucker_score = self.get_mouth_pucker_blendshape(result)['mouthPucker']
            self.left_eye_blink_score = self.get_left_eye_blink_blendshape(result)[result.face_blendshapes[0][10].category_name]
            self.right_eye_blink_score = self.get_right_eye_blink_blendshape(result)[result.face_blendshapes[0][9].category_name]
            self.mouth_roll_score = self.get_mouth_roll_blendshape(result)['mouthRoll']

    def draw_landmarks_on_image(self, np_image, results):
        face_landmarks_results = results.face_landmarks

        annotated_image = copy.deepcopy(np_image)
        for idx in range(len(face_landmarks_results)):
            face_landmarks = face_landmarks_results[idx]

            face_landmarks_proto = landmark_pb2.NormalizedLandmarkList()
            face_landmarks_proto.landmark.extend([
                landmark_pb2.NormalizedLandmark(x=landmark.x, y=landmark.y, z=landmark.z) for landmark in face_landmarks
            ])

            mp_drawing.draw_landmarks(
                image=annotated_image,
                landmark_list=face_landmarks_proto,
                connections=mp.solutions.face_mesh.FACEMESH_TESSELATION,
                landmark_drawing_spec=None,
                connection_drawing_spec=mp.solutions.drawing_styles.get_default_face_mesh_tesselation_style()
            )
            # mp_drawing.draw_landmarks(
            #     image=annotated_image,
            #     landmark_list=face_landmarks_proto,
            #     connections=mp.solutions.face_mesh.FACEMESH_CONTOURS,
            #     landmark_drawing_spec=None,
            #     connection_drawing_spec=mp.solutions.drawing_styles.get_default_face_mesh_contours_style()
            # )

            # mp_drawing.draw_landmarks(
            #     image=annotated_image,
            #     landmark_list=face_landmarks_proto,
            #     connections=mp.solutions.face_mesh.FACEMESH_IRISES,
            #     landmark_drawing_spec=None,
            #     connection_drawing_spec=mp.solutions.drawing_styles.get_default_face_mesh_iris_connections_style()
            # )

        return annotated_image

    def get_face_rotation(self, result):
        if result.face_landmarks is None or len(result.face_landmarks) == 0:
            return {"Yaw" : 0, "Pitch" : 0, "Roll": 0}
        # Get the rotation of the face
        landmarks = result.face_landmarks[0]  # First detected face

         # Define 2D image points from MediaPipe landmark indices
        image_points = np.array([
            (landmarks[1].x, landmarks[1].y),   # Nose tip
            (landmarks[33].x, landmarks[33].y), # Left eye
            (landmarks[263].x, landmarks[263].y), # Right eye
            (landmarks[61].x, landmarks[61].y), # Left mouth
            (landmarks[291].x, landmarks[291].y), # Right mouth
            (landmarks[199].x, landmarks[199].y) # Chin
        ], dtype=np.float64)

        # Convert normalized [0,1] coordinates to pixel coordinates
        image_points *= np.array([self.result_frame.shape[1], self.result_frame.shape[0]])

        # 3D model points
        model_points = np.array([
            (image_points[0][0], image_points[0][1], landmarks[1].z),   # Nose tip
            (image_points[1][0], image_points[1][1], landmarks[33].z), # Left eye
            (image_points[2][0], image_points[2][1], landmarks[263].z), # Right eye
            (image_points[3][0], image_points[3][1], landmarks[61].z), # Left mouth
            (image_points[4][0], image_points[4][1], landmarks[291].z), # Right mouth
            (image_points[5][0], image_points[5][1], landmarks[199].z)  # Chin
        ], dtype=np.float64)

        # Camera internals
        focal_length = self.result_frame.shape[1]
        camera_matrix = np.array([
            [focal_length, 0, self.result_frame.shape[1] / 2],
            [0, focal_length, self.result_frame.shape[0] / 2],
            [0, 0, 1]
        ], dtype=np.float64)
        
        dist_coeffs = np.zeros((4, 1), dtype=np.float64)  # Assuming no lens distortion

        # Solve PnP: get rotation vector (rvec) and translation vector (tvec)
        success, rvec, tvec = cv2.solvePnP(model_points, image_points, camera_matrix, dist_coeffs)

        if success:
            # Convert rotation vector to rotation matrix
            rmat, _ = cv2.Rodrigues(rvec)

            angles = cv2.RQDecomp3x3(rmat)[0]  # Get first returned value

            pitch = angles[0] 
            yaw = angles[1]

            # Roll (Tilt Left/Right)
            left_eye = landmarks[133]
            right_eye = landmarks[362]
            eye_slope = (right_eye.y - left_eye.y) / (right_eye.x - left_eye.x)

        return {"Yaw" : yaw, "Pitch" : pitch, "Roll": eye_slope}
    

    def get_mouth_open_blendshape(self, result):
        if result.face_blendshapes is None or len(result.face_blendshapes) == 0:
            return None
        mouth_blendshape = result.face_blendshapes[0][25]
        return { mouth_blendshape.category_name : mouth_blendshape.score }
    
    def get_mouth_pucker_blendshape(self, result):
        if result.face_blendshapes is None or len(result.face_blendshapes) == 0:
            return None
        blendshapes = []
        blendshapes.append(result.face_blendshapes[0][32])
        blendshapes.append(result.face_blendshapes[0][38])

        top_blendshape = max(blendshapes, key=lambda b: b.score)
        return { "mouthPucker" : top_blendshape.score }

    def get_left_eye_blink_blendshape(self, result):
        if result.face_blendshapes is None or len(result.face_blendshapes) == 0:
            return None
        left_eye_blendshape = result.face_blendshapes[0][10] # This is opposite for purpose (frame was flipped)
        return { left_eye_blendshape.category_name : left_eye_blendshape.score }

    def get_right_eye_blink_blendshape(self, result):
        if result.face_blendshapes is None or len(result.face_blendshapes) == 0:
            return None
        right_eye_blendshape = result.face_blendshapes[0][9] # This is opposite for purpose (frame was flipped)
        return { right_eye_blendshape.category_name : right_eye_blendshape.score }
    
    def get_mouth_roll_blendshape(self, result):
        if result.face_blendshapes is None or len(result.face_blendshapes) == 0:
            return None
        blendshapes = []
        blendshapes.append(result.face_blendshapes[0][40])
        blendshapes.append(result.face_blendshapes[0][41])

        top_blendshape = max(blendshapes, key=lambda b: b.score)
        return { "mouthRoll" : top_blendshape.score }
    
    
    def get_face_landmarks(self, result):
        if result.face_landmarks is None or len(result.face_landmarks[0]) == 0:
            return None
        landmarks = [{"x" : landmark.x, "y": landmark.y, "z": landmark.z} for landmark in result.face_landmarks[0]]
        return landmarks
            