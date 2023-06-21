using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    private float fade;
    private float zoom = 0.7f;

    private CanvasGroup Panel;

    public GameObject blurPanel;

    GameObject textobj;

    private bool activebool;
   


    void Awake()
    {
        textobj = this.gameObject.transform.GetChild(0).gameObject;
        Panel = GetComponent<CanvasGroup>();
        //textobj = this.gameObject.
        Debug.Log(gameObject.name);
        CheckAktive();
        //activebool = false;
    }


    void Update()
    {
        if (activebool)
            Show();
        else
            Hide();

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
    public void Panelin(GameObject Panel, GameObject Background, GameObject BlurBackground)
    {
        //Panel.SetActive(true);
        CanvasAlpha(ZerotoOne(), Panel);
        CanvasScale(ZoomIn(), Background);
        BlurBackground.SetActive(true);
    }

    public void Panelout(GameObject Panel, GameObject Background, GameObject Blurbackground)
    {
        CanvasAlpha(OnetoZero(), Panel);
        CanvasScale(ZoomOut(), Background);
        if (OnetoZero() * 10 < 0)
        {
            gameObject.SetActive(false);
            Blurbackground.SetActive(false);
            if (!gameObject.activeSelf) {
                activebool = true;
            }
        }
    }
    void CheckAktive() {
        if (gameObject.activeSelf)
        {
            activebool = true;
            Show();
        }
                                                                                                                                                                                                                                                                                                                                                    
    }
    void Show() {
        Panelin(gameObject, textobj, blurPanel);
    }
    void Hide() {
        Panelout(gameObject, textobj, blurPanel);
    }

    public void NotActive() {
        //gameObject.SetActive(false);
        //blurPanel.SetActive(false);
        //Hide
        activebool = false;

    }
}
