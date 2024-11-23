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

    [Header("# signup")]
    public TMP_InputField signupIdInput;
    public TMP_InputField signupPWDInput;
    public GameObject signupUI;

    [Header("# warning")]
    public TextMeshProUGUI warningTxt;
    public GameObject warningUI;

    public void CloseUi(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void WarningClose()
    {
        warningUI.SetActive(false);
    }

    public void SignBtn()
    {
        signupUI.SetActive(true);
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
                //�̹� �����ϴ� ���̵��Դϴ�.
                Debug.Log("�̹� �����ϴ� ���̵��Դϴ�.");
                warningTxt.text = "�̹� �����ϴ� ���̵��Դϴ�.";
                warningUI.SetActive(true);

                Invoke("WarningClose", 3f);
            }
            else
            {
                if (pwd.ToString().Length == 4)
                {
                    MySQLConnection.Insert("users", $"'{playerName}', {pwd}");
                    //ȸ������ 
                    Debug.Log("ȸ������ ����");
                    warningTxt.text = "ȸ������ ����";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
                else
                {
                    //��й�ȣ�� 4�ڸ��Դϴ�. 4�϶��� �Է°���
                    Debug.Log("��й�ȣ�� 4�ڸ��Դϴ�.");
                    warningTxt.text = "��й�ȣ�� 4�ڸ��Դϴ�.";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
            }
        }
        else if (string.IsNullOrEmpty(signupIdInput.text) || (!string.IsNullOrEmpty(signupPWDInput.text)))
        {
            //���̵� ������� ��
            Debug.Log("ID�� �Է����ּ���.");
            warningTxt.text = "���̵� �Է����ּ���";
            warningUI.SetActive(true);

            Invoke("WarningClose", 3f);
        }
        else if (string.IsNullOrEmpty(signupPWDInput.text))
        {
            //��й�ȣ�� ������� ��
            Debug.Log("��й�ȣ�� �Է����ּ���");
            warningTxt.text = "��й�ȣ�� �Է����ּ���";
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
                }
                else
                {
                    //OpenUI("alert");
                    //alertText.text = "��й�ȣ�� �߸��Ǿ����ϴ�";
                    Debug.Log("��й�ȣ�� �Է����ּ���");
                    warningTxt.text = "��й�ȣ�� �߸��Ǿ����ϴ�";
                    warningUI.SetActive(true);

                    Invoke("WarningClose", 3f);
                }
            }
            else
            {
                // ������������ ���̵��Դϴ�.
                Debug.Log("�������� ���� ���̵��Դϴ�");
                warningTxt.text = "�������� ���� ���̵��Դϴ�";
                warningUI.SetActive(true);

                Invoke("WarningClose", 3f);
            }
        }
        else if (string.IsNullOrEmpty(loginIdInput.text) || (!string.IsNullOrEmpty(loginPWDInput.text)))
        {
            //���̵� ������� ��
        }
        else if (string.IsNullOrEmpty(loginPWDInput.text))
        {
            //��й�ȣ�� ������� ��
        }
    }
    void Awake()
    {
        mainPage.SetActive(true);
        setPopup.SetActive(false);
        warningUI.SetActive(false);
        //loginUI.SetActive(true);
        //signupUI.SetActive(true);

        if (AudioPlayBGM.instance != null)
        {
            AudioPlayBGM.instance.ChangeClip(AudioPlayBGM.instance.Game);
        }
        else
        {
            Debug.LogError("AudioPlayBGM �ν��Ͻ��� �����ϴ�.");
        }
    }

    private void Update()
    {
        if(signupUI.activeSelf == true)
        {
            loginSignBtn.SetActive(false);
        }
        else
        {
            loginSignBtn.SetActive(true);
        }
    }

    public void Clicked_start() //�����ϱ�
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -78);
        PlayerPrefs.SetFloat("SaveY", 11);
        PlayerPrefs.SetInt("SaveStage", 0);
    }
    public void set() //����â ����
    {
        setPopup.SetActive(true);
    }
    public void quit() //�����ϱ�
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
