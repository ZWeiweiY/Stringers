using System;
using UnityEngine;

public class SecondStage : MonoBehaviour
{
    public static SecondStage instance;
    public float bpm = 60f;
    public float horizontalSpacing;
    public float verticalSpacing;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
