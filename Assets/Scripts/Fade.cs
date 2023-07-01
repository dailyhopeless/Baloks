using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Fade : MonoBehaviour
{

    private float fade;
    private float zoom = 0.9f;

    [Header("Fade Options")]
    public bool useBlurBackground;
    [Tooltip("one panel one trigger")]
    public bool triggerZoomOut;

    [HideInInspector]
    public GameObject blurPanel;
    [HideInInspector]
    public GameObject buttonClose;

    GameObject textobj;

    private bool activebool;
   


    void Start()
    {
       
        textobj = this.gameObject.transform.GetChild(0).gameObject;
        gameObject.GetComponent<CanvasGroup>();
        CheckAktive();
        //FindTrigerChild();
      
    }

    void Update()
    {
        if (activebool)
            Show();
        else
            Hide();
        if (triggerZoomOut) {
            Checktriggerclose();
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
            fade += (Time.unscaledDeltaTime * 0.1f) * 100;
        return fade;
    }
    public float OnetoZero()
    {
        if (fade > 0)
            fade -= (Time.unscaledDeltaTime * 0.1f) * 30;
        return fade;
    }
    public float ZoomIn()
    {
        if (zoom < 1)
        {
            zoom += (Time.unscaledDeltaTime * 0.1f) * 30;
            if ((int)zoom == 1)
                zoom = 1;
        }
        return zoom;
    }
    public float ZoomOut()
    {
        if (zoom > 0.9f)
            zoom -= (Time.unscaledDeltaTime * 0.1f) * 20;
        return zoom;
    }
    public void Panelin(GameObject Panel, GameObject Background, GameObject BlurBackground)
    {
        //Panel.SetActive(true);
        CanvasAlpha(ZerotoOne(), Panel);
        CanvasScale(ZoomIn(), Background);
        if (useBlurBackground)
            BlurBackground.SetActive(true);

    }

    public void Panelout(GameObject Panel, GameObject Background, GameObject Blurbackground)
    {
        CanvasAlpha(OnetoZero(), Panel);
        CanvasScale(ZoomOut(), Background);
        if (OnetoZero() * 10 < 0)
        {
            gameObject.SetActive(false);
            if (useBlurBackground)
                Blurbackground.SetActive(false);
            if (!gameObject.activeSelf) {
                activebool = true;
                if (triggerZoomOut)
                    buttonClose.SetActive(true);
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
    void Checktriggerclose() { 
        if(!buttonClose.activeSelf)
            activebool = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Fade))]
public class OptionsFadeScript : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Fade fade = (Fade)target;
        if (fade.useBlurBackground)
            fade.blurPanel = EditorGUILayout.ObjectField("Use Background Blur",fade.blurPanel, typeof(GameObject),true ) as GameObject;
        if(fade.triggerZoomOut)
            fade.buttonClose = EditorGUILayout.ObjectField("Add Gameobject empty", fade.buttonClose, typeof(GameObject), true) as GameObject;
    }

}

#endif