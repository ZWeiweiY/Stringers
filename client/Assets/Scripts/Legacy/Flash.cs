using System;
using UnityEngine;
using UnityEngine.Events;

public class Flash : Generated
{
    public KeyCode targetKey = KeyCode.E;
    public float reactionTime = 1.5f;
    private float timer;
    public GameObject flashEffect;

    private void Awake()
    {
        if (flashEffect == null)
        {
            flashEffect = FindAnyObjectByType<FlashEffect>().gameObject;
        }
        flashEffect?.gameObject.GetComponent<FlashEffect>().TriggerFlash(0.1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(targetKey))
        {
            FlashDodged();
        }
        
        timer += Time.deltaTime;
        if (reactionTime < timer)
        {
            ScreenFlash();
        }

        
    }

    void ScreenFlash()
    {
        Debug.Log("Flashed");
        
        flashEffect?.gameObject.GetComponent<FlashEffect>().TriggerFlash(flashEffect.gameObject.GetComponent<FlashEffect>().flashDuration);
        Despawn();
    }

    void FlashDodged()
    {
        Debug.Log("Flash Dodged");
        Despawn();
    }

    public override GameObject Spawn()
    {
        return Instantiate(gameObject);
    }

    public override void Despawn()
    {
        Destroy(gameObject);
    }
}
