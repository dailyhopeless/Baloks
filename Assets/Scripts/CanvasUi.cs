using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetMusic();
        SetSoundEffect();
   
        isPaused = false;
        Game.gameover = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Game.gameover)
        {
            GameOver();
        }
        else {
            PanelPause();
        }
    }
    public GameObject PanelBox;
    public GameObject blurBackground;
    public GameObject BackgroundCanvas;
    public static bool isPaused = false;
    private bool fadeinbool = false;
    private readonly float speedfade = 0.1f;
    public static float fade;
    private float zoom = 0.7f;

    public Toggle SettingsMusic;
    public Toggle SettingSoundEffects;

    [Header("Game Over Panel")]
    public GameObject PanelGameOver;
    public GameObject BackgroundGameOver;
    public Text scoreText;
    public Text hightScoreText;
    public Text levelText;
    public Text lineText;
    private float score;
    private float highscore = 0;
    private float level;
    private float line;




    //private float zoom = 1.0f;


    public void GameEndScore() {
        
        if (level < Game.currentLevel)
            level += (1 * Time.unscaledDeltaTime) * 10;
            levelText.text = Mathf.Round(level).ToString();

        if (score < Game.currentScore)
            score += (1 * Time.unscaledDeltaTime) * 10;
            scoreText.text = Mathf.Floor(score).ToString();
        if (line < Game.numLinesCleared)
            line += (1 * Time.unscaledDeltaTime) * 10;
            lineText.text = Mathf.Floor(line).ToString();

        if (Game.startingHighScore > Game.currentScore)
        {
            hightScoreText.text = Game.startingHighScore.ToString();
        }
        else if (Game.startingHighScore < Game.currentScore && Game.tempScore == 0)
        {
            if ((int)score == Game.currentScore)
            {
                if (highscore < Game.currentScore)
                    highscore += (1 * Time.unscaledTime) * 0.01f;
                hightScoreText.text = Mathf.Floor(highscore).ToString();
            }
        }
        else if (Game.startingHighScore < Game.currentScore ) {
           
            if ((int)score == Game.currentScore) {
                if (highscore < (Game.currentScore - Game.tempScore))
                    highscore += (1 * Time.unscaledDeltaTime) * 10;
                //newhightscore = Game.tempScore + highscore;
                //hightScoreText.text = Mathf.Floor(newhightscore).ToString();
            }
            hightScoreText.text = (Game.tempScore + Mathf.Floor(highscore)).ToString();

        }
    }
    public void SettingsMenuMusic(bool value) {
        if (value)
        {
            PlayerPrefs.SetInt("SettingsMusic", 1);
        }
        else {
            
            PlayerPrefs.SetInt("SettingsMusic", 0);
        }
        
    }
    public void SettingsSoundEffect(bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt("SettingsSoundEffect", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SettingsSoundEffect", 0);
        }

    }

    private void SetSoundEffect()
    {
        if (PlayerPrefs.GetInt("SettingsSoundEffect") == 1)
        {
            SettingSoundEffects.isOn = true;
         
        }
        else
        {
            SettingSoundEffects.isOn = false;  
        }
    }
    private void SetMusic() {
        if (PlayerPrefs.GetInt("SettingsMusic") == 1)
        {
            SettingsMusic.isOn = true;
            FindObjectOfType<AudioGame>().GetComponent<AudioSource>().Play();

        }
        else {
            SettingsMusic.isOn = false;
            if (PlayerPrefs.GetInt("SettingsMusic") != 0)
                AudioGame.audioSource.Stop();
        }
    }

    public void CanvasScale(float value, GameObject canvas)
    {
        canvas.GetComponentInChildren<RectTransform>().localScale = new Vector2(value, value);
    }
    public float ZoomIn()
    {
        if (zoom < 1)
        {
            zoom += (Time.unscaledDeltaTime * speedfade) * 30;
            if ((int)zoom == 1)
                zoom = 1;
        }
        return zoom;
    }

    public float ZoomOut()
    {
        if (zoom > 0.7f)
            zoom -= (Time.unscaledDeltaTime * speedfade) * 20;
        return zoom;
    }


    public void CanvasAlpha(float value, GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    public float ZerotoOne()
    {
        if (fade < 1)
            fade += (Time.unscaledDeltaTime * speedfade) * 30;
        return fade;
    }

    public float OnetoZero()
    {
        if (fade > 0)
            fade -= (Time.unscaledDeltaTime * speedfade) * 20;
        return fade;
    }

    public void GameOver() {

        PanelGameOver.SetActive(true);
        GameEndScore();
            CanvasAlpha(ZerotoOne(), PanelGameOver);
            CanvasScale(ZoomIn(), BackgroundGameOver);
            Time.timeScale = 0;
        isPaused = true;
        //AudioGame.audioSource.clip =
        blurBackground.SetActive(true);

    }
    private void PanelPause()
    {
       
        if (fadeinbool)
        {
            
            CanvasAlpha(ZerotoOne(), PanelBox);
            CanvasScale(ZoomIn(), BackgroundCanvas);
            Time.timeScale = 0;
            isPaused = true;
            

            blurBackground.SetActive(true);
          
        }
        else
        {
            CanvasAlpha(OnetoZero(), PanelBox);
            CanvasScale(ZoomOut(), BackgroundCanvas);
            if (OnetoZero() * 10 < 0)
            {
                isPaused = false;
                PanelBox.SetActive(false);
                Time.timeScale = 1;
                blurBackground.SetActive(false);
                
                
                
            }
  
        }
    }
    public void OpenCanvas()
    {
        AudioGame.audioSource.Pause();
        fadeinbool = true;
        PanelBox.SetActive(true);
    }
    public void PlayAgain()
    {
       
        SceneManager.LoadScene("Level");
        FindObjectOfType<Game>().UpdateHighScore();
        PlayerPrefs.SetInt("tempscore", PlayerPrefs.GetInt("highscore"));


    }
    public void BackMenu(string value)
    {
        SceneManager.LoadScene(value);
    }
    public void CloseCanvas()
    {
        SetMusic();
        fadeinbool = false;
        
        FindObjectOfType<Game>().SaveSetting(Game.AddSpeedFall);
        
        //PanelBox.SetActive(false);

    }

  

}

