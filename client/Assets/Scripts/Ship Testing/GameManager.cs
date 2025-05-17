using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject character;
    public Vector3 startPosition;
    public Quaternion startRotation;
    [SerializeField] private float respawnWaitTime = 0.5f;
    public CountDown countDown;
    public GameSettings gameSettings;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        if (character != null)
        {
            startPosition = character.transform.position;
            startRotation = character.transform.rotation;
        }
        gameSettings.ResetIndex();
        countDown.gameObject.SetActive(false);
    }

    public IEnumerator RestartGame()
    {
        character.GetComponent<RBMoveController>().resetting = true;
        character.GetComponentInChildren<RBMoveController>()?.StopMovement();


        
        yield return StartCoroutine(character.GetComponentInChildren<CharacterDissolve>()?.DissolveCo());
        
        character.transform.position = startPosition;
        character.transform.rotation = startRotation;

        ProgressTracker.Instance.IncrementRound();

        yield return new WaitForSeconds(respawnWaitTime);

        /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);*/
        countDown.gameObject.SetActive(true);
        StartCoroutine(countDown.CountDownTwoSec());
        SoundManager.Instance.PlayRebornSound();
        yield return StartCoroutine(character.GetComponent<CharacterDissolve>()?.ReEmerge());
        character.GetComponent<RBMoveController>().resetting = false;
        character.GetComponent<PlanetShooter>()?.ResetFeatures();
        character.GetComponentInChildren<PlanetShooter>()?.ClearPlanets();
        
    }

    public IEnumerator StartGame()
    {
        character.GetComponent<RBMoveController>().resetting = true;
        character.GetComponentInChildren<RBMoveController>()?.StopMovement();
        character.transform.position = startPosition;
        character.transform.rotation = startRotation;
        countDown.gameObject.SetActive(true);
        SoundManager.Instance.PlayCountdownSound();
        StartCoroutine(countDown.CountDownTwoSec());
        yield return StartCoroutine(character.GetComponent<CharacterDissolve>()?.ReEmerge());
        character.GetComponent<RBMoveController>().resetting = false;
        character.GetComponent<PlanetShooter>()?.ResetFeatures();
        ProgressTracker.Instance.ResetRound();
    }

    public IEnumerator NextLevel()
    {
        character.GetComponent<RBMoveController>().resetting = true;
        character.GetComponentInChildren<RBMoveController>()?.StopMovement();
        yield break;
        /*
        yield return StartCoroutine(character.GetComponentInChildren<CharacterDissolve>()?.DissolveCo());
        
        character.transform.position = startPosition;
        character.transform.rotation = startRotation;
        LevelManager.Instance.LoadNextLevel();*/
    }

    public void StopGame()
    {
        character.GetComponent<RBMoveController>().resetting = true;
        character.GetComponentInChildren<RBMoveController>()?.StopMovement();
        LevelManager.Instance.bgmPlayer.PlayBGM(LevelManager.Instance.bgmPlayer.audioSource.clip, 0.2f);
    }
    
}
