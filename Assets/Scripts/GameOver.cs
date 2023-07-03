using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Game Over Panel")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hightScoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI lineText;
    private float score;
    private float highscore = 0;
    private float level;
    private float line;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameEndScore();
    }
    public void GameEndScore()
    {

        if (level < Game.currentLevel)
            level += (1 * Time.unscaledDeltaTime) * 10;
        levelText.text = Mathf.Round(level).ToString();
        scoreText.text = Game.currentScore.ToString();

        if (line < Game.numLinesCleared)
            line += (1 * Time.unscaledDeltaTime) * 10;
        lineText.text = Mathf.Floor(line).ToString();

        if (Game.startingHighScore > Game.currentScore)
        {
            hightScoreText.text = Game.startingHighScore.ToString();
        }
        else if (Game.startingHighScore < Game.currentScore && Game.tempScore == 0)
        {
                if (highscore < Game.currentScore)
                    highscore += (1 * Time.unscaledDeltaTime) * 10f;
                hightScoreText.text = Mathf.Floor(highscore).ToString();
            
        }
        else if (Game.startingHighScore < Game.currentScore)
        {
                if (highscore < (Game.currentScore - Game.tempScore))
                    highscore += (1 * Time.unscaledDeltaTime) * 10;
            hightScoreText.text = (Game.tempScore + Mathf.Floor(highscore)).ToString();

        }
    }
}
