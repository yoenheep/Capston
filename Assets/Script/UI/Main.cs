using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [Header("# gameObj")]
    [SerializeField] private GameObject setPopup;
    [SerializeField] private GameObject mainPage;

    // Start is called before the first frame update
    void Awake()
    {
        mainPage.SetActive(true);
        setPopup.SetActive(false);
    }

    public void Clicked_start() //시작하기
    {
        //저장한 곳으로 이동하는
        SceneManager.LoadScene("Game");
    }
    public void set() //설정창 열기
    {
        setPopup.SetActive(true);
    }
    public void re() //새로시작하기
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetFloat("SaveX", -7.5f);
        PlayerPrefs.SetFloat("SaveY", -2);
        PlayerPrefs.SetInt("SaveStage", 0);
    }
    public void quit() //종료하기
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
