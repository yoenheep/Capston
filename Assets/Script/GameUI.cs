using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameUI : MonoBehaviour
{
    [Header("# stop")]
    [SerializeField] private GameObject stopPopup;
    [SerializeField] private GameObject stopBg;
    [SerializeField] private GameObject setPopup;

    [Header("# quiz")]
    [SerializeField] private GameObject quizPopup;
    public InputField answerField;
    public Image quizImg;

    [Header("# over")]
    [SerializeField] private GameObject overPopup;

    [Header("# clear")]
    [SerializeField] private GameObject clearPopup;
    public TextMeshProUGUI clearTimeTxt;

    [Header("Dash")]
    [SerializeField] private Image DashCoolTime;
    private float coolTime = 8f;
    private float coolTime_max = 8f;

    [Header("# heart")]
    [SerializeField] private Image hpBar;
    private float hp_max = 100;
    private float hp_now;
    private bool isDamaged = false;

    //[Header("# weapon")]

    private void Awake()
    {
        Time.timeScale = 1;
        stopPopup.SetActive(false);
        quizPopup.SetActive(false);
        overPopup.SetActive(false);
        clearPopup.SetActive(false);
        setPopup.SetActive(false);

        hp_now = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // �Ͻ�����
        {
            if (stopPopup.activeSelf == false)
            {
                stopPopup.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Continue();
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) // ���� ��Ÿ�� ����Ʈ
        {
            if(coolTime < 0|| coolTime == 8f)
            {
                coolTime = coolTime_max;
                StartCoroutine(CoolTimeFunc());
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            clearPopup.SetActive(true);
            Time.timeScale = 0;
        }

        hp(); // HP �ӽ�Ű
        hpBar.fillAmount = hp_now / hp_max; // ĳ���� hpbar
    }

    public void Continue() // ����ϱ�
    {
        if (stopPopup.activeSelf == true)
        {
            stopPopup.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Setting() // ����
    {
        stopBg.SetActive(false);
        setPopup.SetActive(true);
    }

    public void x() // ����â ���� ��ư
    {
        stopBg.SetActive(true);
        setPopup.SetActive(false);
    }

    public void NewGame() // ������
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void goMain() //a���ΰ���
    {
        SceneManager.LoadScene("Main");
    }

    IEnumerator CoolTimeFunc() // ���� ��Ÿ�� ����Ʈ
    {
        while (coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;
            DashCoolTime.fillAmount = coolTime / coolTime_max;

            yield return new WaitForFixedUpdate();
        }
    }

    void CanDamaged()
    {
        isDamaged = false;
    }

    public void hp()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hp_now -= 10;
            isDamaged = true;

            if(hp_now <= 0)
            {
                overPopup.SetActive(true);
                Time.timeScale = 0;
            }

            Invoke("CanDamaged", 0.4f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hp_now += 10;
        }
    }
}
