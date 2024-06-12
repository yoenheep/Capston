using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    public GameObject quizPopup;
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
    private int max = 2;

    private void Awake()
    {
        quizTimer_now = quizTimer_max;

        AList = new List<string>() { "∞≥∆˜µø", "ƒ©º÷" };
    }

    private void Update()
    {
        quizEnter();
    }

    IEnumerator quizTimerFunc() // ƒ˚¡Ó ƒ≈∏¿” ¿Ã∆—∆Æ
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

    public void quiz() //¿”Ω√ ƒ˚¡Ó√¢
    {
        if (quizPopup.activeSelf == false) // ¿”Ω√ƒ˚¡Ó∏ÛΩ∫≈Õ≈∞
        {
            answerField.text = "";
            answerFalseIcon.SetActive(false);
            answerTrueIcon.SetActive(false);
            quizPopup.SetActive(true);
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
}
