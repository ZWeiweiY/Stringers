using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject visual;
    public ParticleSystem[] explosion;
    public GameObject colliderObj;
    private PlanetShooter planetShooter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explosion = gameObject.GetComponentsInChildren<ParticleSystem>();
        planetShooter = FindFirstObjectByType<PlanetShooter>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //other.gameObject.GetComponentInChildren<PlanetShooter>()?.ResetFeatures();
            Debug.Log("Player Hurt");
        }

        else if (other.CompareTag("Planet"))
        {
            if (FindFirstObjectByType<RBMoveController>().resetting) return;
            Debug.Log("Player Hit");
            /*planetShooter?.AddFeature(gameObject);
            gameObject.SetActive(false);*/
            StartCoroutine(Explode());
            CSVLogger.Instance.AddMeteorCount();

        }


        
    }

    private IEnumerator Explode()
    {
        PlanetShooter planetShooter = FindFirstObjectByType<PlanetShooter>();
        visual.SetActive(false);
        colliderObj.SetActive(false);
        planetShooter?.AddFeature(colliderObj);

        SoundManager.Instance.PlayExplosionSound();
        float duration = 0f;
        foreach (var e in explosion)
        {
            e.Play();
            if (duration < e.main.duration) duration = e.main.duration;
        }

        yield return new WaitForSeconds(duration);
        //Destroy(explodeObject);
        planetShooter?.AddFeature(gameObject);
        visual.SetActive(true);
        gameObject.SetActive(false);

    }
}
