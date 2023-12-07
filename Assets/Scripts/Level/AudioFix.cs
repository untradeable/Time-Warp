using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioFix : MonoBehaviour
{
    public TMP_Text volumeSliderTMP;

    private void Start()
    {
        AudioListener.volume = 0.2f;

        AudioListener.pause = true;
        AudioListener.pause = false;

        Time.timeScale = 0f;
        Time.timeScale = 1f;
    }

    public void AudioSlider(float volume)
    {
        volumeSliderTMP.text = (volume * 100).ToString("0") + "%";
        AudioListener.volume = volume;
    }

    // não questione o motivo disso estar no audio fix
    public void SetFullScreen(bool self)
    {
        Screen.fullScreen = self;
    }

}