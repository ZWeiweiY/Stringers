using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    private static float[] linearSpeed = { 9f, 12f, 14f };
    private static float[] angularSpeed = { 120f, 160f, 179.9f };
    public int[] index = { 0, 0, 0 };

    public bool autoExpression;

    public void IncreaseIndex(int i)
    {
        index[i]++;
    }
    
    public void DecreaseIndex(int i)
    {
        index[i]--;
    }

    public float GetLinearSpeed(int i)
    {
        return linearSpeed[index[i]];
    }

    public float GetAngularSpeed(int i)
    {
        return angularSpeed[index[i]];
    }

    public void ResetIndex()
    {
        index = new []{0, 0, 0};
    }
}
