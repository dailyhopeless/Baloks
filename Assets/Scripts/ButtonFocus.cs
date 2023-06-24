using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonFocus : MonoBehaviour
{

    public Sprite FocusSpriteImage;
    public Sprite defaultSprite;
    public GameObject FocusButton; 
    public GameObject ColorButton;
    public GameObject TextButton;


    [Header("Setting Button")]
    public bool smallButton;
    public bool ForSceneManager;
    public bool ForGameObject;
    public bool ForQuitButton;
    public bool resetButton;

    /// <summary>
    /// ButtonFocus dont have back button if want back button call script Fade
    /// </summary>

    [HideInInspector]
    public int NumberScene;
    [HideInInspector]
    public GameObject ActiveGameObject;


    void Update()
    {
      
    }
    /// <summary>
    /// Focus for button push add event trigger put in pointer down
    /// </summary>
    public void Focus (bool value)
    {
        if (value)
            FocusButton.GetComponent<Image>().sprite = FocusSpriteImage;
        StartCoroutine(ButtonAnimate(value));

    }
    /// <summary>
    /// Focusup for effect button add to event triger pointer up
    /// </summary>
    public void FocusUp(bool value) {
        StartCoroutine(ButtonAnimateUp());
    }
    IEnumerator ButtonAnimate(bool value) {
        
        FocusButton.GetComponent<Image>().sprite = FocusSpriteImage;
        if (smallButton)
        {
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0, -0.9f);
            TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0 , 1.5f);
        }
        else {
            ColorButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(162, 37);
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.13f, 2);
            TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.19f, 0);
        }
        yield return new WaitForSeconds(0.2f);
        if (ForSceneManager) 
            SceneManager.LoadScene(NumberScene);
        if (ForGameObject) 
            ActiveGameObject.SetActive(value);
        if (ForQuitButton) {
            Application.Quit();
            Debug.Log("Quit Games");
        }
        yield return null;

    }
    IEnumerator ButtonAnimateUp() {
        FocusButton.GetComponent<Image>().sprite = defaultSprite;

        if (smallButton)
        {
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0, 3);
            TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0, 4);
        }
        else {
            ColorButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(162, 43);
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.13f, 5);
            TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.19f, 5.97f);
        }
       
        yield return null;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonFocus))]
public class RandomScript_Editor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        ButtonFocus script = (ButtonFocus)target;
        if (script.ForSceneManager == true) {
            script.ForGameObject = false;
            script.NumberScene = EditorGUILayout.IntField("Direct Scene Number", script.NumberScene);
        }
            
        if (script.ForGameObject == true) {
            script.ForSceneManager = false;
            script.ActiveGameObject = EditorGUILayout.ObjectField("Active Object", script.ActiveGameObject, typeof(GameObject), true) as GameObject;
        }

    }
}

#endif
