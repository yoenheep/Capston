using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public string userName;
        public bool isLoggedIn = false; // �α��� ����

    private void Awake()
    {
        var obj = FindObjectsOfType<UserData>();

        if (instance == null && obj.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �������� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϸ� �ߺ� ����
        }
    }
}
