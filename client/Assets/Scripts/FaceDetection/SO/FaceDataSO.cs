using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FaceDataSO", menuName = "Scriptable Objects/FaceDataSO")]
public class FaceDataSO : ScriptableObject
{
    public float Yaw;
    public float Pitch;
    public float Roll;
    public string action;
    public float mouthOpenActionScore;
    public float mouthPuckerActionScore;
    public float leftEyeBlinkActionScore;
    public float rightEyeBlinkActionScore;
    public float mouthRollActionScore;
    public bool lastPressed;
    public string lastAction;
    public bool guardFinish;

    public List<Vector3> landmarkList { get; private set; } = new List<Vector3>();
    private float landmarkScaleFactor = 10000f;

    public const float yawThreshold = 0.06f;
    public const float pitchOffset = -0.035f;
    public const float pitchMax = 0.115f;
    public const float rollThreshold = 0.15f;
    public const float mouthOpenThreshold = 0.2f;
    public const float mouthPuckerThreshold = 0.4f;
    public const float mouthPuckerOffset = 0.35f;
    public const float eyeBlinkThreshold = 0.5f;
    public const float eyeBlinkMin = 0.45f;
    public const float eyeBlinkMax = 0.7f;
    public const float mouthRollThreshold = 0.5f;


    private void OnEnable()
    {
        if (landmarkList == null)
        {
            landmarkList = new List<Vector3>(new Vector3[478]);
        }
    }

    public void SetFaceRotation(string yaw, string pitch, string roll)
    {
        Yaw = Mathf.Abs(float.Parse(yaw)) >= yawThreshold ? Mathf.Clamp(float.Parse(yaw) * 10, -1, 1) : 0;
        Pitch = Mathf.Abs(float.Parse(pitch) + pitchOffset) >= pitchOffset ? Mathf.Clamp((float.Parse(pitch) + pitchOffset) / pitchMax, -1, 1) : 0;
        Roll = Mathf.Abs(float.Parse(roll)) >= rollThreshold ? Mathf.Clamp(float.Parse(roll), -1, 1) : 0;
    }

    public void SetAction(string mouthOpenScore, string mouthPuckerScore, string leftEyeBlinkScore, string rightEyeBlinkScore, string mouthRollScore)
    {
        if (
            string.IsNullOrEmpty(mouthOpenScore) || 
            string.IsNullOrEmpty(mouthPuckerScore) ||
            string.IsNullOrEmpty(leftEyeBlinkScore) ||
            string.IsNullOrEmpty(rightEyeBlinkScore) ||
            string.IsNullOrEmpty(mouthRollScore)
            )
        {
            action = "None";
            return;
        }

        mouthOpenActionScore = Mathf.Abs(float.Parse(mouthOpenScore)) >= mouthOpenThreshold ? Mathf.Clamp01(float.Parse(mouthOpenScore)) : 0;
        mouthPuckerActionScore = Mathf.Abs(float.Parse(mouthPuckerScore) - mouthPuckerOffset) >= mouthPuckerThreshold ? Mathf.Clamp01(float.Parse(mouthPuckerScore) - mouthPuckerOffset) : 0;
        leftEyeBlinkActionScore = Mathf.Abs(float.Parse(leftEyeBlinkScore)) >= eyeBlinkThreshold ? Mathf.Clamp01((float.Parse(leftEyeBlinkScore) - eyeBlinkMin) / (eyeBlinkMax - eyeBlinkMin)) : 0;
        rightEyeBlinkActionScore = Mathf.Abs(float.Parse(rightEyeBlinkScore)) >= eyeBlinkThreshold ? Mathf.Clamp01((float.Parse(rightEyeBlinkScore) - eyeBlinkMin) / (eyeBlinkMax - eyeBlinkMin)) : 0;
        mouthRollActionScore = Mathf.Abs(float.Parse(mouthRollScore)) >= mouthRollThreshold ? Mathf.Clamp01(float.Parse(mouthRollScore)) : 0;

        if (mouthOpenActionScore > mouthPuckerActionScore && mouthOpenActionScore > mouthRollActionScore)
        {
            action = "Eat";
        }
        else if (mouthPuckerActionScore > mouthOpenActionScore && mouthPuckerActionScore > mouthRollActionScore)
        {
            action = "Shoot";
        }
        else if (leftEyeBlinkActionScore > 0 || rightEyeBlinkActionScore > 0)
        {
            action = "Dash";
        }
        else if (mouthRollActionScore > mouthOpenActionScore && mouthRollActionScore > mouthPuckerActionScore)
        {
            action = "Shrink";
        }
        else
        {
            action = "None";
        }


        //if (string.IsNullOrEmpty(blendShape)) {
        //    action = "None";
        //    return;
        //}
        //if (blendShape == "jawOpen")
        //{
        //    action = "Eat";
        //}
        //else if (blendShape == "mouthFunnel" || blendShape == "mouthPucker")
        //{
        //    action = "Shoot";
        //}
        //else if (blendShape == "mouthRollLower" || blendShape == "mouthRollUpper" || blendShape == "mouthShrugLower" || blendShape == "mouthShrugUpper")
        //{
        //    action = "Shield";
        //}
        //else
        //{
        //    Debug.Log(blendShape);
        //    action = "None";
        //}

        //if (guardFinish)
        //{
        //    guardFinish = false;
        //}
        //if (lastAction != blendShape)
        //{
        //    lastPressed = true;

        //    lastAction = blendShape;
        //}
        //else
        //{
        //    lastPressed = false;
        //}
    }

    public void SetLandmarks(FaceLandmarks[] landmarks)
    {

        if (landmarks == null || landmarks.Length < 478)
        {
            Debug.LogWarning("Received landmarks are null or do not contain 478 points.");
            return;
        }

        if (landmarkList.Count != 478)
        {
            Debug.LogWarning("vertexList size is incorrect. Reinitializing...");
            landmarkList = new List<Vector3>(new Vector3[478]);
        }

        for (int i = 0; i < landmarks.Length; i++)
        {
            landmarkList[i] = new Vector3(
                (landmarks[i].x * landmarkScaleFactor) / landmarkScaleFactor,
                (landmarks[i].y * landmarkScaleFactor) / landmarkScaleFactor,
                (landmarks[i].z * landmarkScaleFactor) / landmarkScaleFactor
            );
        }
    }
}
