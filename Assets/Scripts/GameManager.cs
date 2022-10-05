using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameManager(){}
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; set; }
    public bool GameStarting { get; set; }

    public GameObject GameOverScreen;
    public Text GameOverScoreText;
    public ScoreManager ScoreManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    void Start()
    {
        GameStarting = false;
        Time.timeScale = 0;
    }

    void Update()
    {
        if (!GameStarting)
        {
            if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") > 0)
            {
                GameStarting = true;
                Time.timeScale = 1;
            }
        }
        
        if (IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space)) Replay();
            else if (Input.GetKeyDown(KeyCode.Escape)) ToMenu();
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        GameOverScreen.SetActive(true);
        GameOverScoreText.text = "" + ScoreManager.Score;
        Time.timeScale = 0;
    }

    public void Replay()
    {
        GameOverScreen.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

}
