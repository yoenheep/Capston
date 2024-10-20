using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaySFX : MonoBehaviour
{
    private AudioSource sfxAudio;
    private GameObject[] sfx;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectsWithTag("Music");

        if (sfx.Length >= 3)
        {
            Destroy(this.gameObject);
        }

        sfxAudio = GetComponent<AudioSource>();

        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        sfxAudio.volume = sfxVolume;
    }

    private void Update()
    {
        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        sfxAudio.volume = sfxVolume;
    }

    public void PlaySFX()
    {
        if (sfxAudio.isPlaying) return;
        sfxAudio.Play();
    }
}