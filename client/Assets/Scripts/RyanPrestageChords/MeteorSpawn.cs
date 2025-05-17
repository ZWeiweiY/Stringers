using UnityEngine;
using System.Collections;

public class MeteorSpawn : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] meteor;
    public float fixedSpawnTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SpawnTheMeteor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnTheMeteor() 
    {
        float m = Random.Range(0f, 1f);
        Vector3 spawnLine = Vector3.Lerp(startPoint.position, endPoint.position, m);
        int randomIndex = Random.Range(0, meteor.Length);
        GameObject meteorToSpawn = meteor[randomIndex];
        Instantiate(meteorToSpawn, spawnLine, Quaternion.identity);
        StartCoroutine(SpawnAfterDelay());
    }
    IEnumerator SpawnAfterDelay() 
    {
        yield return new WaitForSeconds(fixedSpawnTime);
        SpawnTheMeteor();
    }
}
