using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct ObjectToGenerate
{
    public GameObject gameObject;
    public float weight;
}
public class STGObjectGenerator : MonoBehaviour
{
    public static STGObjectGenerator Generator;
    public ObjectToGenerate[] objects;
    public int maxInScreen = 1;
    public float generateIntervalBase = 2.5f;
    public float generateIntervalRandomness = 1.5f;
    private float nextGenerateTime;
    private float totalWeight;
    [SerializeField] private List<GameObject> generatedObjects;

    private void Awake()
    {
        if (Generator == null) { Generator = this; } else { Destroy(gameObject); }
        GetNewGenerateTime();
        foreach (ObjectToGenerate obj in objects)
        {
            totalWeight += obj.weight;
        }
        
    }

    private void Update()
    {
        if (Time.time >= nextGenerateTime && generatedObjects.Count < maxInScreen)
        {
            GenerateObject();
            GetNewGenerateTime();
        }
    }

    void GetNewGenerateTime()
    {
        nextGenerateTime = Time.time + generateIntervalBase + Random.Range(0, generateIntervalRandomness);
    }

    void GenerateObject()
    {
        Debug.Log("Generating new object");
        float randomWeight = Random.Range(0, totalWeight);
        for (int i = 0; i < objects.Length; i++)
        {
            randomWeight -= objects[i].weight;
            if (randomWeight <= 0)
            {
                generatedObjects.Add(objects[i].gameObject.GetComponent<Generated>()?.Spawn());
                return;
            }
        }
    }

    public void RemoveObjectFromList(GameObject obj)
    {
        generatedObjects.Remove(obj);
    }
}
