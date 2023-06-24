using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SliderSettings : MonoBehaviour
{
    [Header("Settings Slider")]
    public bool longSlider;
    public bool toggleSlider;
    public PlayerData saveData;

    [HideInInspector]
    public TMPro.TextMeshProUGUI countSpeedText;

    [HideInInspector]
    public GameObject ifDisable;
    [HideInInspector]
    public GameObject haddle;

    private Slider slider;


    void Start()
    {
        Checkifnull();
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        slider.value = PlayerPrefs.GetInt(saveData.ToString());

        

    }


    public void ValueChangeCheck()
    {
        PlayerPrefs.SetInt(saveData.ToString(), (int)slider.value);
        if(longSlider)
            countSpeedText.text = Mathf.Floor(slider.value).ToString();
        if (toggleSlider) {
           
            if (slider.value == 1)
            {
                ifDisable.SetActive(true);
                haddle.GetComponent<Image>().color = new Color32(204, 10, 10, 255);
            }
            else {
                ifDisable.SetActive(false);
                haddle.GetComponent<Image>().color = new Color32(9, 209, 13, 255);
            }
               
        }
    }

    public void HandleButton() {
        if (slider.value == 1)
            slider.value = 2;
        else
            slider.value = 1; 
    }


    void Checkifnull() {
        if (toggleSlider)
        {
            if (PlayerPrefs.GetInt(saveData.ToString()) == 0)
            {
                PlayerPrefs.SetInt(saveData.ToString(), 2);
            }
        }
        else if (longSlider) {
            if (PlayerPrefs.GetInt(saveData.ToString()) == 0) 
            {
                PlayerPrefs.SetInt(saveData.ToString(), 6);
            }
        }
        
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(SliderSettings))]

public class SliderSettingsEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SliderSettings slider = (SliderSettings)target;

        if (slider.longSlider == true)
        {
            slider.toggleSlider = false;
            slider.countSpeedText = EditorGUILayout.ObjectField("Text MeshPro", slider.countSpeedText, typeof(TMPro.TextMeshProUGUI), true) as TMPro.TextMeshProUGUI;
        }
        if (slider.toggleSlider == true)
        {
            slider.longSlider = false;
            slider.ifDisable = EditorGUILayout.ObjectField("Change Color Disable", slider.ifDisable, typeof(GameObject), true) as GameObject;
            slider.haddle = EditorGUILayout.ObjectField("Handle Slider", slider.haddle, typeof(GameObject), true) as GameObject;
        }    
    }
}
#endif