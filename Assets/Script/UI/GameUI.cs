using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Net.Http.Headers;
using System.Xml;

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
    public QuizUI QuizUI;
    public GameObject quizPopup;

    [Header("# over")]
    public GameObject overPopup;
    public Sprite deathSprite;

    [Header("# clear")]
    public GameObject clearPopup;
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
    [SerializeField] private Image firstWeaponImg;
    [SerializeField] private Image secondWeaponImg;
    [SerializeField] private Image NowWeaponImg;

    [SerializeField] private List<Sprite> weaponsSprite = new List<Sprite>();

    [Header("# description")]
    public GameObject descrip;
    public GameObject ItemZTxtObj;
    [SerializeField] private Description Description;

    public void SaveRanking()
    {
        string playerName = UserData.instance.userName;
        XmlNodeList user = MySQLConnection.Select("ranking", $"WHERE ID = '{playerName}'");

        if (user != null)
        {

            MySQLConnection.UpdateRanking("ranking", "time", timeSecs, $"ID = '{playerName}'");
            Debug.Log("Update UserRanking");
        }
        else
        {
            string query = $"{playerName},{timeSecs.ToString()}";
            MySQLConnection.Insert("ranking", query);
            // INSERT INTO ranking VALUES playerName, timeSecs 
            Debug.Log("save UserRanking");
        }
    }

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

        hp_max = PlayerController.playerData.charac_MaxHP;
        coolTime_max = PlayerController.playerData.dashCooldown;
        ACoolTime_max = PlayerController.playerData.AttackCoolTime_max;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && quizPopup.activeSelf == false) // 일시정지
        {
            esc();
        }

        if (PlayerController.playerData.isDash == true) // 데쉬 쿨타임 이팩트
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

        changeWeapon();
        time(); // 타임표시
        hp(); // HP 임시키
        hp_now = PlayerController.playerData.charac_PreHP;
        hpBar.fillAmount = hp_now / hp_max; // 캐릭터 hpbar

        if (Input.GetKeyDown(KeyCode.P))
        {
            Clear();
        }
               
            
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

    public void esc() // 설정창 끄기 버튼 && 설명칸 끄기
    {
        if (descrip.activeSelf == true)
        {
            descrip.SetActive(false);
            Description.anim.SetTrigger("Des_on");
        }
        else if (stopPopup.activeSelf == false)
        {
            stopPopup.SetActive(true);
            ItemZTxtObj.SetActive(false);
            Time.timeScale = 0;
        }
        else if (setPopup.activeSelf == true)
        {
            stopBg.SetActive(true);
            setPopup.SetActive(false);
        }
        else if (stopPopup.activeSelf == true)
        {
            stopPopup.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void NewGame() // 새게임
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -7.5f);
        PlayerPrefs.SetFloat("SaveY", -2);
        PlayerPrefs.SetInt("SaveStage", 0);
    }

    public void goMain() // 메인가기
    {
        SceneManager.LoadScene("Main");
    }

    public void ReGame()
    {
        SceneManager.LoadScene("Game");
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
    IEnumerator ACoolTimeFunc() // 공격 쿨타임 이팩트
    {

        while (ACoolTime > 0.0f)
        {
            ACoolTime -= Time.deltaTime;
            ACoolTimeImg.fillAmount = ACoolTime / ACoolTime_max;

            yield return new WaitForFixedUpdate();
        }
    }

    void hp_Invoke()
    {
        PlayerController.playerData.isDead = true;
    }

    public void hp()
    {
        if (quizPopup.activeSelf == false)
        {
            if (PlayerController.playerData.charac_PreHP <= 0)
            {
                overPopup.SetActive(true);
                if (AudioPlayBGM.instance.bgmAudio.clip != AudioPlayBGM.instance.gameOver)
                {
                    AudioPlayBGM.instance.bgmAudio.clip = AudioPlayBGM.instance.gameOver;

                    // 오디오가 재생 중이지 않으면 재생
                    if (!AudioPlayBGM.instance.bgmAudio.isPlaying)
                    {
                        AudioPlayBGM.instance.bgmAudio.Play();
                    }
                }
                Invoke("hp_Invoke", 0.5f);
                if (PlayerController.playerData.isDead)
                {
                    Time.timeScale = 0;

                    if (GameManager.gameMgr.nowStage == 4)
                    {
                        GameManager.gameMgr.nowStage = 0;
                    }
                    else if (GameManager.gameMgr.nowStage == 7 || GameManager.gameMgr.nowStage == 8)
                    {
                        GameManager.gameMgr.nowStage = 3;
                    }
                }
                
            }
        }
    }

    public void Clear()
    {
        if(quizPopup.activeSelf == false)
        {
            clearPopup.SetActive(true);
            if (AudioPlayBGM.instance.bgmAudio.clip != AudioPlayBGM.instance.gameClear)
            {
                AudioPlayBGM.instance.bgmAudio.clip = AudioPlayBGM.instance.gameClear;

                // 오디오가 재생 중이지 않으면 재생
                if (!AudioPlayBGM.instance.bgmAudio.isPlaying)
                {
                    AudioPlayBGM.instance.bgmAudio.Play();
                }
            }
            clearTimeTxt.text = TimeTxt.text;
            SaveRanking();
            Time.timeScale = 0;
        }
    }

    public void changeWeapon()
    {
        int firstWeapon = PlayerController.playerData.weapon_item[0];
        int secondWeapon = PlayerController.playerData.weapon_item[1];

        if (secondWeapon == 0)
        {
            secondWeaponImg.enabled = false;
        }
        else 
        {
            secondWeaponImg.enabled= true;
        }

        firstWeaponImg.sprite = weaponsSprite[firstWeapon];
        secondWeaponImg.sprite = weaponsSprite[secondWeapon];

        if(PlayerController.playerData.nowWeapon == 0)
        {
            NowWeaponImg.sprite= weaponsSprite[firstWeapon];
        }
        else if (secondWeapon != 0)
        {
            NowWeaponImg.sprite = weaponsSprite[secondWeapon];
        }
    } 
}
