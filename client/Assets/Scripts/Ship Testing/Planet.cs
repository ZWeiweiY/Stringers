using System;
using UnityEngine;
public class Planet : MonoBehaviour
{
    public GameObject planetPrefab;
    public bool canEat = false;
    public ExpressionTrigger expression;

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            canEat = true;
            return;
        }*/

        if (other.CompareTag("Meteor"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (expression.inTrigger)
        {
            canEat = true;
        }
        else
        {
            canEat = false;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEat = false;
        }
    }*/
}
