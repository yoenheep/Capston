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
        instance = this;
        Musics = GameObject.FindGameObjectsWithTag("Music");

        if (Musics.Length >= 3)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        bgmAudio = GetComponent<AudioSource>();

        float bgmVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = bgmVolume;
    }

    private void Update()
    {
        bgmAudio.ignoreListenerPause = true;

        float bgmVolume = PlayerPrefs.GetFloat("MusicVol");

        bgmAudio.volume = bgmVolume;

        if (bgmAudio.clip != Game)
        {
            bgmAudio.clip = Game;

            // 오디오가 재생 중이지 않으면 재생
            if (!AudioPlayBGM.instance.bgmAudio.isPlaying)
            {
                AudioPlayBGM.instance.bgmAudio.Play();
            }
        }
        //null 오류
        //수정 필요
        /*if (GameUI.UIData.clearPopup.activeSelf == false && GameUI.UIData.overPopup.activeSelf == false && GameManager.gameMgr.nowStage != 6 && GameManager.gameMgr.nowStage != 8)
        {
            
        }*/
    }

    public void PlayMusic()
    {
        if (bgmAudio.isPlaying) return;
        bgmAudio.Play();
    }
}