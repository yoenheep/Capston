using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZText : MonoBehaviour
{
    public Transform owner; // Z ����
    public Vector3 ZPos;

    private void Update()
    {
        Vector3 _ZPos = Camera.main.WorldToScreenPoint(owner.transform.position + ZPos);
        transform.position = _ZPos;// ��ġ �ǽð� ����
    }
}
