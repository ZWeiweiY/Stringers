using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public LevelManager levelManager;
    public FaceDataSO faceDataSO;
    public Texture preTexture;
    public Texture postTexture;
    protected bool finished;
    public float cooldown = 5f;
    private float cooldownTimer;

    private void Awake()
    {
        Reset();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if (cooldownTimer <= 0 && !finished)
        {
            CheckFinish();
        }
    }

    public virtual void CheckFinish()
    {
        gameObject.GetComponent<RawImage>().texture = postTexture;
    }

    public virtual void Reset()
    {
        gameObject.GetComponent<RawImage>().texture = preTexture;
        cooldownTimer = cooldown;
        finished = false;
    }
}
