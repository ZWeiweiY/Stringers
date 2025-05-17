using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLogger : Singleton<CSVLogger>
{
    [System.Serializable]
    public struct CSVLoggerData
    {
        public string playerId;
        public System.DateTime timestamp;

        // Game Data
        public int level;
     
        public int round;
        
        //public int difficulty; // speed can be used instead
        public float speed;
        public float rotationSpeed;

        // Round Data
        public int planetEatenThisRound;
        public int meteorDestroyedThisRound;
        public int dashSuccessThisRound;
        public int shrinkSuccessThisRound;

        public System.DateTime startTimeInRound;
        public double timeSpentInRound;

        // Face Data
        public Vector2 facePositionInFrame;
        public float Yaw;
        public float Pitch;
        public float Roll;
        public string action;
        public float mouthOpenActionScore;
        public float mouthPuckerActionScore;
        public float leftEyeBlinkActionScore;
        public float rightEyeBlinkActionScore;
        public float mouthRollActionScore;

        // Path Data
        public List<string> roundPositionsStringList;
        public List<string> landmarkListStringList;
    }

    public CSVLoggerData data;

    [SerializeField] private GameObject player;
    [SerializeField] private FaceDataSO faceData;

    private string playerId;
    private string directoryPath;
    private string dataFilePath;
    private string pathRoutesPath;
    private SplineOutput splineOutput;
    private const string pathFilePrefix = "path_";

    private bool isCurrentlyLogging = false;

    protected override void Awake()
    {
        base.Awake();
        playerId = System.Guid.NewGuid().ToString();
        // Initialize the directory path
        directoryPath = Path.Combine(Application.persistentDataPath, playerId);
        Directory.CreateDirectory(directoryPath);

        // Initialize the file paths
        dataFilePath = Path.Combine(directoryPath, "data.csv");
        // Check if the file exists, if not create it and write the header
        if (!File.Exists(dataFilePath))
        {
            File.WriteAllText(dataFilePath, ToDataCsvHeader() + "\n");
        }
        
        pathRoutesPath = Path.Combine(directoryPath, "path_routes.csv");
        if (!File.Exists(pathRoutesPath))
        {
            File.WriteAllText(pathRoutesPath, ToPathRouteCsvHeader() + "\n");
        }

    }

    private string ToDataCsvHeader()
    {
        return string.Join(",", new string[]
        {
            "playerId",
            "timestamp",
            "level",
            "round",
            "speed",
            "rotationSpeed",
            "planetEatenThisRound",
            "meteorDestroyedThisRound",
            "dashSuccessThisRound",
            "shrinkSuccessThisRound",
            "timeSpentInRound",
            "facePositionInFrameX",
            "facePositionInFrameY",
            "Yaw",
            "Pitch",
            "Roll",
            "action",
            "mouthOpenActionScore",
            "mouthPuckerActionScore",
            "leftEyeBlinkActionScore",
            "rightEyeBlinkActionScore",
            "mouthRollActionScore",
        });
    }

    private string ToPathRouteCsvHeader()
    {
        return string.Join(",", new string[]
        {
            "level",
            "radius",
            "knotPositionX",
            "knotPositionY",
            "knotPositionZ",
            "knotRotationX",
            "knotRotationY",
            "knotRotationZ",
            "knotTangentInX",
            "knotTangentInY",
            "knotTangentInZ",
            "knotTangentOutX",
            "knotTangentOutY",
            "knotTangentOutZ",
        });
    }
    private string ToPathCsvHeader()
    {
        string landmarkCoordinates = "";

        for (int i = 0; i < 478; i++)
        {
            landmarkCoordinates += $"landmark_{i}X,landmark_{i}Y,landmark_{i}Z,";
        }

        return string.Join(",", new string[]
        {
            "playerId",
            "level",
            "speed",
            "rotationSpeed",
            "positionX",
            "positionY",
            "positionZ",
            landmarkCoordinates
        });
    }

    public void LogData()
    {
        if (isCurrentlyLogging) return;
        isCurrentlyLogging = true;

        SetEndData();
        
        string dataLine = string.Join(",", new string[]
        {
            data.playerId,
            data.timestamp.ToString("o"), // ISO 8601 format
            data.level.ToString(),
            data.round.ToString(),
            data.speed.ToString(),
            data.rotationSpeed.ToString(),
            data.planetEatenThisRound.ToString(),
            data.meteorDestroyedThisRound.ToString(),
            data.dashSuccessThisRound.ToString(),
            data.shrinkSuccessThisRound.ToString(),
            data.timeSpentInRound.ToString(),
            (data.facePositionInFrame.x * WebCamController.Instance._webCamTexture.width).ToString(),
            (data.facePositionInFrame.y * WebCamController.Instance._webCamTexture.height).ToString(),
            data.Yaw.ToString(),
            data.Pitch.ToString(),
            data.Roll.ToString(),
            data.action,
            data.mouthOpenActionScore.ToString(),
            data.mouthPuckerActionScore.ToString(),
            data.leftEyeBlinkActionScore.ToString(),
            data.rightEyeBlinkActionScore.ToString(),
            data.mouthRollActionScore.ToString(),

        });

        try
        {
            File.AppendAllText(dataFilePath, dataLine + "\n");
            //Debug.Log("Data [" + dataLine + "] logged to CSV file: " + dataFilePath);
            LogPathData(data.round, data.level);
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write to CSV file: " + ex.Message);
        }

        NewRoundReset();
        isCurrentlyLogging = false;
    }

    private void LogPathData(int roundNumber, int levelNumber)
    {
        string pathFilePath = Path.Combine(directoryPath, pathFilePrefix + levelNumber.ToString() + "-" + roundNumber.ToString() + ".csv");
        
        if (!File.Exists(pathFilePath))
        {
            File.WriteAllText(pathFilePath, ToPathCsvHeader() + "\n");
        }

        data.roundPositionsStringList = GetPositionDataList(ProgressTracker.Instance.positionStackForRound);
        data.landmarkListStringList = GetFaceLandmarksDataList(ProgressTracker.Instance.faceLandmarkReadOutForRound);

        for (int i = 0; i < data.roundPositionsStringList.Count; i++)
        {
            string pathLine = string.Join(",", new string[]
            {
                data.playerId,
                data.level.ToString(),
                data.speed.ToString(),
                data.rotationSpeed.ToString(),
                data.roundPositionsStringList[i],
                data.landmarkListStringList[i]
            });
            try
            {
                File.AppendAllText(pathFilePath, pathLine + "\n");
                //Debug.Log("Path [" + pathLine + "] logged to CSV file: " + pathFilePath);
            }
            catch (IOException ex)
            {
                Debug.LogError("Failed to write to CSV file: " + ex.Message);
            }
        }
    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        data.playerId = playerId;
        splineOutput = GetComponent<SplineOutput>();
        if (splineOutput != null)
            LogPathRoutes();
    }

    private void LogPathRoutes()
    {
        
        Debug.Log(splineOutput.Getlevel3Line().Count);
        LogLevel1SplineRoute();
        LogLevel2SplineRoute();
        LogLevel3SplineRoute();

        void LogLevel1SplineRoute()
        {
            foreach (var ele in splineOutput.Getlevel1Line())
            {
                try
                {
                    File.AppendAllText(pathRoutesPath, ele + "\n");
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to write to RoutesCSV: " + e.Message);
                    throw;
                }
            
            }
        }
        void LogLevel2SplineRoute()
        {
            foreach (var ele in splineOutput.Getlevel2Line())
            {
                try
                {
                    File.AppendAllText(pathRoutesPath, ele + "\n");
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to write to RoutesCSV: " + e.Message);
                    throw;
                }
            
            }
        }
        void LogLevel3SplineRoute()
        {
            foreach (var ele in splineOutput.Getlevel3Line())
            {
                try
                {
                    File.AppendAllText(pathRoutesPath, ele + "\n");
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to write to RoutesCSV: " + e.Message);
                    throw;
                }
            
            }
        }
    }

    public void SetCurrentRoundInitialData()
    {
        data.round = ProgressTracker.Instance.roundNumber;
        data.level = (LevelManager.Instance.GetCurrentLevel() - 2)/2;
        data.speed = player.GetComponent<RBMoveController>().maxSpeed;
        data.rotationSpeed = player.GetComponent<RBMoveController>().rotationSpeed;
        data.planetEatenThisRound = 0;
        data.meteorDestroyedThisRound = 0;
        data.dashSuccessThisRound = 0;
        data.shrinkSuccessThisRound = 0;
        data.startTimeInRound = System.DateTime.UtcNow;
        data.timeSpentInRound = 0;
    }

    #region In-Game Calls
    public void AddPlanetCount()
    {
        data.planetEatenThisRound++;
    }

    public void AddMeteorCount()
    {
        data.meteorDestroyedThisRound++;
    }

    public void AddDashCount()
    {
        data.dashSuccessThisRound++;
    }

    public void AddShrinkCount()
    {
        data.shrinkSuccessThisRound++;
    }
    #endregion

    public double GetTimeDifferenceInSeconds(System.DateTime previousTimestamp)
    {
        return (data.timestamp - previousTimestamp).TotalSeconds;
    }
    public List<string> GetPositionDataList(List<Vector3> positionStack)
    {
        List<string> positionDataList = new List<string>();
        if (positionStack.Count <= 0)
        {
            return positionDataList;
        }
        foreach (Vector3 pos in positionStack)
        {
            string positionData = $"{pos.x}, {pos.y}, {pos.z}";
            positionDataList.Add(positionData);
        }
        return positionDataList;
    }

    public List<string> GetFaceLandmarksDataList(List<List<Vector3>> faceLandmarkReadOut)
    {
        List<string> landmarkPositionStringList = new List<string>();
        if (faceLandmarkReadOut.Count <= 0)
        {
            return landmarkPositionStringList;
        }

        //Debug.Assert(faceLandmarkReadOut.Count == positionStack.Count);

        foreach (List<Vector3> faceLandmarkList in faceLandmarkReadOut)
        {
            //Debug.Log(faceLandmarkList.Count);
            string landmarkData = string.Join(",", faceLandmarkList.ConvertAll(pos => $"{pos.x}, {pos.y}, {pos.z}"));
            landmarkPositionStringList.Add(landmarkData);
        }
        return landmarkPositionStringList;

    }

    /*
    public string LogPositionData(List<Vector3> positionStack)
    {
        if (positionStack.Count <= 0)
        {
            return string.Empty;
        }
        string positionData = string.Join(",", positionStack.ConvertAll(pos => $"{pos.x}, {pos.y}, {pos.z}"));
        return positionData;
    }

    public string LogFaceLandmarksData(List<List<Vector3>> faceLandmarkReadOut)
    {
        if (faceLandmarkReadOut.Count <= 0)
        {
            return string.Empty;
        }

        //Debug.Assert(faceLandmarkReadOut.Count == positionStack.Count);
        
        List<string> landmarkPositionStringList = new List<string>();

        foreach (var faceLandmarkList in faceLandmarkReadOut)
        {
            //Debug.Log(faceLandmarkList.Count);
            string landmarkData = string.Join(",", faceLandmarkList.ConvertAll(pos => $"{pos.x}, {pos.y}, {pos.z}"));
            landmarkPositionStringList.Add(landmarkData);
        }
        string landmarkDataString = string.Join(";", landmarkPositionStringList);
        return landmarkDataString;

    }
    
    */
    
    public void SetEndData()
    {
        data.level = (LevelManager.Instance.GetCurrentLevel() - 2) / 2;
        data.round = ProgressTracker.Instance.roundNumber;
        data.timestamp = System.DateTime.UtcNow;
        data.timeSpentInRound = GetTimeDifferenceInSeconds(data.startTimeInRound);
        SetFaceData();
    }

    private void SetFaceData()
    {
        data.facePositionInFrame = new Vector2(faceData.landmarkList[0].x, faceData.landmarkList[0].y);
        data.Yaw = faceData.Yaw;
        data.Pitch = faceData.Pitch;
        data.Roll = faceData.Roll;
        data.action = faceData.action;
        data.mouthOpenActionScore = faceData.mouthOpenActionScore;
        data.mouthPuckerActionScore = faceData.mouthPuckerActionScore;
        data.leftEyeBlinkActionScore = faceData.leftEyeBlinkActionScore;
        data.rightEyeBlinkActionScore = faceData.rightEyeBlinkActionScore;
        data.mouthRollActionScore = faceData.mouthRollActionScore;
    }

    private void NewRoundReset()
    {
        data.round = ProgressTracker.Instance.roundNumber;
        data.level = (LevelManager.Instance.GetCurrentLevel() - 2) / 2;
        data.speed = player.GetComponent<RBMoveController>().maxSpeed;
        data.rotationSpeed = player.GetComponent<RBMoveController>().rotationSpeed;
        data.planetEatenThisRound = 0;
        data.meteorDestroyedThisRound = 0;
        data.dashSuccessThisRound = 0;
        data.shrinkSuccessThisRound = 0;
        data.startTimeInRound = System.DateTime.UtcNow;
        data.timeSpentInRound = 0;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
