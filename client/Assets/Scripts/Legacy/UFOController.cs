using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFOController : Generated
{
    public float xMoveSpeed = 2.5f;
    public float zMoveSpeed = 1.5f;
    public float fireRate = 0.5f;
    public GameObject bullet;
    public float zRandomTimeBase = .75f;
    public float zRandomTimeMax = .75f;
    private float nextFireTime;
    private float nextZRandomTime;
    private float zMove;
    public Transform[] spawnPoints;
    public float spawnRadius;
    private int spawnPointIndex;
    public event Action ExplosionActionTriggered;

    void FireBullet()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        nextFireTime = Time.time + fireRate;
    }

    void UFOMovement()
    {
        if (Time.time >= nextZRandomTime)
        { 
            zMove = Random.Range(-zMoveSpeed, zMoveSpeed);
            nextZRandomTime = Time.time + Random.Range(zRandomTimeBase, zRandomTimeBase + zRandomTimeMax);
        }
        transform.position +=  zMove* Time.deltaTime * Vector3.forward;
        Vector3 horizontal = transform.right * (Time.deltaTime * xMoveSpeed);
        if (spawnPointIndex == 0)
        {
            horizontal = -horizontal;
        }
        transform.position += horizontal;
    }

    private void Awake()
    {
        zMove = Random.Range(-zMoveSpeed, zMoveSpeed);
    }

    private void Update()
    {
        UFOMovement();
        if (Time.time >= nextFireTime)
        {
            FireBullet();
        }
    }

    public override GameObject Spawn()
    {
        spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnPointIndex].position + Vector3.forward * spawnRadius;
        return Instantiate(gameObject, spawnPosition, transform.rotation);
    }

    public override void Despawn()
    {
        Destroy(gameObject);
        ExplosionActionTriggered?.Invoke();
    }

    public void Hit()
    {
        //TODO: Play effects
        Despawn();
    }
}
