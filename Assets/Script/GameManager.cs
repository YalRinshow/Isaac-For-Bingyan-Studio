using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameScene;
    public GameObject UIPanel;
    public TextMeshProUGUI gameOverText;
    public static GameManager Instance { get; private set; }
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
    }
    void Start()
    {
        gameOverPanel.SetActive(false);
        Prefabs.LoadPrefabs();
        LoadMapAndEnemy();
    }
    void LoadMapAndEnemy()
    {
        Map.Instance.Initialize();
    }
    public void GameOver(bool win)
    {
        gameScene.SetActive(false);
        UIPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        if (win)
        {
            gameOverText.text = Constants.GAME_OVER_WIN;
        }
        else
        {
            gameOverText.text = Constants.GAME_OVER_LOSE;
        }
    }
}
