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
    [SerializeField] private GameObject answerTrueIcon;

    [Header("# over")]
    public GameObject overPopup;

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

    [Header("# weapon")]
    [SerializeField] private Image AWeaponImg;
    [SerializeField] private Image SWeaponImg;

    [Header("# description")]
    [SerializeField] private Ray2D ray;

    //싱글톤
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && quizPopup.activeSelf == false) // 일시정지
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

        if (Input.GetKeyDown(KeyCode.D)) // 데쉬 쿨타임 이팩트
        {
            if(coolTime < 0 || coolTime == coolTime_max)
            {
                coolTime = coolTime_max;
                StartCoroutine(CoolTimeFunc());
            }
        }

        time(); // 타임표시
        hp(); // HP 임시키
        quiz_on(); // 퀴즈켜지면
        hp_now = PlayerController.playerData.charac_PreHP;
        hpBar.fillAmount = hp_now / hp_max; // 캐릭터 hpbar
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
    public void Continue() // 계속하기
    {
        if (stopPopup.activeSelf == true)
        {
            stopPopup.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Setting() // 설정
    {
        stopBg.SetActive(false);
        setPopup.SetActive(true);
    }

    public void x() // 설정창 끄기 버튼
    {
        stopBg.SetActive(true);
        setPopup.SetActive(false);
    }

    public void NewGame() // 새게임
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void goMain() //a메인가기
    {
        SceneManager.LoadScene("Main");
    }

    IEnumerator CoolTimeFunc() // 데쉬 쿨타임 이팩트
    {
        while (coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;
            DashCoolTime.fillAmount = coolTime / coolTime_max;

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

    IEnumerator quizTimerFunc() // 데쉬 쿨타임 이팩트
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

    public void quiz() //임시 퀴즈창
    {
        if (quizPopup.activeSelf == false) // 임시퀴즈몬스터키
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
        if(quizPopup.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("넌가");
                if (answerField.text == "")
                {
                    answerField.ActivateInputField();
                }
                else
                {
                    if (answerField.text == "넌 못지나간다")
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
                Debug.Log("ㅋ");
                quizPopup.SetActive(false);
                PlayerController.playerData.charac_PreHP -= 10.0f;
                quizTimer_now = 20f;
            }
        }
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
}
