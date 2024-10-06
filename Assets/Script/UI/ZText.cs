using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZText : MonoBehaviour
{
    public Transform owner; // Z 주인
    public Vector3 ZPos;

    private void Update()
    {
        Vector3 _ZPos = Camera.main.WorldToScreenPoint(owner.transform.position + ZPos);
        transform.position = _ZPos;// 위치 실시간 적용
    }
}
