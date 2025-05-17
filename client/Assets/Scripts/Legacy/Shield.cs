using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyBullet>() != null)
        {
            Debug.Log("blocked");
            other.GetComponent<EnemyBullet>().Blocked();
        }
    }
}
