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
                //�̹� �����ϴ� ���̵��Դϴ�.
                Debug.Log("�̹� �����ϴ� ���̵��Դϴ�.");
            }
            else
            {
                if (pwd.ToString().Length == 4)
                {
                    MySQLConnection.Insert("users", $"'{playerName}', {pwd}");
                    //ȸ������ 
                    Debug.Log("ȸ������ ����");
                }
                else
                {
                    //��й�ȣ�� 4�ڸ��Դϴ�. 4�϶��� �Է°���
                    Debug.Log("��й�ȣ�� 4�ڸ��Դϴ�.");
                }
            }
        }
        else if (string.IsNullOrEmpty(signupIdInput.text) || (!string.IsNullOrEmpty(signupPWDInput.text)))
        {
            //���̵� ������� ��
            Debug.Log("ID�� �Է����ּ���.");
        }
        else if (string.IsNullOrEmpty(signupPWDInput.text))
        {
            //��й�ȣ�� ������� ��
            Debug.Log("��й�ȣ�� �Է����ּ���");
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
                    //alertText.text = "��й�ȣ�� �߸��Ǿ����ϴ�";
                }
            }
            else
            {
                // ������������ ���̵��Դϴ�.
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
    }

    public void Clicked_start() //�����ϱ�
    {
        //������ ������ �̵��ϴ�
        SceneManager.LoadScene("Game");
    }
    public void set() //����â ����
    {
        setPopup.SetActive(true);
    }
    public void re() //���ν����ϱ�
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -78);
        PlayerPrefs.SetFloat("SaveY", 11);
        PlayerPrefs.SetInt("SaveStage", 0);
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
