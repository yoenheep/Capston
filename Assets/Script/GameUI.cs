using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameUI : MonoBehaviour
{
    [Header("# Time")]
    [SerializeField] private TextMeshProUGUI TimeTxt;
    private float timeSecs = 0f;
    private int timeMin = 0;

    [Header("# stop")]
    [SerializeField] private GameObject stopPopup;
    [SerializeField] private GameObject stopBg;
    [SerializeField] private GameObject setPopup;

    [Header("# quiz")]
    [SerializeField] private GameObject quizPopup;
    public TMP_InputField answerField;
    public Image quizImg;
    [SerializeField] private Image quizTimer;
    private float quizTimer_max = 20f;
    private float quizTimer_now;

    [Header("# over")]
    [SerializeField] private GameObject overPopup;

    [Header("# clear")]
    [SerializeField] private GameObject clearPopup;
    public TextMeshProUGUI clearTimeTxt;

    [Header("Dash")]
    [SerializeField] private Image DashCoolTime;
    private float coolTime = 2f;
    private float coolTime_max = 2f;

    [Header("# heart")]
    [SerializeField] private Image hpBar;
    public float hp_max;
    public float hp_now;
    private bool isDamaged = false;

    [Header("# weapon")]
    [SerializeField] private Image AWeaponImg;
    [SerializeField] private Image SWeaponImg;

    private void Awake()
    {
        Time.timeScale = 1;
        stopPopup.SetActive(false);
        quizPopup.SetActive(false);
        overPopup.SetActive(false);
        clearPopup.SetActive(false);
        setPopup.SetActive(false);

        quizTimer_now = quizTimer_max;
        hp_max = PlayerController.playerData.charac_MaxHP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && quizPopup.activeSelf == false) // �Ͻ�����
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
            if(coolTime < 0 || coolTime == coolTime_max)
            {
                coolTime = coolTime_max;
                StartCoroutine(CoolTimeFunc());
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && quizPopup.activeSelf == false) // �ӽ� Ŭ���� Ű
        {
            clearPopup.SetActive(true);
            Time.timeScale = 0;
            clearTimeTxt.text = TimeTxt.text;
        }

        time(); // Ÿ��ǥ��
        quiz(); // quiz �ӽ�Ű
        hp(); // HP �ӽ�Ű
        hp_now = PlayerController.playerData.charac_PreHP;
        hpBar.fillAmount = hp_now / hp_max; // ĳ���� hpbar
    }

    public void time()
    {
        timeSecs += Time.deltaTime;
        if(timeSecs >= 60)
        {
            timeMin += 1;
            timeSecs = 0;
        }

        TimeTxt.text = string.Format("{0:D2}:{1:00}", timeMin, (int)timeSecs);
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
        if (Input.GetKeyDown(KeyCode.Q) && quizPopup.activeSelf == false)
        {
            PlayerController.playerData.charac_PreHP -= 10;
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

    IEnumerator quizTimerFunc() // ���� ��Ÿ�� ����Ʈ
    {
        while (quizTimer_now > 0.0f && quizPopup.activeSelf == true)
        {
            quizTimer_now -= Time.deltaTime;
            quizTimer.fillAmount = quizTimer_now / quizTimer_max;

             yield return new WaitForFixedUpdate();
        }
    }

    public void quiz() //�ӽ� ����â
    {
        if (Input.GetKeyDown(KeyCode.R)) // �ӽ��������Ű
        {
            answerField.text = "";
            quizPopup.SetActive(true);
            StartCoroutine(quizTimerFunc()); 
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (answerField.text == "�����")
            {
                quizPopup.SetActive(false);
                quizTimer_now = 20f;
            }
        }

        if (quizTimer_now <= 0.0f)
        {
            quizPopup.SetActive(false);
            hp_now -= 10.0f;
            quizTimer_now = 20f;
        }
    }
}
