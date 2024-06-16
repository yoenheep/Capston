using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    private void Start()
    {
        Debug.Log(slider);
        Debug.Log(mixer);
        slider.value = PlayerPrefs.GetFloat("MusicVol", 0.3f);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10 (sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }
}
