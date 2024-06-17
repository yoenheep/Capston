using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    GameObject quizPopup;
    public TMP_InputField answerField;
    public Image quizImg;
    [SerializeField] private Image quizTimer;
    private float quizTimer_max = 20f;
    private float quizTimer_now;
    [SerializeField] private GameObject answerFalseIcon;
    public GameObject answerTrueIcon;
    [SerializeField] private List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField] private List<string> AList;
    private int rand;
    private int max = 6;

    //몬스터 데미지 처리를 위한 변수
    private Monsters mon;

    private void Awake()
    {
        quizTimer_now = quizTimer_max;
        quizPopup = GameUI.UIData.quizPopup;

        AList = new List<string>() { "개포동", "칫솔","다리꼬지마","주토피아","메이플스토리","멕시코시티" };
    }

    private void Update()
    {
        quizEnter();
    }

    IEnumerator quizTimerFunc() // 퀴즈 쿨타임 이팩트
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

            Debug.Log(rand);
            AList.RemoveAt(rand);
            SpriteList.RemoveAt(rand);
            max--;
        }
    }

    public void quiz(GameObject mon) //임시 퀴즈창
    {
        if (quizPopup.activeSelf == false) // 임시퀴즈몬스터키
        {
            answerField.text = "";
            answerFalseIcon.SetActive(false);
            answerTrueIcon.SetActive(false);
            quizPopup.SetActive(true);

            this.mon = mon.GetComponent<Monsters>();

            quiz_on();
            StartCoroutine(quizTimerFunc());
        }
    }

    void quiz_on()
    {
        rand = Random.Range(0, max);
        quizImg.sprite = SpriteList[rand];

        Debug.Log(AList[rand]);   
    }

    void quizEnter()
    {
        if(quizPopup.activeSelf == true)
        {
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

                        if(this.mon != null)
                        {
                            this.mon.Quiz_Mon_Die();
                        } 
                    }
                    else
                    {
                        answerFalseIcon.SetActive(true);

                        Invoke("QAIcon", 2f);
                    }
                }
            }
        }

        if (quizTimer_now <= 0.0f)
        {
            quizPopup.SetActive(false);
            PlayerController.playerData.Hp(10f, this.mon.gameObject.transform.position);
            quizTimer_now = 20f;
        }
    }
}
