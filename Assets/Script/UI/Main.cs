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
