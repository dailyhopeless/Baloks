using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class AudioGame : MonoBehaviour
{

    public bool setUIAudio;
    public bool setGameAudio;
    
    [Header("Audio UI")]
    [HideInInspector]
    public AudioClip buttonAudio;
    [HideInInspector]
    public AudioClip mainAudio;
    [HideInInspector]
    public AudioClip scoreAudio;


    [Header("Audio Game")]
    [HideInInspector]
    public AudioClip MainBackSound;
    [HideInInspector]
    public AudioClip MoveSound;
    [HideInInspector]
    public AudioClip rowSound;
    [HideInInspector]
    public AudioClip rotatioSound;
    [HideInInspector]
    public AudioClip swapSound;

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MusicPlayedBg();
        
    }




    public bool CheckSettingsMusic()
    {
        if (PlayerPrefs.GetInt(PlayerData.SettingsMusic.ToString()) == 2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool CheckSettingSounds()
    {
        if (PlayerPrefs.GetInt(PlayerData.SettingsSoundEffect.ToString()) == 2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SoundButton() {
        if (CheckSettingSounds())
            audioSource.PlayOneShot(buttonAudio);

    }
    public void MusicPlayedBg()
    {
        audioSource.clip = mainAudio;
        audioSource.loop = true;
        if (CheckSettingsMusic())
            audioSource.Play();
    }

    public void PlayLineClearedSound()
    {
        if (CheckSettingSounds())
            audioSource.PlayOneShot(rowSound);
    }
    public void SoundRotation()
    {
        if (CheckSettingSounds())
            audioSource.PlayOneShot(rotatioSound);
    }
    public void PlayMoveAudio()
    {
        if (CheckSettingSounds()) 
            audioSource.PlayOneShot(MoveSound);
    }
    public void SoundSwap()
    {
        if (CheckSettingSounds())
            audioSource.PlayOneShot(swapSound);

    }




}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioGame))]
public class EditorAudioGame : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AudioGame audioGame = (AudioGame)target;
        if (audioGame.setUIAudio)
        {
            audioGame.buttonAudio = EditorGUILayout.ObjectField("Audio Button", audioGame.buttonAudio, typeof(AudioClip), true) as AudioClip;
            audioGame.mainAudio = EditorGUILayout.ObjectField("Audio Backsound", audioGame.mainAudio, typeof(AudioClip), true) as AudioClip;
            audioGame.scoreAudio = EditorGUILayout.ObjectField("Audio Point", audioGame.mainAudio, typeof(AudioClip), true) as AudioClip;
        }
        if (audioGame.setGameAudio) {
            audioGame.MoveSound = EditorGUILayout.ObjectField("Audio Button", audioGame.buttonAudio, typeof(AudioClip), true) as AudioClip;
            audioGame.rowSound = EditorGUILayout.ObjectField("Audio Button", audioGame.buttonAudio, typeof(AudioClip), true) as AudioClip;
            audioGame.swapSound = EditorGUILayout.ObjectField("Audio Button", audioGame.buttonAudio, typeof(AudioClip), true) as AudioClip;
            audioGame.rotatioSound = EditorGUILayout.ObjectField("Audio Button", audioGame.buttonAudio, typeof(AudioClip), true) as AudioClip;

        }

    }

}


#endif