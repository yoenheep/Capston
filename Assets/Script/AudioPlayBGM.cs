using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayBGM : MonoBehaviour
{
    private AudioSource bgmAudio;
    private GameObject[] Musics;

    private void Awake()
    {
        Musics = GameObject.FindGameObjectsWithTag("Music");

        if (Musics.Length >= 2)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        bgmAudio = GetComponent<AudioSource>();

        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = sfxVolume;
    }

    private void Update()
    {
        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = sfxVolume;
    }

    public void PlayMusic()
    {
        if (bgmAudio.isPlaying) return;
        bgmAudio.Play();
    }
}