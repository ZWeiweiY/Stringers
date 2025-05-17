using UnityEngine;

public class UI3Manager : TutorialManager
{
    public override void CheckFinish()
    {
        base.CheckFinish();
        if (faceDataSO.action.Equals("Shrink"))
        {
            finished = true;
            levelManager.LoadNextLevel();
        }
    }
}
