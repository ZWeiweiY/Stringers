using UnityEngine;
using System.Collections.Generic;
public class ProgressTracker : Singleton<ProgressTracker>
{
    [SerializeField] private float logPerSecond = 0.5f;
    public int roundNumber { get; private set; } // The current round number

    public List<Vector3> positionStackForRound ;
    //{ get; private set; }
    public List<List<Vector3>> faceLandmarkReadOutForRound ;

    [SerializeField] private FaceDataSO faceData;

    private GameObject character;
    private RBMoveController characterMoveController;

    private float logTimer;
    public bool hasLoggedCurrentRound = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        positionStackForRound = new List<Vector3>();
        faceLandmarkReadOutForRound = new List<List<Vector3>>();
        roundNumber = 1;

        if (faceData == null)
        {
            Debug.LogError("FaceDataSO not assigned in ProgressTracker.");
            return;
        }

        character = GameManager.Instance.character;
        if (character == null)
        {
            Debug.LogError("Character not found in GameManager.");
            return;
        }

        characterMoveController = character.GetComponent<RBMoveController>();
        if (characterMoveController == null)
        {
            Debug.LogError("RBMoveController not found on character.");
        }

        logTimer = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        logTimer += Time.deltaTime;
        if (logTimer >= logPerSecond)
        {
            if(characterMoveController.resetting != true)
            {
                hasLoggedCurrentRound = false;
                Vector3 currentPosition = character.transform.position;
                positionStackForRound.Add(currentPosition);
                faceLandmarkReadOutForRound.Add(faceData.landmarkList);
            }
            else
            {
                if(positionStackForRound.Count != 0 && !hasLoggedCurrentRound)
                {
                    Vector3 deathPosition = character.transform.position;
                    positionStackForRound.Add(deathPosition);
                    faceLandmarkReadOutForRound.Add(faceData.landmarkList);
                    CSVLogger.Instance.LogData();
                    hasLoggedCurrentRound = true;
                    positionStackForRound.Clear();
                    faceLandmarkReadOutForRound.Clear();
                }
                
                
            }
            logTimer = 0f;
        }
    }

    public void IncrementRound()
    {
        roundNumber++;
        positionStackForRound.Clear();
        faceLandmarkReadOutForRound.Clear();
        hasLoggedCurrentRound = false;
    }

    public void ResetRound()
    {
        roundNumber = 1;
        Debug.Log("Call hard set round number");
        positionStackForRound.Clear();
        faceLandmarkReadOutForRound.Clear();
        hasLoggedCurrentRound = false;
        CSVLogger.Instance.SetCurrentRoundInitialData();
    }
}
