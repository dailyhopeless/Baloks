using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetMusic();
        SetSoundEffect();
    }

    // Update is called once per frame
    void Update()
    {
        PanelPause();
       



    }
    public GameObject PanelBox;
    public GameObject blurBackground;
    public GameObject BackgroundCanvas;
    public static bool isPaused = false;
    private bool fadeinbool = false;
    private bool fadeoutbool = false;
    private readonly float speedfade = 0.1f;
    private float fade;
    private float zoom = 0.7f;

    public Toggle SettingsMusic;
    public Toggle SettingSoundEffects;




    //private float zoom = 1.0f;

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
            AudioGame.audioSource.Play();
        }
        else {
            SettingsMusic.isOn = false;
            AudioGame.audioSource.Stop();
        }
    }

    public void CanvasScale(float value, GameObject canvas)
    {
        canvas.GetComponentInChildren<RectTransform>().localScale = new Vector2(value, value);
    }
    private float ZoomIn()
    {
        if (zoom < 1)
        {
            zoom += (Time.unscaledDeltaTime * speedfade) * 30;
            if ((int)zoom == 1)
                zoom = 1;
        }
        return zoom;
    }

    private float ZoomOut()
    {
        if (zoom > 0.7f)
            zoom -= (Time.unscaledDeltaTime * speedfade) * 20;
        return zoom;
    }


    void CanvasAlpha(float value, GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    private float ZerotoOne()
    {
        if (fade < 1)
            fade += (Time.unscaledDeltaTime * speedfade) * 30;
        return fade;
    }

    private float OnetoZero()
    {
        if (fade > 0)
            fade -= (Time.unscaledDeltaTime * speedfade) * 20;
        return fade;
    }
    private void PanelPause()
    {
        if (fadeinbool)
        {
            CanvasAlpha(ZerotoOne(), PanelBox);
            CanvasScale(ZoomIn(), BackgroundCanvas);
            Time.timeScale = 0;
            AudioGame.audioSource.Pause();

            blurBackground.SetActive(true);
            fadeoutbool = true;
        }
        else if (!fadeinbool && fadeoutbool)
        {
            CanvasAlpha(OnetoZero(), PanelBox);
            CanvasScale(ZoomOut(), BackgroundCanvas);
            if (OnetoZero() * 10 < 0)
            {
                PanelBox.SetActive(false);
                Time.timeScale = 1;
                blurBackground.SetActive(false);
     
                    SetMusic();
                
                
            }
            if (!PanelBox.activeSelf)
            {
                fadeoutbool = false;
            }
        }
    }
    public void OpenCanvas()
    {
        fadeinbool = true;
        PanelBox.SetActive(true);
    }
    public void CloseCanvas()
    {
        fadeinbool = false;
        
        FindObjectOfType<Game>().SaveSetting(Game.AddSpeedFall);
        
        //PanelBox.SetActive(false);

    }
}

