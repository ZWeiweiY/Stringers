using UnityEngine;

public class UI1Manager : TutorialManager
{
    public float threshold = 0.2f;
    public override void CheckFinish()
    {
        base.CheckFinish();
        if (Mathf.Abs(faceDataSO.Roll) > threshold)
        {
            finished = true;
            levelManager.LoadNextLevel();
        }
    }
}
