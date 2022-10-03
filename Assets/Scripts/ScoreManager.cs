using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Player Player;
    [HideInInspector] public int Score = 0;
    public TMP_Text scoreText;
    public TMP_Text lifeText;

    void Update()
    {
        scoreText.text = "Score: " + Score;
        lifeText.text = "Vie: " + Player.getCurrentLife() + "/" + Player.getMaxLife();
    }

    public void ResetScore() => Score = 0;

    public void AddScore(int score) => Score += score;
}
