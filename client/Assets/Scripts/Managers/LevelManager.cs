using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : Singleton<LevelManager>
{
    //public enum Scenes
    //{
    //    //MainMenuScene,
    //    Level1Scene,
    //    Level2Scene,
    //    Level3Scene,
    //}
    [SerializeField] private GameObject character;

    /*[SerializeField] private GameObject _mainScene;
    [SerializeField] private GameObject _levelSelect;

    [SerializeField] private GameObject _playground;
    [SerializeField] private GameObject _level1;
    [SerializeField] private GameObject _level2;
    [SerializeField] private GameObject _level3;
    
    [SerializeField] private GameObject _level1Hint;
    [SerializeField] private GameObject _level2Hint;
    [SerializeField] private GameObject _level3Hint;*/
    
    [SerializeField] private GameObject[] _levels;

    [SerializeField] private int currentLevel = 0;
    
    public GameSettings gameSettings;

    public BGMPlayer bgmPlayer;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    //public void LoadScene(int toScene)
    //{

    //    var scene = SceneManager.LoadSceneAsync(toScene);
    //    scene.allowSceneActivation = true;
    //}

    public void ReloadLevel()
    {
        LoadLevel(currentLevel);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void LoadLevel(int i)
    {
        currentLevel = i;
        character.SetActive(i is 2 or 4 or 6 or 8);
        for (int j = 0; j < _levels.Length; j++)
        {
            if (i == j && i is 2 or 4 or 6 or 8)
            {
                StartCoroutine(GameManager.Instance.StartGame());

                

            }
            _levels[j].SetActive(i==j);
        }

        if (!bgmPlayer) return;
        switch (i)
        {
            case 0:
                bgmPlayer.PlayBGM(bgmPlayer.mainMenu, 0.5f);
                break;
            case 2:
                bgmPlayer.PlayBGM(bgmPlayer.playgroundMusic, 0.5f);
                break;
            case 4:
                bgmPlayer.PlayBGM(bgmPlayer.playgroundMusic, 0.5f);
                break;
            case 6:
                bgmPlayer.PlayBGM(bgmPlayer.level2Music, 0.5f);
                break;
            case 8:
                bgmPlayer.PlayBGM(bgmPlayer.level3Music, 0.5f);
                break;
            default:
                bgmPlayer.PlayBGM(bgmPlayer.mainMenu, 0.5f);
                break;
        }
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        LoadLevel(currentLevel);
    }

    private void LoadEndPage()
    {
        //UIManager.Instance.ShowEndPage();
        Debug.Log("End of the game");
    }

    private void Start()
    {
        if (bgmPlayer != null)
        {
            bgmPlayer.PlayBGM(bgmPlayer.mainMenu, 0.5f);
        }
        else
        {
            Debug.LogError("BGMPlayer is not assigned in the inspector.");
        }
        LoadNextLevel();
    }

}
