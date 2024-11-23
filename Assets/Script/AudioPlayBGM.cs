using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayBGM : MonoBehaviour
{
    public AudioSource bgmAudio;
    private GameObject[] Musics;

    public AudioClip Game;
    public AudioClip middleBoss;
    public AudioClip lastBoss;
    public AudioClip gameClear;
    public AudioClip gameOver;

    //싱글톤
    public static AudioPlayBGM instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        bgmAudio = GetComponent<AudioSource>();

        float bgmVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = bgmVolume;
    }

    private void Update()
    {

        float bgmVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = bgmVolume;
    }

    public void ChangeClip(AudioClip newClip)
    {
        if (bgmAudio == null)
        {
            Debug.LogError("AudioSource가 초기화되지 않았습니다.");
            return;
        }

        if (bgmAudio.clip == newClip)
        {
            Debug.Log("같은 클립으로 변경 시도, 무시합니다.");
            return;
        }

        bgmAudio.clip = newClip;
        bgmAudio.Play();
    }
}