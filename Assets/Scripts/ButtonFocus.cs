using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode] //perubahan nilai akan di eksekusi secara langsung pada inspector
public class ButtonFocus : MonoBehaviour
{

    public bool useSpriteButton;
    public Sprite FocusSpriteImage;
    public Sprite defaultSprite;
    public GameObject FocusButton;

    // if have background color
    public bool haveBackgroundColor;
    public GameObject ColorButton;
    public Vector2 ColorButtonPositionBefore;
    public Vector2 ColorButtonPositionAfter;

    // if have icon in button
    public bool icon;
    public GameObject iconButton;
    public Vector2 iconButtonBefore;
    public Vector2 iconButtonAfter;

    // if have text in button;
    public bool text;
    public GameObject TextButton;
    public Vector2 TextButtonBefore;
    public Vector2 TextButtonAfter;

    // if for input
    public bool setInput;
    public enum NameInput
    {
        left,
        right,
        down,
        rotation,
        swap

    }
    public NameInput nameInput;
    public static bool left;
    public static bool right;
    public static bool down;
    public static bool swap;
    public static bool rotation;
    public static bool downUp;
    public static bool leftrightUp;

    //for close
    public bool close;
    public GameObject closeTrigger;

    //for change scene
    public bool ForSceneManager;
    public int NumberScene;
    public bool setTransition;
    public GameObject playTrasition;
    public RuntimeAnimatorController newController;

    // for Panel
    public bool ForGameObject;
    public GameObject ActiveGameObject;

    //for quit
    public bool ForQuitButton;

    //for reset
    public bool resetButton;


    void Start()
    {
        //Debug.Log(inputs[0].button);

    }
    void Update()
    {

    }
    private void OnEnable()
    {
        DefaultButtonBefore();
    }
    /// <summary>
    /// Focus for button push add event trigger put in pointer down
    /// </summary>
    public void Focus(bool value)
    {
        StartCoroutine(ButtonAnimate(value));

    }
    /// <summary>
    /// Focusup for effect button add to event triger pointer up
    /// </summary>
    public void FocusUp(bool value)
    {
        StartCoroutine(ButtonAnimateUp(value));
    }
    void ButtonInput(bool value) {
        if (nameInput.ToString() == "left")
            left = value;
        else if (nameInput.ToString() == "right")
            right = value;
        if (nameInput.ToString() == "down")
            down = value;
        if (nameInput.ToString() == "rotation")
            rotation = value;
    }


    void DefaultButtonBefore() {
        FocusButton.GetComponent<Image>().sprite = defaultSprite;
        if (haveBackgroundColor)
        {
            ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = ColorButtonPositionBefore;
        }
        if (text)
        {
            TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = TextButtonBefore;
        }
        if (icon)
        {
            iconButton.GetComponent<Image>().rectTransform.anchoredPosition = iconButtonBefore;
        }
    }

    IEnumerator ButtonAnimate(bool value)
    {
        if (useSpriteButton) {
            FocusButton.GetComponent<Image>().sprite = FocusSpriteImage;
            if (haveBackgroundColor) {
                ColorButton.GetComponent<Image>().rectTransform.anchoredPosition = ColorButtonPositionAfter;
            }
            if (text) {
                TextButton.GetComponent<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition = TextButtonAfter;
            }
            if (icon) {
                iconButton.GetComponent<Image>().rectTransform.anchoredPosition = iconButtonAfter;
            }
         
        }

        if (setInput) {
            ButtonInput(value);
        }


        yield return new WaitForSecondsRealtime(0.2f);

        if (ForGameObject)
            ActiveGameObject.SetActive(value);

        if ((ForQuitButton || ForSceneManager))
        {
            Time.timeScale = 1;
            if (setTransition) {
                playTrasition.GetComponent<Animator>().runtimeAnimatorController = newController;
             
                yield return new WaitForSecondsRealtime(playTrasition.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }
            
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
        if (close)
        {
            closeTrigger.SetActive(false);
        }

        yield return null;

    }
    IEnumerator ButtonAnimateUp(bool value)
    {
        if (useSpriteButton) {
            DefaultButtonBefore();
        }
        if (setInput) {
            ButtonInput(value);
            if (nameInput.ToString() == "down")
                downUp = true;
            if (nameInput.ToString() == "left" || (nameInput.ToString() == "right"))
                leftrightUp = true;
            if (nameInput.ToString() == "swap")
                swap = true;
        }

        yield return null;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonFocus))]
[CanEditMultipleObjects]
public class RandomScript_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector(); // menampilkan seluruh property pada inpector termasuk yang di disembunyikan.
        
        
        ButtonFocus script = (ButtonFocus)target;

        EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);

        script.useSpriteButton = EditorGUILayout.Toggle("Change Sprite", script.useSpriteButton);
        if (script.useSpriteButton) 
        {
            script.FocusSpriteImage = EditorGUILayout.ObjectField("Sprite After", script.FocusSpriteImage, typeof(Sprite), true) as Sprite;
            script.defaultSprite = EditorGUILayout.ObjectField("Sprite Before", script.defaultSprite, typeof(Sprite), true) as Sprite;
            script.FocusButton = EditorGUILayout.ObjectField("Target Change Sprite", script.FocusButton, typeof(GameObject), true) as GameObject;
            script.haveBackgroundColor = EditorGUILayout.Toggle("Background Color", script.haveBackgroundColor);
         
            if (script.haveBackgroundColor) {
               
                script.ColorButton = EditorGUILayout.ObjectField("Background Color", script.ColorButton, typeof(GameObject), true) as GameObject;
                EditorGUILayout.Space();
                script.ColorButtonPositionBefore = EditorGUILayout.Vector2Field("Position Before", script.ColorButtonPositionBefore);
                script.ColorButtonPositionAfter = EditorGUILayout.Vector2Field("Position After", script.ColorButtonPositionAfter);
                EditorGUILayout.Space();
            }
            script.text = EditorGUILayout.Toggle("Text", script.text);
            if (script.text) {

                script.TextButton = EditorGUILayout.ObjectField("Text Button", script.TextButton, typeof(GameObject), true) as GameObject;
                EditorGUILayout.Space();
                script.TextButtonBefore = EditorGUILayout.Vector2Field("Position Before", script.TextButtonBefore);
                script.TextButtonAfter = EditorGUILayout.Vector2Field("Position After", script.TextButtonAfter);
                EditorGUILayout.Space();
            }
            script.icon = EditorGUILayout.Toggle("Icon", script.icon);
            if (script.icon) {
                script.iconButton = EditorGUILayout.ObjectField("Icon Button", script.iconButton, typeof(GameObject), true) as GameObject;
                EditorGUILayout.Space();
                script.iconButtonBefore = EditorGUILayout.Vector2Field("Position Before", script.iconButtonBefore);
                script.iconButtonAfter = EditorGUILayout.Vector2Field("Position After", script.iconButtonAfter);
                EditorGUILayout.Space();
            }

        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Functions Buttons", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        script.close = EditorGUILayout.Toggle("Close", script.close);
        if (script.close)
        {
           script.closeTrigger = EditorGUILayout.ObjectField("trigger zoom out", script.closeTrigger, typeof(GameObject), true) as GameObject;
           EditorGUILayout.Space();
        }


        script.ForSceneManager = EditorGUILayout.Toggle("Direct Scene", script.ForSceneManager);
        if (script.ForSceneManager)
        {
            script.ForGameObject = false;
            script.ForQuitButton = false;
            script.resetButton = false;
            script.setInput = false;
            script.NumberScene = EditorGUILayout.IntField("Direct Scene Number", script.NumberScene);
            script.setTransition = EditorGUILayout.Toggle("Add Animation transition", script.setTransition);
            if (script.setTransition)
            {
                script.playTrasition = EditorGUILayout.ObjectField("Transition Animation", script.playTrasition, typeof(GameObject), true) as GameObject;
                script.newController = EditorGUILayout.ObjectField("Controller reverse", script.newController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
            }
            EditorGUILayout.Space();
        }
        script.ForGameObject = EditorGUILayout.Toggle("Panel Canvas", script.ForGameObject);
        if (script.ForGameObject)
        {
            script.ForSceneManager = false;
            script.ForQuitButton = false;
            script.resetButton = false;
            script.setInput = false;
            script.ActiveGameObject = EditorGUILayout.ObjectField("Game Object For Panel", script.ActiveGameObject, typeof(GameObject), true) as GameObject;
            EditorGUILayout.Space();
        }
        script.ForQuitButton = EditorGUILayout.Toggle("Quit Button", script.ForQuitButton);
        if (script.ForQuitButton)
        {
            script.ForSceneManager = false;
            script.ForGameObject = false;
            script.resetButton = false;
            script.setInput = false;
            script.setTransition = EditorGUILayout.Toggle("Add Animation transition", script.setTransition);
            if (script.setTransition)
            {
                script.playTrasition = EditorGUILayout.ObjectField("Transition Animation", script.playTrasition, typeof(GameObject), true) as GameObject;
                script.newController = EditorGUILayout.ObjectField("Controller reverse", script.newController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
                
            }
            EditorGUILayout.Space();
        }
        script.resetButton = EditorGUILayout.Toggle("Reset Button", script.resetButton);
        if (script.resetButton)
        {
            script.ForGameObject = false;
            script.ForQuitButton = false;
            script.ForSceneManager = false;
            script.setInput = false;
        }
        script.setInput = EditorGUILayout.Toggle("Input Button", script.setInput);
        if (script.setInput)
        {
            script.ForGameObject = false;
            script.ForQuitButton = false;
            script.ForSceneManager = false;
            script.resetButton = false;
            script.nameInput = (ButtonFocus.NameInput)EditorGUILayout.EnumPopup("name", script.nameInput);
            EditorGUILayout.Space();

        }

    }
}

#endif
