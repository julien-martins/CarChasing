using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public int Score = 0;
    public TMP_Text scoreText;

    void Update()
    {
        scoreText.text = "Score: " + Score;
    }

    public void ResetScore() => Score = 0;

    public void AddScore(int score) => Score += score;
}
