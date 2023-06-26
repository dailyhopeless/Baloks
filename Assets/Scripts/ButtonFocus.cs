using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;

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

    [Header("Type Button")]
    public bool smallButton;
    public bool setAudio;
    [HideInInspector]
    public GameObject audioObject;

    [Header("Setting Button")]
    public bool close;
    [HideInInspector]
    public GameObject closeTrigger;

    public bool ForSceneManager;
    [HideInInspector]
    public int NumberScene;
    [HideInInspector]
    public bool setTransition;
    [HideInInspector]
    public GameObject playTrasition;
    [HideInInspector]
    public RuntimeAnimatorController newController;
    //public Animator

    public bool ForGameObject;
    [HideInInspector]
    public GameObject ActiveGameObject;

    public bool ForQuitButton;
    public bool resetButton;






    void Start()
    {


    }
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
        yield return new WaitForSecondsRealtime(0.2f);

        if (ForGameObject) 
            ActiveGameObject.SetActive(value);
    
        if ((ForQuitButton || ForSceneManager) && setTransition) {
            playTrasition.GetComponent<Animator>().runtimeAnimatorController = newController;

            yield return new WaitForSecondsRealtime(playTrasition.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            if (ForQuitButton)
            {
                Application.Quit();
                Debug.Log("Quit Games");
            }
            if (ForSceneManager)
                SceneManager.LoadScene(NumberScene);
        }

        if (resetButton)
        {
            foreach (PlayerData playerData in Enum.GetValues(typeof(PlayerData)))
            {
                PlayerPrefs.SetInt(playerData.ToString(), 0);
            }
        }
        if (close) {
            closeTrigger.SetActive(false);
        }

        if (setAudio) {
            audioObject.GetComponent<AudioGame>().SoundButton();
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




        if (script.ForSceneManager == true)
        {
            script.ForGameObject = false;
            script.ForQuitButton = false;
            script.resetButton = false;
            script.NumberScene = EditorGUILayout.IntField("Direct Scene Number", script.NumberScene);
            script.setTransition = EditorGUILayout.Toggle("Add Animation transition", script.setTransition);
            if (script.setTransition)
            {
                script.playTrasition = EditorGUILayout.ObjectField("Transition Animation", script.playTrasition, typeof(GameObject), true) as GameObject;
                script.newController = EditorGUILayout.ObjectField("Controller reverse", script.newController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
            }
        }

        if (script.ForGameObject == true)
        {
            script.ForSceneManager = false;
            script.ForQuitButton = false;
            script.resetButton = false;
            script.ActiveGameObject = EditorGUILayout.ObjectField("Game Object For Panel", script.ActiveGameObject, typeof(GameObject), true) as GameObject;
        }
        if (script.ForQuitButton == true)
        {
            script.ForSceneManager = false;
            script.ForGameObject = false;
            script.resetButton = false;
            script.setTransition = EditorGUILayout.Toggle("Add Animation transition", script.setTransition);
            if (script.setTransition) {
                script.playTrasition = EditorGUILayout.ObjectField("Transition Animation", script.playTrasition, typeof(GameObject), true) as GameObject;
                script.newController = EditorGUILayout.ObjectField("Controller reverse", script.newController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
            }
        }
        if (script.resetButton == true)
        {
            script.ForGameObject = false;
            script.ForQuitButton = false;
            script.ForSceneManager = false;
        }
        if (script.close) {
            script.closeTrigger = EditorGUILayout.ObjectField("trigger zoom out", script.closeTrigger, typeof(GameObject), true) as GameObject;
        }
        if (script.setAudio) {
            script.audioObject = EditorGUILayout.ObjectField("Add Audio Game Object", script.audioObject, typeof(GameObject), true) as GameObject;
        }

    }
}

#endif
