using System;
using UnityEngine;

[Serializable]
public struct Expression
{
    public bool eat;
    public bool puke;
    public bool dash;
    public bool shrink;
    public bool enlarge;
}
public class ExpressionTrigger : MonoBehaviour
{
    public Expression expression;
    public GameObject planet;
    public Transform dashTarget;
    public bool inTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            other.gameObject.GetComponent<GhostFaceController>().SetExpression(expression);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            other.gameObject.GetComponent<GhostFaceController>().ResetExpression();
        }
    }

    public void MarkTriggered()
    {
        inTrigger = false;
    }
}
