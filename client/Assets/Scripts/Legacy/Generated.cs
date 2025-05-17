using System;
using UnityEngine;

public abstract class Generated: MonoBehaviour
{
    public abstract GameObject Spawn();

    public abstract void Despawn();

    private void OnDestroy()
    {
        STGObjectGenerator.Generator.RemoveObjectFromList(gameObject);
    }
}
