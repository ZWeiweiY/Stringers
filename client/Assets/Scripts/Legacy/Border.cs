using System;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        other.gameObject.GetComponent<Generated>()?.Despawn();
    }
}
