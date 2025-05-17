using UnityEngine;

public class UI2Manager : TutorialManager
{
    public override void CheckFinish()
    {
        base.CheckFinish();
        if (faceDataSO.action.Equals("Eat"))
        {
            finished = true;
            levelManager.LoadNextLevel();
        }
    }
}
