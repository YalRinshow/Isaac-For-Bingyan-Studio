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
    public AudioSource backgroundMusic;
    public string audioFilePath = Constants.MUSIC_FILE;
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
        PlayMusic(audioFilePath + Constants.MUSIC_GAME, backgroundMusic);
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
            PlayMusic(audioFilePath + Constants.MUSIC_GAMEOVER_WIN, backgroundMusic);
        }
        else
        {
            gameOverText.text = Constants.GAME_OVER_LOSE;
            PlayMusic(audioFilePath + Constants.MUSIC_GAMEOVER_LOSE, backgroundMusic);
        }
    }
    public void PlayBossMusic()
    {
        PlayMusic(audioFilePath + Constants.MUSIC_BOSS, backgroundMusic);
    }
    private void PlayMusic(string audioPath, AudioSource audio, bool loop = true)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
        if (audioClip != null)
        {
            audio.clip = audioClip;
            audio.Play();
            audio.loop = loop;
        }
        else
        {
            Debug.Log(Constants.AUDIO_LOAD_FAILED + audioPath);
        }
    }
}
