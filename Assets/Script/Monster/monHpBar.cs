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
    public Transform owner; // ü�¹� ����
    public GameObject parent; // ü�¹� �θ� ������Ʈ (canvas)
    public Image nowHpBar; // ���� hp image
    public Image nowHpBackBar;
    public float maxHp; // �ִ� hp
    public float nowHp; // ���� hp
    public Vector3 hpBarPos;
    bool backHpHit;

    private void OnEnable()
    {
            parent = Monster_Spawn_Manager.instance.hpBarParent;

            transform.SetParent(parent.transform);// �θ� ����
    }
    private void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(owner.transform.position + hpBarPos);
        transform.position = _hpBarPos;// ��ġ �ǽð� ����

        nowHpBar.fillAmount = Mathf.Lerp(nowHpBar.fillAmount, nowHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit )
        {
            nowHpBackBar.fillAmount = Mathf.Lerp(nowHpBackBar.fillAmount, nowHpBar.fillAmount, Time.deltaTime * 10f);

            if(nowHpBar.fillAmount >= nowHpBackBar.fillAmount - 0.01f)
            {
                backHpHit = false;
                nowHpBackBar.fillAmount = nowHpBar.fillAmount;
            }
        }
    }

    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }

    void BackHpFun()
    {
        backHpHit = true;
    }
}
