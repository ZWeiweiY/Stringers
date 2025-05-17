using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 2.0f; // Controls how smoothly it stops

    private float shakeTimer;
    private Vector3 originalLocalPosition;

    void Start()
    {
        originalLocalPosition = transform.localPosition; // Store the camera’s initial local position
    }

    [ContextMenu("Shake Camera")]
    public void TriggerShake()
    {
        shakeTimer = shakeDuration;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            float currentMagnitude = shakeMagnitude * (shakeTimer / shakeDuration); // Smoothly decrease shake effect

            // Shake effect (adds random small movement)
            transform.localPosition = originalLocalPosition + Random.insideUnitSphere * currentMagnitude;
        }
        else
        {
            // Smoothly return to original position
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, Time.deltaTime * dampingSpeed);
        }
    }
    
}
