using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    private AudioSource bgmAudio;
    private GameObject[] musics;

    private void Awake()
    {
        musics = GameObject.FindGameObjectsWithTag("Music");

        if(musics.Length >= 2)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        bgmAudio = GetComponent<AudioSource>();

        bgmAudio.volume = 0.3f;
    }

    public void PlayMusic()
    {
        if (bgmAudio.isPlaying) return;
        bgmAudio.Play();
    }
}
