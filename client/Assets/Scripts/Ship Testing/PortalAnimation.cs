using System;
using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RBMoveController rb = other.gameObject.GetComponent<RBMoveController>();
            rb.LevelClear();
            CSVLogger.Instance.LogData();
            ProgressTracker.Instance.ResetRound();
            StartCoroutine(GameManager.Instance.NextLevel());
            UIManager.Instance.ShowEndPanel();
        }
    }
}
