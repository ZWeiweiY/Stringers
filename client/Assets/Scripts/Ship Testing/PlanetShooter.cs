using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlanetShooter : MonoBehaviour
{
    public GameSettings gameSettings;
    [SerializeField] private float eatShrinkDuration = 2f;
    [SerializeField] private float shootSpeed = 10f;
    [SerializeField] private float pukeCooldown = 1.5f; // Time buffer for puking
    //[SerializeField] private GameObject ghost;
    //[SerializeField] private FaceDataSO faceData;
    [SerializeField] private GhostFaceController ghostFace;

    public List<GameObject> planets;
    public List<GameObject> initalFeatures;

    //private bool isMouthOpen = false;
    //private SkinnedMeshRenderer skinnedMeshRenderer;
    public float currentCooldown = 0f;

    public bool isEating = false;

    //levelUp VFX
    public VisualEffect levelUp;
    private bool levelingUp = false;



    private void Awake()
    {
        planets = new List<GameObject>();
        initalFeatures = new List<GameObject>();
        levelUp.enabled = false;
    }

    private void Start()
    {
        //skinnedMeshRenderer = ghost.GetComponent<SkinnedMeshRenderer>();
    }


    private void Update()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.GetComponent<Enlarge>())
        //{
        //    GetComponent<RBMoveController>().Enlarge();
        //    return;
        //}
        ExpressionTrigger trigger = other.GetComponent<ExpressionTrigger>();
        if (trigger == null) return;
        if (trigger.planet != null)
        {
            Planet planet = trigger.planet.GetComponent<Planet>();
            if (gameSettings.autoExpression 
                && trigger.inTrigger 
                && ghostFace.expression.eat
                || (planet != null 
                    && planet.canEat
                    && ghostFace.IsMouthOpen
                    && ghostFace.expression.eat
                    && !isEating
                    && trigger.inTrigger))
            {
                isEating = true;
                trigger.MarkTriggered();
                AddFeature(planet.gameObject);
                CSVLogger.Instance.AddPlanetCount();
                planet.gameObject.SetActive(false);
                GameObject planetClone = Instantiate(planet.gameObject, planet.transform, true);
                EatPlanet(planetClone.gameObject);
            }
        }
        
        if (gameSettings.autoExpression 
            && trigger.inTrigger 
            && ghostFace.expression.puke 
            || (ghostFace.expression.puke 
                && ghostFace.IsMouthFunnel 
                && currentCooldown <= 0f 
                && !isEating
                && trigger.inTrigger))
        {
            trigger.MarkTriggered();
            PukePlanet();
        }
        //else if (trigger.expression.shrink)
        //{
        //    GetComponent<RBMoveController>().Shrink();
        //}
    }

    public void EatPlanet(GameObject planet)
    {
        planets.Add(planet);
        //StartCoroutine(ShrinkPlanetOverTime(planet));
        /*isEating = true;*/
        StartCoroutine(EatPlanetWithMouth(planet));
        if (!levelingUp)
        {
            levelUp.enabled = true;
            if (levelUp != null)
                levelUp.Play();
            levelingUp = true;
            StartCoroutine(ResetLevelUpFlag(0.5f));
        }
    }

    IEnumerator ResetLevelUpFlag(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        levelingUp = false;
    }

    private IEnumerator EatPlanetWithMouth(GameObject planet)
    {
        // Eat the planet (Shrink + Hide)
        yield return StartCoroutine(ShrinkPlanetOverTime(planet));
        isEating = false;
    }

    private IEnumerator ShrinkPlanetOverTime(GameObject planet)
    {
        // Parent to player so it follows the player
        planet.transform.SetParent(transform);
        planet.SetActive(true);
        planet.GetComponent<Collider>().enabled = false;

        Vector3 startScale = planet.transform.localScale;
        Vector3 endScale = Vector3.zero;
        Vector3 startPos = planet.transform.localPosition;
        Vector3 endPos = Vector3.up * 2;
        startPos.z = 0;
        startPos.x = 0;
        float elapsed = 0f;

        SoundManager.Instance.PlayEatPlanetSound();
        while (elapsed < eatShrinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / eatShrinkDuration;
            planet.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            planet.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }


        planet.transform.localPosition = Vector3.up * 2;
        planet.SetActive(false);
        planet.transform.localScale = Vector3.one;
    }

    public void PukePlanet()
    {
        if (GetComponent<RBMoveController>().resetting || GetComponent<RBMoveController>().stopped) return;
        if (planets.Count == 0) return;

        GameObject pukedPlanet = planets[^1];
        planets.RemoveAt(planets.Count - 1);
        pukedPlanet.transform.localScale = Vector3.one; // Reset size
        pukedPlanet.SetActive(true);
        pukedPlanet.GetComponent<Collider>().enabled = true;
        pukedPlanet.transform.SetParent(null); // Detach

        Rigidbody rb = pukedPlanet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * shootSpeed;
        }

        currentCooldown = pukeCooldown;

        SoundManager.Instance.PlayFiringSound();

        Debug.Log("Puked planet");
    }

    public void ClearPlanets()
    {
        foreach (GameObject planet in planets)
        {
            Destroy(planet);
        }
        planets.Clear();
    }

    public void AddFeature(GameObject gameObject)
    {
        initalFeatures.Add(gameObject);
    }

    public void ResetFeatures()
    {
        foreach (GameObject feature in initalFeatures)
        {
            feature.SetActive(true);
        }
        initalFeatures.Clear();
    }
}
