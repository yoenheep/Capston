using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr { get; private set; }

    public GameObject Player;

    public GameObject[] drop_Item;
    public GameObject item_tr;

    public int nowStage; // [map1 =0 / trap =4 / map2 =1 / middle =5 / map3 = 2 / map4 = 3 / last = 6]
    public GameObject[] mainStages;
    public GameObject mainPortal;
    public int mainStageIndex;
    public GameObject[] subStages;
    public GameObject subPortal;
    public int subStageIndex;

    private void Awake()
    {
        gameMgr = this;

        GameStart();
    }

    void GameStart()
    {
        float x = PlayerPrefs.GetFloat("SaveX");
        float y = PlayerPrefs.GetFloat("SaveY");
        int stageNum = PlayerPrefs.GetInt("SaveStage");

        Player.transform.position = new Vector3(x, y, 0);

        nowStage = stageNum;
        mainStageIndex = stageNum;

        mainStages[nowStage].SetActive(true);

        if(stageNum == 0)
        {
            subStageIndex = 0;
        }
        else
        {
            subStageIndex = 1;
        }

        subPortal.SetActive(true);
        mainPortal.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.gameMgr.nowStage == 0) //맵1
        {
            subPortal.transform.position = new Vector3(52.5f, -17.5f,0);
            mainPortal.transform.position = new Vector3(109.5f, -21.5f, 0);
        }
        else if (GameManager.gameMgr.nowStage == 1) //맵2
        {
            subPortal.transform.position = new Vector3(-2.5f, -16.5f, 0);
            mainPortal.transform.position = new Vector3(121.5f, -27.5f, 0);
        }
        else if (GameManager.gameMgr.nowStage == 2) //맵3
        {
            subPortal.transform.position = new Vector3(6.5f, -30.5f, 0);
            mainPortal.transform.position = new Vector3(115.5f, -15.5f, 0);
        }
        else if (GameManager.gameMgr.nowStage == 3) //맵4
        {
            subPortal.transform.position = new Vector3(94.5f, -20.5f, 0);
            mainPortal.transform.position = new Vector3(145.5f, 9.5f, 0);
        }
        else if (GameManager.gameMgr.nowStage == 4) //trap
        {
            subPortal.SetActive(false);
            mainPortal.SetActive(false);
        }
        else if (GameManager.gameMgr.nowStage == 5) //miniboss
        {
            subPortal.SetActive(false);
            mainPortal.SetActive(false);
        }
        else if (GameManager.gameMgr.nowStage == 6) //라스트보스
        {
            subPortal.SetActive(false);
            mainPortal.SetActive(false);
        }
    }

    public void Drop_Item(GameObject obj)
    {
        Transform parentTransform = item_tr.transform;
        Vector2 loc = obj.GetComponent<Rigidbody2D>().position;
        //Debug.Log(tr);

        if(parentTransform == null)
        {
            Debug.Log("부모 찾지 못함");
            //return;
        }

        int i = Random_Percentage();

        if(i != 666)
        {
            GameObject createdPrefab = Instantiate(drop_Item[i], loc, Quaternion.identity);
            createdPrefab.transform.SetParent(parentTransform, false);

            //부모 좌표로 변경
            createdPrefab.transform.localPosition = parentTransform.InverseTransformPoint(loc);
        }
    }

    public int Random_Percentage()
    {
        int num = Random.Range(0, 10);

        if(num % 2 == 0)
        {
            num = Random.Range(0, drop_Item.Length);
            return num;
        } else
        {
            Debug.Log("꽝");
            return 666;
        }
    }

    public void MainNextStage() //main map 이동
    {
        mainStages[mainStageIndex].SetActive(false);
        mainStageIndex++;
        mainStages[mainStageIndex].SetActive(true);

        MainPlayerMove();
    }
    public void SubNextStage() // 트랩, 미니보스방 이동
    {
        subStages[subStageIndex].SetActive(true);
        mainStages[mainStageIndex].SetActive(false);

        subPortal.SetActive(false);
        SubPlayerMove();
    }

    public void SubOutStage() // 트랩, 미니보스방 나가기
    {
        SubOutPlayerMove();
        subStages[subStageIndex].SetActive(false);
        subStageIndex++;
        mainStages[mainStageIndex].SetActive(true);
    }

    void MainPlayerMove()
    {
        subPortal.SetActive(true);
        mainPortal.SetActive(false);

        if (mainStageIndex == 0)
        {
            Player.transform.position = new Vector3(-7.5f, -2, 0);

            PlayerPrefs.SetFloat("SaveXFirst", -7.5f); //임시 저장
            PlayerPrefs.SetFloat("SaveYFirst", -2);
            PlayerPrefs.SetInt("SaveStage", 0);
        }
        else if (mainStageIndex == 1)
        {
            Player.transform.position = new Vector3(-11, -22, 0);
            
            PlayerPrefs.SetFloat("SaveX", -11);
            PlayerPrefs.SetFloat("SaveY", -22);
            PlayerPrefs.SetInt("SaveStage", 1);
            nowStage = 1;
        }
        else if (mainStageIndex == 2)
        {
            Player.transform.position = new Vector3(-34, -28, 0);
            
            PlayerPrefs.SetFloat("SaveX", -34);
            PlayerPrefs.SetFloat("SaveY", -28);
            PlayerPrefs.SetInt("SaveStage", 2);
            nowStage = 2;
        }
        else if(mainStageIndex == 3)
        {
            Player.transform.position = new Vector3(-36, -19, 0);
            
            PlayerPrefs.SetFloat("SaveX", -36);
            PlayerPrefs.SetFloat("SaveY", -19);
            PlayerPrefs.SetInt("SaveStage", 3);
            nowStage = 3;
        }
        else if( mainStageIndex == 4)
        {
            Player.transform.position = new Vector3(-15, -15, 0);
            nowStage = 6;
        }
    }

    void SubOutPlayerMove()
    {
        mainPortal.SetActive(true);
        subPortal.SetActive(false);

        Player.transform.position = subPortal.transform.position;

        if(mainStageIndex == 0)
        { 
            nowStage = 0;
        }
        else if(mainStageIndex == 1)
        {           
            nowStage = 1;
        }
        else if (mainStageIndex == 2)
        {          
            nowStage = 2;
        }
        else if (mainStageIndex == 3)
        {
            subPortal.SetActive(false);
            nowStage = 3;
        }
    }

    void SubPlayerMove()
    {
        if (subStageIndex == 0)
        {
            //Player.transform.position = new Vector3(19, -25, 0); 트랩
            Player.transform.position = new Vector3(-12, -14, 0); //미니보스
            nowStage = 4;
        }
        else if(subStageIndex == 1 || subStageIndex == 2 || subStageIndex == 3)
        {
            Player.transform.position = new Vector3(-12, -14, 0);
            nowStage = 5;
        }
    }
}
