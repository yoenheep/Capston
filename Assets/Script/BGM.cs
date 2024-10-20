using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip main;
    public AudioClip middleBoss;
    public AudioClip lastBoss;
    public AudioClip gameClear;
    public AudioClip gameOver;

    //ΩÃ±€≈Ê
    public static BGM instance { get; private set; }
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = main;

        instance = this;
    }

    private void Update()
    {
        if (GameUI.UIData.clearPopup.activeSelf == false && GameUI.UIData.overPopup.activeSelf == false && GameManager.gameMgr.nowStage != 8)
        {
            audioSource.clip = main;
        }
    }
}
