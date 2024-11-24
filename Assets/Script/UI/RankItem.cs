using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText; // 플레이어 이름 텍스트
    [SerializeField] TextMeshProUGUI playerScoreText; // 플레이어 시간 텍스트

    public void SetRankingData(string name, int score)
    {
        // Rank를 2자리 숫자로 분리하여 이미지에 설정
        //rankImg.sprite = rankImg_No[rank / 10]; // 앞자리
        //rankImg2.sprite = rankImg_No[rank % 10]; // 뒷자리

        // 나머지 데이터 설정
        playerNameText.text = name;
        playerScoreText.text = $"{score / 60}:{score % 60}";
    }
}
