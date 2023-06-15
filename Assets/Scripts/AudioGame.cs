using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioGame : MonoBehaviour
{

    public static AudioSource audioSource;
    public AudioClip MainBackSound;
    public AudioClip MoveSound;
    public AudioClip rowSound;
    public AudioClip rowHighSound;
    public AudioClip dropSound;

    // Start is called before the first frame update
    void Start()
    {
        
        MusicPlayedBg();
        
    }

    // Update is called once per frame
    void Update()
    {
      
        //SoundEfecttBool();


    }
    public void MusicPlayedBg() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MainBackSound;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        if (MusicBool())
            audioSource.Play();
    }

    public void PlayLineClearedSound()
    {
        if (SoundEfecttBool())
            audioSource.PlayOneShot(rowSound);
    }
    public void PlayLineClearedSoundHight()
    {
        if (SoundEfecttBool())
            audioSource.PlayOneShot(rowHighSound);
    }
    public void PlayMoveAudio()
    {
        if (SoundEfecttBool()) 
            audioSource.PlayOneShot(MoveSound);
    }
    public void PlayLandAudio()
    {
        if (SoundEfecttBool())
            audioSource.PlayOneShot(dropSound);

    }

    public bool SoundEfecttBool() {
        if (PlayerPrefs.GetInt("SettingsSoundEffect") == 1) {
            return true;
        } else {
            return false;
        }
        
    }
    public bool MusicBool()
    {
        if (PlayerPrefs.GetInt("SettingsMusic") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }





}
