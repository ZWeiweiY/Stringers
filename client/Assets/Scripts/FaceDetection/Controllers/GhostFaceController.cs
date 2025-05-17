using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFaceController : MonoBehaviour
{
    [SerializeField] private FaceDataSO faceData;
    [SerializeField] private GameObject ghostBlendShape;
    [SerializeField] private GameObject ghostLeftEyeBlendShape;
    [SerializeField] private GameObject ghostRightEyeBlendShape;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private SkinnedMeshRenderer leftEyeSkinnedMeshRenderer;
    private SkinnedMeshRenderer rightEyeSkinnedMeshRenderer;

    public bool IsMouthOpen { get; private set; }
    public bool IsMouthFunnel { get; private set; }
    public bool IsMouthRoll { get; private set; }

    public bool IsEyeClosed; 
    //{ get; private set; }
    
    private bool isLeftEyeClosed;
    private bool isRightEyeClosed;

    public Expression expression;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        IsMouthOpen = false;
        IsMouthFunnel = false;
        IsMouthRoll = false;
        expression = new Expression
        {
            dash = false,
            eat = false,
            puke = false,
            shrink = false
        };
    }
    private void Start()
    {
        skinnedMeshRenderer = ghostBlendShape.GetComponent<SkinnedMeshRenderer>();
        leftEyeSkinnedMeshRenderer = ghostLeftEyeBlendShape.GetComponent<SkinnedMeshRenderer>();
        rightEyeSkinnedMeshRenderer = ghostRightEyeBlendShape.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (faceData.action == "Eat")
        {
            if (!IsMouthOpen)
            {
                IsMouthOpen = true;
                StartCoroutine(AnimateBlendshape(0, 100, 0.2f)); // Open mouth
            }
        }
        else if (faceData.action != "Eat" && IsMouthOpen)
        {
            IsMouthOpen = false;
            StartCoroutine(AnimateBlendshape(0, 0, 0.2f)); // Close mouth
        }


        if (faceData.action == "Shoot")
        {
            if (!IsMouthFunnel)
            {
                IsMouthFunnel = true;
                StartCoroutine(AnimateBlendshape(2, 100, 0.1f));
                StartCoroutine(AnimateBlendshape(8, 100, 0.1f)); // Shoot
                StartCoroutine(AnimateBlendshape(9, 100, 0.1f)); // Hand
                StartCoroutine(AnimateBlendshape(11, 100, 0.1f)); // Back Force
                StartCoroutine(AnimateEyeBlendshape(0, 100, 0.1f)); // Front Force

            }
        }
        else if (faceData.action != "Shoot" && IsMouthFunnel)
        {
            IsMouthFunnel = false;
            StartCoroutine(AnimateBlendshape(2, 0, 0.2f));
            StartCoroutine(AnimateBlendshape(8, 0, 0.2f)); // Unshoot
            StartCoroutine(AnimateBlendshape(9, 0, 0.2f)); // Unhand
            StartCoroutine(AnimateBlendshape(11, 0, 0.2f)); // Recenter
            StartCoroutine(AnimateEyeBlendshape(0, 0, 0.2f)); // Front Force
        }

        if (faceData.action == "Shrink")
        {
            if (!IsMouthRoll)
            {
                IsMouthRoll = true;
                //StartCoroutine(AnimateBlendshape(1, 60, 0.5f)); // Shrink
                //StartCoroutine(AnimateBlendshape(3, 100, 0.5f));
                //StartCoroutine(AnimateBlendshape(6, 100, 0.5f));
                //StartCoroutine(AnimateBlendshape(7, 100, 0.5f));
            }
        }
        else if (faceData.action != "Shrink" && IsMouthRoll)
        {
            IsMouthRoll = false;
        }

        // Left Eye Blink
        if (faceData.leftEyeBlinkActionScore > 0.45f)
        {
            isLeftEyeClosed = true;
            StartCoroutine(AnimateBlendshape(5, 100, 0.1f));

        }
        else
        {
            isLeftEyeClosed = false;
            StartCoroutine(AnimateBlendshape(5, 0, 0.1f));
        }

        // Right Eye Blink
        if (faceData.rightEyeBlinkActionScore > 0.45f)
        {
            isRightEyeClosed = true;
            StartCoroutine(AnimateBlendshape(4, 100, 0.1f));
        }
        else
        {
            isRightEyeClosed = false;
            StartCoroutine(AnimateBlendshape(4, 0, 0.1f));
        }

        IsEyeClosed = isLeftEyeClosed || isRightEyeClosed;
    }

    public void SetExpression(Expression e)
    {
        expression = e;
    }

    public void ResetExpression()
    {
        expression = new Expression
        {
            dash = false,
            eat = false,
            puke = false,
            shrink = false
        };
    }

    private IEnumerator AnimateBlendshape(int blendShapeIndex, float targetWeight, float duration)
    {
        if (skinnedMeshRenderer == null) yield break;

        float startWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, Mathf.Lerp(startWeight, targetWeight, t));
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
    }

    private IEnumerator AnimateEyeBlendshape(int blendShapeIndex, float targetWeight, float duration)
    {
        if (leftEyeSkinnedMeshRenderer == null || rightEyeSkinnedMeshRenderer == null) yield break;
        float startWeight = leftEyeSkinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            leftEyeSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, Mathf.Lerp(startWeight, targetWeight, t));
            rightEyeSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, Mathf.Lerp(startWeight, targetWeight, t));
            yield return null;
        }
        leftEyeSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
        rightEyeSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
    }


}
