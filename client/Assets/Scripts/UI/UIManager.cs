using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject endPanel;
    public GameObject pausePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (LevelManager.Instance.GetCurrentLevel() is not (2 or 4 or 6 or 8)) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.StopGame();
            pausePanel.SetActive(true);
        }
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }
}
