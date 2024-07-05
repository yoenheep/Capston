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
        nowStage = 0;

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
        if (nowStage == 0) //맵1
        {
            subPortal.transform.position = new Vector3(-19, -5,0);
            mainPortal.transform.position = new Vector3(38, -9, 0);

            PlayerPrefs.SetFloat("SaveX", -78); //임시 저장
            PlayerPrefs.SetFloat("SaveY", 11);
            PlayerPrefs.SetInt("SaveStage", 0);
        }
        else if (nowStage == 1) //맵2
        {
            subPortal.transform.position = new Vector3(176, -4, 0);
            mainPortal.transform.position = new Vector3(300, -15, 0);

            PlayerPrefs.SetFloat("SaveX", 169);
            PlayerPrefs.SetFloat("SaveY", -10);
            PlayerPrefs.SetInt("SaveStage", 1);
        }
        else if (nowStage == 2) //맵3
        {
            subPortal.transform.position = new Vector3(438, -31.28f, 0);
            mainPortal.transform.position = new Vector3(547, -16.27f, 0);

            PlayerPrefs.SetFloat("SaveX", 398);
            PlayerPrefs.SetFloat("SaveY", -29);
            PlayerPrefs.SetInt("SaveStage", 2);
        }
        else if (nowStage == 3) //맵4
        {
            subPortal.transform.position = new Vector3(706, -21.27f, 0);
            mainPortal.transform.position = new Vector3(757, 8.72f, 0);

            PlayerPrefs.SetFloat("SaveX", 576);
            PlayerPrefs.SetFloat("SaveY", -20);
            PlayerPrefs.SetInt("SaveStage", 3);
        }
        else if (nowStage == 4 || nowStage == 5 || nowStage == 6) //trap, mid, last
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
        mainStageIndex++;

        MainPlayerMove();
    }
    public void SubNextStage() // 트랩, 미니보스방 이동
    {
        SubPlayerMove();
    }

    public void SubOutStage() // 트랩, 미니보스방 나가기
    {
        SubOutPlayerMove();

        subStageIndex++;
    }

    void MainPlayerMove()
    {
        subPortal.SetActive(true);
        mainPortal.SetActive(false);

        if (mainStageIndex == 0) // map1
        {
            Player.transform.position = new Vector3(-78, 11, 0);

            
        }
        else if (mainStageIndex == 1) // map2
        {
            Player.transform.position = new Vector3(169, -10, 0);
            
            
            nowStage = 1;
        }
        else if (mainStageIndex == 2) // map3
        {
            Player.transform.position = new Vector3(398, -29, 0);
            
            
            nowStage = 2;
        }
        else if(mainStageIndex == 3) // map4
        {
            Player.transform.position = new Vector3(576, -20, 0);
            
            
            nowStage = 3;
        }
        else if( mainStageIndex == 4) // lastboss
        {
            Player.transform.position = new Vector3(870, -26, 0);
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
            nowStage = 3;
        }
    }

    void SubPlayerMove()
    {
        if (subStageIndex == 0) //trap
        {
            Player.transform.position = new Vector3(115, -26, 0);
            nowStage = 4;
        }
        else if(subStageIndex == 1 || subStageIndex == 2 || subStageIndex == 3) //midboss
        {
            Player.transform.position = new Vector3(333, -2, 0);
            nowStage = 5;
        }
    }
}
