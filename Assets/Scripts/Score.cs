using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    int score = 0;
    public TextMeshProUGUI scoreLabel;

    public void AddScore()
    {
        score++;
        scoreLabel.SetText("Score: " + score);
    }
}
