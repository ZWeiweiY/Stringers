using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCounter : MonoBehaviour
{
    [SerializeField] private float initCounter = 1;
    [SerializeField] private float counter;
    [SerializeField] private bool dead = false;
    public UnityEvent OnDeath;

    private void Awake()
    {
        counter = initCounter;
        OnDeath.AddListener(Fallen);
    }

    void Update()
    {
        if (counter == 0 && !dead)
        {
            dead = true;
            OnDeath.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet")) return;
        counter++;
        dead = false;
    }

    private void OnTriggerExit(Collider other)
    {
        counter--;
    }

    private void Fallen()
    {
        gameObject.GetComponent<RBMoveController>()?.StopMovement();
        StartCoroutine(gameObject.GetComponent<CharacterDissolve>()?.DissolveCo());
    }
}
