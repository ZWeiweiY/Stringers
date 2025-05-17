using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public enum Pattern
{
    Down = -1,
    None = 0,
    Up = 1
}
public class PatternGeneration : MonoBehaviour
{
    //should be grabbed from the file
    public Pattern[] pattern;
    //===============================

    public GameObject start;
    public GameObject downStroke;
    public GameObject upStroke;
    public GameObject currentCamera;
    private Vector3 initialCameraPosition;
    private bool generating;
    private float generationTimer = 0f;

    public GameObject parent;
    private SecondStage stage;
    private void Awake()
    {
        stage = SecondStage.instance;
        //TODO: grab pattern from file
        initialCameraPosition = currentCamera.transform.position;
        StartCoroutine(GeneratePattern());
    }

    public void ReadFile()
    {
        
    }

    IEnumerator GeneratePattern()
    {
        generating = true;
        Vector3 position = start.transform.position;
        for (int i = 0; i < pattern.Length; i++)
        {
            
            if (pattern[i] == Pattern.Down)
            {
                Instantiate(downStroke, position + new Vector3(stage.horizontalSpacing * i, 0, 0), Quaternion.identity).transform.parent = parent.transform;
            } else if (pattern[i] == Pattern.Up)
            {
                Instantiate(upStroke, position + new Vector3(stage.horizontalSpacing * i, stage.verticalSpacing, 0), Quaternion.identity).transform.parent = parent.transform;
            }
            yield return new WaitForSeconds(60/SecondStage.instance.bpm);
        }
        generating = false;
    }

    void Update()
    {
        if (generating)
        {
            generationTimer += Time.deltaTime;
            Vector3 position = Vector3.Lerp(initialCameraPosition,
                initialCameraPosition + new Vector3(stage.horizontalSpacing * pattern.Length, 0, 0), generationTimer / (60/SecondStage.instance.bpm*pattern.Length));
            currentCamera.transform.position = position;
        }
        else if (currentCamera.transform.position != initialCameraPosition)
        {
            ResetCamera();
        }
    }

    void ResetCamera()
    {
        currentCamera.transform.position = initialCameraPosition;
    }
}
