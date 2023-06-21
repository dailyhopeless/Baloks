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
    [HideInInspector]
    public int NumberScene;
    [HideInInspector]
    public GameObject ActiveGameObject;


    void Update()
    {
      
    }

    public void Focus (bool value)
    {
        if (value)
            FocusButton.GetComponent<Image>().sprite = FocusSpriteImage;
        StartCoroutine(ButtonAnimate(value));

    }

    public void FocusUp(bool value) {
        StartCoroutine(ButtonAnimateUp());
    }
    IEnumerator ButtonAnimate(bool value) {
        
        FocusButton.GetComponent<Image>().sprite = FocusSpriteImage;
        if (smallButton)
        {

        }
        else {
            ColorButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(162, 37);
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.13f, 2);
        }
        TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.19f, 0);
        if (ForSceneManager) {

            SceneManager.LoadScene(NumberScene);
        }
        yield return new WaitForSeconds(0.2f);
        if (ForGameObject) {
           
            ActiveGameObject.SetActive(value);
        }
        yield return null;

    }
    IEnumerator ButtonAnimateUp() {
        FocusButton.GetComponent<Image>().sprite = defaultSprite;
        if (smallButton)
        {

        }
        else {
            ColorButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(162, 43);
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.13f, 5);
        }
        TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.19f, 5.97f) ;
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

        // draw checkbox for the bool
        //script.ForSceneManager = EditorGUILayout.Toggle("For Scene Manager", script.ForSceneManager);
        if (script.ForSceneManager == true ) 
            script.ForGameObject = false;

        if (script.ForGameObject == true)
            script.ForSceneManager = false;

        if (script.ForSceneManager && !script.ForGameObject) // if bool is true, show other fields
            script.NumberScene = EditorGUILayout.IntField("Direct Scene Number", script.NumberScene)  ;

        if (script.ForGameObject && !script.ForSceneManager)
        {
            script.ActiveGameObject = EditorGUILayout.ObjectField("Active Object", script.ActiveGameObject, typeof(GameObject), true) as GameObject;
           
        }

    }
}

#endif
