using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
public class Teleporter : MonoBehaviour
{
    [SerializeField] private GhostFaceController ghostFace;
    public GameSettings gameSettings;
    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        ExpressionTrigger trigger = other.GetComponent<ExpressionTrigger>();
        if (trigger == null) return;

        if(gameSettings.autoExpression 
           && trigger.inTrigger 
           && ghostFace.expression.dash 
           || (ghostFace.IsEyeClosed &&
               ghostFace.expression.dash &&
               trigger.inTrigger &&
               trigger.dashTarget != null))

        {
            trigger.MarkTriggered();
            StartCoroutine(GetComponent<RBMoveController>().Dash(trigger.dashTarget));
        }
    }
}
