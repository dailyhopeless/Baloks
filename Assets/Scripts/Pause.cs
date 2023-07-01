using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public AudioSource mainMusic;
    public GameObject trigger;
    public static bool isPaused = false;
    

    void Start()
    {
        
    }
    void Update()
    {
        StartCoroutine(Pauseaktive());
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    IEnumerator Pauseaktive() {
        if (gameObject.activeSelf)
        {
            isPaused = true;
            if (isPaused)
            {
                Time.timeScale = 0;
            }
            mainMusic.Pause();
        }
        if (!trigger.activeSelf)
        {
            isPaused = false;
            if (!isPaused)
            {
                Time.timeScale = 1;
            }
            mainMusic.Play();
        }

        yield return null;
    }




}
