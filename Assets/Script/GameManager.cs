using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr { get; private set; }

    [SerializeField] private GameObject Player;

    public GameObject[] drop_Item;
    public GameObject item_tr;

    public GameObject[] mainStages;
    public GameObject mainPortal;
    public int mainStageIndex;
    public GameObject[] subStages;
    public GameObject subPortal;
    public int subStageIndex;

    private void Awake()
    {
        gameMgr = this;
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

    public void MainNextStage()
    {
        mainStages[mainStageIndex].SetActive(false);
        mainStageIndex++;
        mainStages[mainStageIndex].SetActive(true);

        mainPortal.SetActive(false);

        MainPlayerMove();
    }
    public void SubNextStage()
    {
        subStages[subStageIndex].SetActive(true);
        mainStages[mainStageIndex].SetActive(false);

        subPortal.SetActive(false);
        SubPlayerMove();
    }

    public void SubOutStage()
    {
        SubOutPlayerMove();
        subStages[subStageIndex].SetActive(false);
        subStageIndex++;
        mainStages[mainStageIndex].SetActive(true);

        mainPortal.SetActive(true);
        if(mainStageIndex != 3)
        {
            subPortal.SetActive(true);
        }
    }

    void MainPlayerMove()
    {
        if (mainStageIndex == 0)
        {
            mainPortal.transform.position = new Vector3(109.5f, -21.5f, 0);
        }
        else if (mainStageIndex == 1)
        {
            Player.transform.position = new Vector3(-11, -23, 0);
            mainPortal.transform.position = new Vector3(121.5f, -27.5f, 0);
        }
        else if (mainStageIndex == 2)
        {
            Player.transform.position = new Vector3(-34, -29, 0);
            mainPortal.transform.position = new Vector3(115.5f, -15.5f, 0);
        }
        else if(mainStageIndex == 3)
        {
            Player.transform.position = new Vector3(-36, -20, 0);
            mainPortal.transform.position = new Vector3(145.5f, 9.5f, 0);
        }
        else if( mainStageIndex == 4)
        {
            mainPortal.SetActive(false);
            Player.transform.position = new Vector3(-15, -16, 0);
        }
    }

    void SubOutPlayerMove()
    {
        Player.transform.position = subPortal.transform.position;

        if(mainStageIndex == 0)
        {
            subPortal.transform.position = new Vector3(-2.5f, -16.5f, 0);
        }
        else if(mainStageIndex == 1)
        {
            subPortal.transform.position = new Vector3(6.5f, -30.5f, 0);
        }
        else if (mainStageIndex == 2)
        {
            subPortal.transform.position = new Vector3(94.5f, -20.5f, 0);
        }
        else if (mainStageIndex == 3)
        {
            subPortal.SetActive(false);
        }
    }

    void SubPlayerMove()
    {
        if(subStageIndex == 0)
        {
            Player.transform.position = new Vector3(19, -26, 0);
        }
        else if(subStageIndex == 1 || subStageIndex == 2 || subStageIndex == 3)
        {
            Player.transform.position = new Vector3(-12, -15, 0);
        }
    }
}
