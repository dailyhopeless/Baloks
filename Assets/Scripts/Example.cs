using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public GameObject PanelBox;
    // Start is called before the first frame update
    void Start()
    {
        
        //CanvasAlpha(FadeIn(), PanelBox);
    }

    // Update is called once per frame
    void Update()
    {
        PanelPause();

        ZoomOut();
        //StartCoroutine(Fade());


    }
    private void Awake()
    {

    }
    private bool fadeinbool = false;
    private bool fadeoutbool = false;
    private readonly float speedfade = 0.1f;
    private float fade;
    private float zoom = 0.7f;
    //private float zoom = 1.0f;
    public GameObject BackgroundCanvas;


    public void CanvasScale(float value, GameObject canvas) {
        canvas.GetComponentInChildren<RectTransform>().localScale = new Vector2(value, value);
    }
    private float ZoomIn() {
        if (zoom < 1)
        {
            zoom += (Time.deltaTime * speedfade) * 30;
            if ((int)zoom == 1)
                zoom = 1;
        }
            return zoom;
    }

    private float ZoomOut() {
        if (zoom > 0.7f)
            zoom -= (Time.deltaTime * speedfade) *20;
            return zoom;
    }


    void CanvasAlpha(float value, GameObject canvas)
    {
            canvas.GetComponent<CanvasGroup>().alpha = value;
    }
    private float ZerotoOne() {
        if (fade < 1)
            fade += (Time.deltaTime * speedfade) * 30;
            return fade;
    }

    private float OnetoZero() {
        if (fade > 0)
            fade -= (Time.deltaTime * speedfade) * 20;
            return fade;
    }
    private void PanelPause()
    {
        if (fadeinbool)
        {
            CanvasAlpha(ZerotoOne(), PanelBox);
            CanvasScale(ZoomIn(), BackgroundCanvas);
            fadeoutbool = true;
        } else if (!fadeinbool && fadeoutbool)
        {
            CanvasAlpha(OnetoZero(), PanelBox);
            CanvasScale(ZoomOut(), BackgroundCanvas);
            if (OnetoZero() * 10 < 0)
                PanelBox.SetActive(false);
            if (!PanelBox.activeSelf) {
                fadeoutbool = false;
            }
        }
    }
    public void OpenCanvas() {
        fadeinbool = true;
        PanelBox.SetActive(true);
    }
    public void CloseCanvas() {
        fadeinbool = false;
        //PanelBox.SetActive(false);

    }

   

}
