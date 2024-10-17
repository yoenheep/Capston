using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr { get; private set; }

    public GameObject Player;

    public GameObject[] drop_Item;
    public GameObject item_tr;

    public int nowStage; // [map1 =0 / map2 =1 / map3 = 2 / map4 = 3 / trap1 = 4 / trap2 = 5 / middle = 6 / angel = 7 / last = 8]
    public GameObject[] mainPoints;
    public GameObject mainPortal;
    public int mainStageIndex;
    public GameObject[] subPoints;
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
        int stageNum = PlayerPrefs.GetInt("SaveStage");

        nowStage = stageNum;
        mainStageIndex = stageNum;

        MainPlayerMove();

        if (stageNum == 0)
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
        if (nowStage == 0) //��1
        {
            subPortal.transform.position = new Vector3(-19, -5,0);
            mainPortal.transform.position = new Vector3(38, -9, 0);
        }
        else if (nowStage == 1) //��2
        {
            subPortal.transform.position = new Vector3(176, -4, 0);
            mainPortal.transform.position = new Vector3(300, -15, 0);
        }
        else if (nowStage == 2) //��3
        {
            subPortal.transform.position = new Vector3(438, -31.28f, 0);
            mainPortal.transform.position = new Vector3(547, -16.27f, 0);
        }
        else if (nowStage == 3) //��4
        {
            subPortal.transform.position = new Vector3(706, -21.27f, 0);
            mainPortal.transform.position = new Vector3(757, 8.72f, 0);
        }
        else if (nowStage == 4 || nowStage == 5 || nowStage == 6) //trap, mid, last
        {
            subPortal.SetActive(false);
            mainPortal.SetActive(false);
        }
    }

    public void Drop_Item(GameObject obj)
    {
        //int i = Random_Percentage();
        int i = 1;
        Transform parentTransform = item_tr.transform;
        Vector2 loc = obj.GetComponent<Rigidbody2D>().position;
        loc = new Vector2(loc.x, loc.y - (obj.transform.localScale.y / 2f) + 1.8f);
        //Debug.Log(tr);

        if(parentTransform == null)
        {
            Debug.Log("�θ� ã�� ����");
            //return;
        }

        if(i != 666)
        {
            GameObject createdPrefab = Instantiate(drop_Item[i], loc, Quaternion.identity);
            createdPrefab.transform.SetParent(parentTransform, false);

            //�θ� ��ǥ�� ����
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
            Debug.Log("��");
            return 666;
        }
    }

    public void MainNextStage() //main map �̵�
    {
        mainStageIndex++;

        MainPlayerMove();
    }
    public void SubNextStage() // Ʈ��, �̴Ϻ����� �̵�
    {
        SubPlayerMove();
    }

    public void SubOutStage() // Ʈ��, �̴Ϻ����� ������
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
            Player.transform.position = mainPoints[0].transform.position;

            PlayerPrefs.SetFloat("SaveX", mainPoints[0].transform.position.x);
            PlayerPrefs.SetFloat("SaveY", mainPoints[0].transform.position.y);
            PlayerPrefs.SetInt("SaveStage", 0);
        }
        else if (mainStageIndex == 1) // map2
        {
            Player.transform.position = mainPoints[1].transform.position;

            PlayerPrefs.SetFloat("SaveX", mainPoints[1].transform.position.x);
            PlayerPrefs.SetFloat("SaveY", mainPoints[1].transform.position.y);
            PlayerPrefs.SetInt("SaveStage", 1);

            nowStage = 1;
        }
        else if (mainStageIndex == 2) // map3
        {
            Player.transform.position = mainPoints[2].transform.position;

            PlayerPrefs.SetFloat("SaveX", mainPoints[2].transform.position.x);
            PlayerPrefs.SetFloat("SaveY", mainPoints[2].transform.position.y);
            PlayerPrefs.SetInt("SaveStage", 2);

            nowStage = 2;
        }
        else if(mainStageIndex == 3) // map4
        {
            Player.transform.position = mainPoints[3].transform.position;

            PlayerPrefs.SetFloat("SaveX", mainPoints[3].transform.position.x);
            PlayerPrefs.SetFloat("SaveY", mainPoints[3].transform.position.y);
            PlayerPrefs.SetInt("SaveStage", 3);
            
            nowStage = 3;
        }
        else if( mainStageIndex == 4) // lastboss
        {
            Player.transform.position = mainPoints[4].transform.position;
            nowStage = 8;
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
        if (subStageIndex == 0) //trap1
        {
            Player.transform.position = subPoints[0].transform.position;
            nowStage = 4;
        }
        else if(subStageIndex == 1) //trap2
        {
            Player.transform.position = subPoints[1].transform.position;
            nowStage = 5;
        }
        else if(subStageIndex == 2) // middle
        {
            Player.transform.position = subPoints[2].transform.position;
            nowStage = 6;
        }
        else if(subStageIndex == 3) // angel
        {
            Player.transform.position = subPoints[3].transform.position;
            nowStage = 7;
        }
    }
}
