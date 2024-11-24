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
    public GameObject loginSignBtn;
    public bool loginBool;

    [Header("# signup")]
    public TMP_InputField signupIdInput;
    public TMP_InputField signupPWDInput;
    public GameObject signupUI;

    [Header("# warning")]
    public TextMeshProUGUI warningTxt;
    public GameObject warningUI;

    [Header("# Ranking")]
    public RankItem[] rankItems;
    public GameObject rankUI;

    public void OpenUI(GameObject ui)
    {
        ui.SetActive(true);
    }
    public void rankingSystem()
    {
        // 데이터베이스에서 1위부터 10위까지의 랭킹 데이터를 가져옴
        XmlNodeList rankingData = MySQLConnection.Select("ranking", "ORDER BY time ASC LIMIT 5");

        if (rankingData != null)
        {
            int rank = 0;

            foreach (XmlNode data in rankingData)
            {
                rankItems[rank].SetRankingData(data["ID"].InnerText, int.Parse(data["time"].InnerText));
                rank++;
            }
        }
        else
        {
            Debug.Log("랭킹 데이터를 가져오지 못했습니다.");
        }
    }
    public void CloseUi(GameObject ui)
    {
        ui.SetActive(false);

        signupIdInput.text = "";
        signupPWDInput.text = "";
    }

    public void WarningClose()
    {
        warningUI.SetActive(false);
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
                warningTxt.text = "이미 존재하는 아이디입니다.";
                warningUI.SetActive(true);

                Invoke("WarningClose", 3f);
            }
            else
            {
                if (pwd.ToString().Length == 4)
                {
                    MySQLConnection.Insert("users", $"'{playerName}', {pwd}");
                    //회원가입 
                    Debug.Log("회원가입 성공");
                    warningTxt.text = "회원가입 성공";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
                else
                {
                    //비밀번호는 4자리입니다. 4일때만 입력가능
                    Debug.Log("비밀번호는 4자리입니다.");
                    warningTxt.text = "비밀번호는 4자리입니다.";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
            }
        }
        else if (string.IsNullOrEmpty(signupIdInput.text) || (!string.IsNullOrEmpty(signupPWDInput.text)))
        {
            //아이디가 비어있을 때
            Debug.Log("ID를 입력해주세요.");
            warningTxt.text = "아이디를 입력해주세요";
            warningUI.SetActive(true);

            Invoke("WarningClose", 3f);
        }
        else if (string.IsNullOrEmpty(signupPWDInput.text))
        {
            //비밀번호가 비어있을 때
            Debug.Log("비밀번호를 입력해주세요");
            warningTxt.text = "비밀번호를 입력해주세요";
            warningUI.SetActive(true);

            Invoke("WarningClose", 3f);
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
                    UserData.instance.userName = playerName;

                    UserData.instance.isLoggedIn = true;
                }
                else
                {
                    //OpenUI("alert");
                    //alertText.text = "비밀번호가 잘못되었습니다";
                    Debug.Log("비밀번호를 입력해주세요");
                    warningTxt.text = "비밀번호가 잘못되었습니다";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
            }
            else
            {
                // 존재하지않은 아이디입니다.
                Debug.Log("존재하지 않은 아이디입니다");
                warningTxt.text = "존재하지 않은 아이디입니다";
                warningUI.SetActive(true);

                Invoke("WarningClose", 3f);
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
        warningUI.SetActive(false);
        rankUI.SetActive(false);
        signupUI.SetActive(false);

        if (UserData.instance.isLoggedIn)
        {
            Debug.Log("로그인 중 : "+ UserData.instance.userName);
            loginSignupUI.SetActive(false); // 로그인 창 비활성화
            loginUI.SetActive(false);
        }
        else
        {
            loginSignupUI.SetActive(true);
            loginUI.SetActive(true); // 로그인 창 활성화
        }

        if (AudioPlayBGM.instance != null)
        {
            AudioPlayBGM.instance.ChangeClip(AudioPlayBGM.instance.Game);
        }
        else
        {
            Debug.LogError("AudioPlayBGM 인스턴스가 없습니다.");
        }
    }

    private void Update()
    {
        if (signupUI.activeSelf == true)
        {
            loginSignBtn.SetActive(false);
        }
        else
        {
            loginSignBtn.SetActive(true);
        }
    }

    public void Clicked_start() //시작하기
    {
        loginBool = true;
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -78);
        PlayerPrefs.SetFloat("SaveY", 11);
        PlayerPrefs.SetInt("SaveStage", 0);
    }
    public void quit() //종료하기
    {
        loginBool = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }
}
