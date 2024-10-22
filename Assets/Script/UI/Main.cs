using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using TMPro;

public class Main : MonoBehaviour
{
    [Header("# gameObj")]
    [SerializeField] private GameObject setPopup;
    [SerializeField] private GameObject mainPage;

    // Start is called before the first frame update

    [Header("# login")]
    public TMP_InputField loginIdInput;
    public TMP_InputField loginPWDInput;
    public GameObject loginUI;
    public GameObject loginSignupUI;

    [Header("# signup")]
    public TMP_InputField signupIdInput;
    public TMP_InputField signupPWDInput;
    public GameObject signupUI;

    public void CloseUi(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void Signup()
    {
        if (!string.IsNullOrEmpty(signupIdInput.text) && !string.IsNullOrEmpty(signupPWDInput.text))
        {
            string playerName = signupIdInput.text;
            int pwd = int.Parse(signupPWDInput.text);

            XmlNodeList user = MySQLConnection.Select("users", $"WHERE ID = '{playerName}'");

            if (user != null)
            {
                //이미 존재하는 아이디입니다.
                Debug.Log("이미 존재하는 아이디입니다.");
            }
            else
            {
                if (pwd.ToString().Length == 4)
                {
                    MySQLConnection.Insert("users", $"'{playerName}', {pwd}");
                    //회원가입 
                    Debug.Log("회원가입 성공");
                }
                else
                {
                    //비밀번호는 4자리입니다. 4일때만 입력가능
                    Debug.Log("비밀번호는 4자리입니다.");
                }
            }
        }
        else if (string.IsNullOrEmpty(signupIdInput.text) || (!string.IsNullOrEmpty(signupPWDInput.text)))
        {
            //아이디가 비어있을 때
            Debug.Log("ID를 입력해주세요.");
        }
        else if (string.IsNullOrEmpty(signupPWDInput.text))
        {
            //비밀번호가 비어있을 때
            Debug.Log("비밀번호를 입력해주세요");
        }
    }

    public void Login()
    {
        if (!string.IsNullOrEmpty(loginIdInput.text) && !string.IsNullOrEmpty(loginPWDInput.text))
        {
            string playerName = loginIdInput.text;
            string pwd = loginPWDInput.text;

            XmlNodeList user = MySQLConnection.Select("users", $"WHERE ID = '{playerName}'");

            if (user != null)
            {
                if (pwd == user[0]["Password"].InnerText.ToString())
                {
                    loginSignupUI.SetActive(false);
                }
                else
                {
                    //OpenUI("alert");
                    //alertText.text = "비밀번호가 잘못되었습니다";
                }
            }
            else
            {
                // 존재하지않은 아이디입니다.
            }
        }
        else if (string.IsNullOrEmpty(loginIdInput.text) || (!string.IsNullOrEmpty(loginPWDInput.text)))
        {
            //아이디가 비어있을 때
        }
        else if (string.IsNullOrEmpty(loginPWDInput.text))
        {
            //비밀번호가 비어있을 때
        }
    }
    void Awake()
    {
        mainPage.SetActive(true);
        setPopup.SetActive(false);
    }

    public void Clicked_start() //시작하기
    {
        //저장한 곳으로 이동하는
        SceneManager.LoadScene("Game");
    }
    public void set() //설정창 열기
    {
        setPopup.SetActive(true);
    }
    public void re() //새로시작하기
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -78);
        PlayerPrefs.SetFloat("SaveY", 11);
        PlayerPrefs.SetInt("SaveStage", 0);
    }
    public void quit() //종료하기
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }
    public void x()
    {
        setPopup.SetActive(false);
    }

}
