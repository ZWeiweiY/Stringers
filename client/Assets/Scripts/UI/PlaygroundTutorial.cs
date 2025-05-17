using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaygroundTutorial : MonoBehaviour
{
    public FaceDataSO faceData;
     [Header("UI References")]
    public RawImage backgroundImage;
    public RawImage instructionImage;
    public Texture2D completeTextImage;
    public GameObject nextArrow;
    public RawImage humanFaceImage;
    public GameObject humanFaceButton;

    [Header("Tutorial Settings")]
    public float checkInterval = 0.2f;

    private int currentStep = 0;
    private float checkTimer = 0f;
    private List<TutorialStep> tutorialSteps;

    [Header("Instruction Images")] 
    public Texture2D[] backgroundTextures;
    public Texture2D[] instructionTextures;
    public Texture2D[] humanFaceTextures;

    private void Awake()
    {
        gameObject.SetActive(false);
        nextArrow.SetActive(false);
        humanFaceButton.SetActive(false);
        // Ensure you have 5 instruction images in the inspector
        tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep(backgroundTextures[0], instructionTextures[0], () => Mathf.Abs(faceData.Roll) > 0.2f),
            new TutorialStep(backgroundTextures[1], instructionTextures[1], humanFaceTextures[0], () => faceData.action.Equals("Eat")),
            new TutorialStep(backgroundTextures[2], instructionTextures[2], humanFaceTextures[1], () => faceData.action.Equals("Shoot")),
            new TutorialStep(backgroundTextures[3], instructionTextures[3], humanFaceTextures[2], () => faceData.action.Equals("Dash")),
            new TutorialStep(backgroundTextures[4], instructionTextures[4], humanFaceTextures[3], () => faceData.action.Equals("Shrink"))
        };

        ShowCurrentInstruction();
    }

    private void OnEnable()
    {
        ShowCurrentInstruction();
    }

    private void Update()
    {
        if (currentStep >= tutorialSteps.Count) return;

        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            if (tutorialSteps[currentStep].checkCompleted())
            {
                instructionImage.texture = completeTextImage;
                nextArrow.SetActive(true);
            }
        }
    }

    void ShowCurrentInstruction()
    {
        if (currentStep < tutorialSteps.Count)
        {
            instructionImage.gameObject.SetActive(true);
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.texture = tutorialSteps[currentStep].backgroundImage;
            instructionImage.texture = tutorialSteps[currentStep].instructionTexture;
            if (currentStep != 0)
            {
                humanFaceImage.texture = tutorialSteps[currentStep].faceTexture;
                humanFaceButton.SetActive(true);
            }
        }
        else
        {
            instructionImage.gameObject.SetActive(false);
            backgroundImage.gameObject.SetActive(false);
            humanFaceImage.gameObject.SetActive(false);
            currentStep = 0;
            LevelManager.Instance.LoadLevel(2);
        }
    }

    public void AdvanceStep()
    {
        currentStep++;
        nextArrow.SetActive(false);
        ShowCurrentInstruction();
    }

    private class TutorialStep
    {
        public Texture2D backgroundImage;
        public Texture2D instructionTexture;
        public Texture2D faceTexture;
        public Func<bool> checkCompleted;

        public TutorialStep(Texture2D backgroundImage, Texture2D instructionTexture, Func<bool> checkCompleted)
        {
            this.backgroundImage = backgroundImage;
            this.instructionTexture = instructionTexture;
            this.checkCompleted = checkCompleted;
        }
        public TutorialStep(Texture2D backgroundImage, Texture2D instructionTexture, Texture2D faceTexture, Func<bool> checkCompleted)
        {
            this.backgroundImage = backgroundImage;
            this.instructionTexture = instructionTexture;
            this.faceTexture = faceTexture;
            this.checkCompleted = checkCompleted;
        }
    }
}