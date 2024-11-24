using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public string userName;
        public bool isLoggedIn = false; // 로그인 상태

    private void Awake()
    {
        var obj = FindObjectsOfType<UserData>();

        if (instance == null && obj.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제되지 않음
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 중복 제거
        }
    }
}
