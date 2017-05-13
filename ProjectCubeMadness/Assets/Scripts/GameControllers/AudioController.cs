using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clip = UnityEngine.AudioClip;

public class AudioController : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource uiSource;

    public Clip[] backgroundMusic;
    public Clip pauseClip;

    private void Start()
    {
        InputHandler.MenuController.onPause += PlayPause;
    }

    private void PlayPause()
    {
        uiSource.clip = pauseClip;
        uiSource.Play();
        if (GameState.IsPaused)
            musicSource.Pause();
        else
            musicSource.UnPause();
    }
}
