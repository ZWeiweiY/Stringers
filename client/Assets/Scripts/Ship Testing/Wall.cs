using System;
using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private bool processingCollision = false;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !processingCollision)
        {
            if (other.gameObject.GetComponent<RBMoveController>().resetting) return;

            processingCollision = true;
            Debug.Log(other.gameObject.name);
            StartCoroutine(HandlePlayerCollision(other.gameObject));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !processingCollision)
        {
            if (other.gameObject.GetComponent<RBMoveController>().resetting) return;
            processingCollision = true;
            other.gameObject.GetComponentInChildren<CameraShake>()?.TriggerShake();
            StartCoroutine(HandlePlayerCollision(other.gameObject));
        }
    }

    private IEnumerator HandlePlayerCollision(GameObject player)
    {
        SoundManager.Instance.PlayDeathSound();
        yield return StartCoroutine(GameManager.Instance.RestartGame());
        processingCollision = false;
    }
}
