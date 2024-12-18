using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class monHpBar : MonoBehaviour
{
    [Header("#HP")]
    public Transform owner; // 체력바 주인
    public GameObject parent; // 체력바 부모 오브젝트 (canvas)
    public Image nowHpBar; // 현재 hp image
    public float maxHp; // 최대 hp
    public float nowHp; // 현재 hp
    public Vector3 hpBarPos;

    private void OnEnable()
    {
            parent = Monster_Spawn_Manager.instance.hpBarParent;

            transform.SetParent(parent.transform);// 부모 설정
    }
    private void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(owner.transform.position + hpBarPos);
        transform.position = _hpBarPos;// 위치 실시간 적용

        nowHpBar.fillAmount = Mathf.Lerp(nowHpBar.fillAmount, nowHp / maxHp, Time.deltaTime * 5f);
    }
}
