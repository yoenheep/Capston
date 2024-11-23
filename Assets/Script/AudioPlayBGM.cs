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

    //�̱���
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
            Debug.LogError("AudioSource�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        if (bgmAudio.clip == newClip)
        {
            Debug.Log("���� Ŭ������ ���� �õ�, �����մϴ�.");
            return;
        }

        bgmAudio.clip = newClip;
        bgmAudio.Play();
    }
}