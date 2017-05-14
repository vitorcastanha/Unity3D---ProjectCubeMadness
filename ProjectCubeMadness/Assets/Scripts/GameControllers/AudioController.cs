using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clip = UnityEngine.AudioClip;

public class AudioController : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource uiSource;

    public Clip[] backgroundMusic;
    public Clip pauseClip;
    public Clip healingClip;

    private void Start()
    {
        InputHandler.MenuController.onPause += PlayPause;
        HeroCharacter.GetInstance().HeroStateHandler.onHealingCallBack += PlayHealing;
    }

    private void PlayPause()
    {
        uiSource.PlayOneShot(pauseClip);
        if (GameState.IsPaused)
            musicSource.Pause();
        else
            musicSource.UnPause();
    }

    private void PlayHealing()
    {
        uiSource.PlayOneShot(healingClip, 2f);
    }
}
