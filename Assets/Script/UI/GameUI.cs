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
    public GameObject quizPopup;
    public TMP_InputField answerField;
    public Image quizImg;
    [SerializeField] private Image quizTimer;
    private float quizTimer_max = 20f;
    private float quizTimer_now;
    [SerializeField] private GameObject answerFalseIcon;
    public GameObject answerTrueIcon;
    [SerializeField] private List<int> QList;
    [SerializeField] private List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField] private List<string> AList;
    private int rand;
    private int max = 2;

    [Header("# over")]
    public GameObject overPopup;

    [Header("# clear")]
    [SerializeField] private GameObject clearPopup;
    public TextMeshProUGUI clearTimeTxt;

    [Header("Dash")]
    [SerializeField] private Image DashCoolTime;
    private float coolTime;
    private float coolTime_max;

    [Header("# A attack")]
    [SerializeField] private Image ACoolTimeImg;
    private float ACoolTime;
    private float ACoolTime_max;

    [Header("# heart")]
    [SerializeField] private Image hpBar;
    public float hp_max;
    public float hp_now;

    [Header("# weapon")]
    [SerializeField] private Image LWeaponImg;
    [SerializeField] private Image SWeaponImg;
    [SerializeField] private Image NowWeaponImg;

    [Header("# description")]
    public GameObject descrip;

    //�̱���
    public static GameUI UIData { get; private set; }

    void Awake()
    {
        UIData = this;

        Time.timeScale = 1;
        stopPopup.SetActive(false);
        quizPopup.SetActive(false);
        overPopup.SetActive(false);
        clearPopup.SetActive(false);
        setPopup.SetActive(false);

        quizTimer_now = quizTimer_max;
        hp_max = PlayerController.playerData.charac_MaxHP;
        coolTime_max = PlayerController.playerData.dashCooldown;
        ACoolTime_max = PlayerController.playerData.AttackCoolTime_max;

        QList = new List<int>() {0,1};
        AList = new List<string>() {"������", "ĩ��"};
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

        if (PlayerController.playerData.isDash == true) // ���� ��Ÿ�� ����Ʈ
        {
            if(coolTime <= 0 || coolTime == coolTime_max)
            {
                coolTime = coolTime_max;
                StartCoroutine(CoolTimeFunc());
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ACoolTime_max = PlayerController.playerData.AttackCoolTime_max;

            if (ACoolTime <= 0 || ACoolTime == ACoolTime_max)
            {
                ACoolTime = ACoolTime_max;
                StartCoroutine(ACoolTimeFunc());
            }
        }

        time(); // Ÿ��ǥ��
        hp(); // HP �ӽ�Ű
        quiz_on(); // ����������
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

    public void x() // ����â ���� ��ư && ����ĭ ����
    {
        stopBg.SetActive(true);
        setPopup.SetActive(false);

        if(descrip.activeSelf == true)
        {
            descrip.SetActive(false);
        }
    }

    public void NewGame() // ������
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void goMain() // ���ΰ���
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
    IEnumerator ACoolTimeFunc() // ���� ��Ÿ�� ����Ʈ
    {

        while (ACoolTime > 0.0f)
        {
            ACoolTime -= Time.deltaTime;
            ACoolTimeImg.fillAmount = ACoolTime / ACoolTime_max;

            yield return new WaitForFixedUpdate();
        }
    }

    public void hp()
    {
        if (quizPopup.activeSelf == false)
        {
            if(PlayerController.playerData.charac_PreHP <= 0)
            {
                overPopup.SetActive(true);
                Time.timeScale = 0;
            }
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

    void QAIcon()
    {
        if (answerFalseIcon.activeSelf == true)
        {
            answerFalseIcon.SetActive(false);
            answerField.text = "";
        }
        else
        {
            answerTrueIcon.SetActive(false);
            quizPopup.SetActive(false);
            quizTimer_now = 20f;
        } 
    }

    public void quiz() //�ӽ� ����â
    {
        if (quizPopup.activeSelf == false) // �ӽ��������Ű
        {
            answerField.text = "";
            answerFalseIcon.SetActive(false);
            answerTrueIcon.SetActive(false);
            quizPopup.SetActive(true);
            StartCoroutine(quizTimerFunc());
        }
    }

    void quiz_on()
    {
        if(quizPopup.activeSelf == true) {

            rand = Random.Range(0, max);
            quizImg.sprite = SpriteList[rand];

            Debug.Log(AList[rand]);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (answerField.text == "")
                    {
                        answerField.ActivateInputField();
                    }
                    else
                    {
                        if (answerField.text == AList[rand])
                        {
                            answerTrueIcon.SetActive(true);
                            StopCoroutine(quizTimerFunc());

                            Invoke("QAIcon", 2f);
                        }
                        else
                        {
                            answerFalseIcon.SetActive(true);

                            Invoke("QAIcon", 2f);
                        }
                    }
                }
            

            if (quizTimer_now <= 0.0f)
            {
                quizPopup.SetActive(false);
                PlayerController.playerData.charac_PreHP -= 10.0f;
                quizTimer_now = 20f;
            }
        }
        //QList.RemoveAt(rand);
        //AList.RemoveAt(rand);
        //SpriteList.RemoveAt(rand);
        //max--;
    }

    public void Clear()
    {
        if(quizPopup.activeSelf == false)
        {
            clearPopup.SetActive(true);
            Time.timeScale = 0;
            clearTimeTxt.text = TimeTxt.text;
        }
    }

    void changeweapon()
    {

    }
}
