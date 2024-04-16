using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monHpBar : MonoBehaviour
{
    [Header("#HP")]
    //[SerializeField] private GameObject canvas;
    public Transform owner; // ü�¹� ����
    public GameObject parent; // ü�¹� �θ� ������Ʈ (canvas)
    public Image nowHpBar; // ���� hp image
    public float maxHp; // �ִ� hp
    public float nowHp; // ���� hp
    public Vector3 hpBarPos;

    private void OnEnable()
    {
        parent = Monster_Spawn_Manager.instance.hpBarParent;

        transform.SetParent(parent.transform);// �θ� ����
    }
    private void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(owner.transform.position + hpBarPos);
        transform.position = _hpBarPos;// ��ġ �ǽð� ����

        nowHpBar.fillAmount = nowHp / maxHp; // �̹��� ��ȭ ����
    }
}
