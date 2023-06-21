using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    

    [Header ("Panel Settings")]
    public GameObject PanelSettingsObject;
    public GameObject backgroundSettings;
    public static bool boolsettings;
    bool boolpanelsettings;

    [Header("Panel Credits")]
    public GameObject PanelCreditsObject;
    public GameObject backgroundCredits;
    public static bool boolcredits;
    bool boolpanelcredits;

    private float fade;
    private float zoom;
    

    public CanvasUi canvasui;

    public GameObject blurBg;
    void Update()
    {
      if(boolpanelsettings)
        PanelSettings();
      if (boolpanelcredits)
        PanelCredits();
        
        
        //FindObjectOfType<CanvasUi>().PanelCredit();

    }

    public void SettingsButton(bool value) {
        if (value)
        {
            boolpanelcredits = !value;
            boolpanelsettings = value;
            boolsettings = value;
        }
        else {
     
            boolsettings = value;
        }
        
    }

    public void CreditsButton(bool value) {
        if (value)
        {
            boolpanelsettings = !value;
            boolpanelcredits = value;
            boolcredits = value;
        }
        else {
      
            boolcredits = value;

        }
        
    }
    void CanvasAlpha(float value, GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    public void CanvasScale(float value, GameObject canvas)
    {
        canvas.GetComponentInChildren<RectTransform>().localScale = new Vector2(value, value);
    }
    public float ZerotoOne()
    {
        if (fade < 1)
            fade += (Time.deltaTime * 0.1f) * 30;
        return fade;
    }
    public float OnetoZero()
    {
        if (fade > 0)
            fade -= (Time.deltaTime * 0.1f) * 20;
        return fade;
    }
    public float ZoomIn()
    {
        if (zoom < 1)
        {
            zoom += (Time.deltaTime * 0.1f) * 30;
            if ((int)zoom == 1)
                zoom = 1;
        }
        return zoom;
    }
    public float ZoomOut()
    {
        if (zoom > 0.7f)
            zoom -= (Time.deltaTime * 0.1f) * 20;
        return zoom;
    }
    public void Panelin(GameObject Panel , GameObject Background, GameObject BlurBackground) {
      Panel.SetActive(true);
      CanvasAlpha(ZerotoOne(), Panel);
      CanvasScale(ZoomIn(), Background);
      BlurBackground.SetActive(true);  
    }

    public void Panelout(GameObject Panel, GameObject Background, GameObject Blurbackground) {
        CanvasAlpha(OnetoZero(), Panel);
        CanvasScale(ZoomOut(), Background);
        if (OnetoZero() * 10 < 0) {
            Panel.SetActive(false);
            Blurbackground.SetActive(false);
        }
    }

    public void PanelSettings() {
        if (boolsettings)
        {
            
            Panelin(PanelSettingsObject, backgroundSettings, blurBg);
        }
        else {
            Panelout(PanelSettingsObject, backgroundSettings, blurBg);
        }    
    }
    public void PanelCredits()
    {
        if (boolcredits)
        {
           
            Panelin(PanelCreditsObject, backgroundCredits, blurBg);
        }
        else
        {
            Panelout(PanelCreditsObject, backgroundCredits, blurBg);
        }
    }
    public void QuitButton() {
        Application.Quit();
    }




}
