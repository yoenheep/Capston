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
    public GameObject[] subOutPortal;

    private void Awake()
    {
        gameMgr = this;

        nowStage = 0;
        mainStageIndex = 0;
        subStageIndex = 0;

        MainPlayerMove();
    }

    private void Update()
    {
        if (nowStage == 0) //맵1
        {
            subPortal.transform.position = new Vector3(-19, -5, 0);
            mainPortal.transform.position = new Vector3(38, -9, 0);

            subOutPortal[subStageIndex].SetActive(false);

            if (Monster_Spawn_Manager.instance.map1_spawnedMonsters.Count == 0 && subPortal.activeSelf == false)
            {
                mainPortal.SetActive(true);
            }
        }
        else if (nowStage == 1) //맵2
        {
            subPortal.transform.position = new Vector3(176, -4, 0);
            mainPortal.transform.position = new Vector3(300, -15, 0);

            subOutPortal[subStageIndex].SetActive(false);

            if (Monster_Spawn_Manager.instance.map2_spawnedMonsters.Count == 0 && subPortal.activeSelf == false)
            {
                mainPortal.SetActive(true);
            }
        }
        else if (nowStage == 2) //맵3
        {
            subPortal.transform.position = new Vector3(438, -31.28f, 0);
            mainPortal.transform.position = new Vector3(547, -16.27f, 0);

            subOutPortal[subStageIndex].SetActive(false);

            if (Monster_Spawn_Manager.instance.map3_spawnedMonsters.Count == 0 && subPortal.activeSelf == false)
            {
                mainPortal.SetActive(true);
            }
        }
        else if (nowStage == 3) //맵4
        {
            subPortal.transform.position = new Vector3(706, -21.27f, 0);
            mainPortal.transform.position = new Vector3(757, 8.72f, 0);

            subOutPortal[subStageIndex].SetActive(false);

            if (Monster_Spawn_Manager.instance.map4_spawnedMonsters.Count == 0 && subPortal.activeSelf == false)
            {
                mainPortal.SetActive(true);
            }
        }
        else if (nowStage == 4) //trap1
        {
            if (Monster_Spawn_Manager.instance.trap1_spawnedMonsters.Count == 0)
            {
                subOutPortal[subStageIndex].SetActive(true);
            }
        }
        else if (nowStage == 6) // middle
        {
            if (Monster_Spawn_Manager.instance.boss_spawnedMonsters.Count == 0)
            {
                subOutPortal[subStageIndex].SetActive(true);
            }
        }
        else if (nowStage == 8) // last
        {
            if (Monster_Spawn_Manager.instance.boss_spawnedMonsters.Count == 0)
            {
                GameUI.UIData.Clear();
            }
        }
    }

    public void Drop_Item(GameObject obj)
    {
        int i = Random_Percentage();
        //시연용
        //i = Random.Range(0, drop_Item.Length);
        Transform parentTransform = item_tr.transform;
        Vector2 loc = obj.GetComponent<Rigidbody2D>().position;
        loc = new Vector2(loc.x, loc.y - (obj.transform.localScale.y / 2f) + 1.8f);
        //Debug.Log(tr);

        if(parentTransform == null)
        {
            Debug.Log("부모 찾지 못함");
            //return;
        }

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
        PlayerController.playerData.audioSource.clip = PlayerController.playerData.trapRoom;
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

        //시연용 이동
        //0 = map 1 | 1 = middle_boss | 2 = last_boss
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

            if (AudioPlayBGM.instance != null)
            {
                AudioPlayBGM.instance.ChangeClip(AudioPlayBGM.instance.lastBoss);
            }
            else
            {
                Debug.LogError("AudioPlayBGM 인스턴스가 없습니다.");
            }

            Monster_Spawn_Manager.instance.Summon_Last_Boss();
        }
    }

    void SubOutPlayerMove()
    {
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
        subOutPortal[subStageIndex].SetActive(false);

        if (subStageIndex == 0) //trap1
        {
            Player.transform.position = subPoints[0].transform.position;
            nowStage = 4;
        }
        else if(subStageIndex == 1) //trap2
        {
            Player.transform.position = subPoints[1].transform.position;
            GameUI.UIData.QuizUI.QuizBoard.SetActive(true);
            nowStage = 5;
        }
        else if(subStageIndex == 2) // angel
        {
            subOutPortal[subStageIndex].SetActive(true);
            Player.transform.position = subPoints[2].transform.position;
            nowStage = 6;
        }
        else if (subStageIndex == 3) // middle
        {
            Monster_Spawn_Manager.instance.Summon_Mini_Boss();

            if (AudioPlayBGM.instance != null)
            {
                AudioPlayBGM.instance.ChangeClip(AudioPlayBGM.instance.middleBoss);
            }
            else
            {
                Debug.LogError("AudioPlayBGM 인스턴스가 없습니다.");
            }

            Player.transform.position = subPoints[3].transform.position;
            nowStage = 7;
        }
    }
}
