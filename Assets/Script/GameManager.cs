using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr { get; private set; }

    public GameObject[] drop_Item;
    public GameObject item_tr;

    public GameObject[] mainStages;
    public GameObject[] mainPortal;
    public int mainStageIndex;
    public GameObject[] subStages;
    public GameObject[] subPortal;
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

    /*public void NextStage()
    {

            if (nearObject.tag == "mainPortal")
            {
                mainStages[mainStageIndex].SetActive(false);
                mainStageIndex++;
                mainStages[mainStageIndex].SetActive(true);

                subPortal[mainStageIndex].SetActive(true);
                mainPortal[mainStageIndex].SetActive(false);
            }
            else if(nearObject.tag == "subPortal")
            {
                subStages[subStageIndex].SetActive(true);
            }
            else if(nearObject.tag == "subOutPortal")
            {
                subStages[subStageIndex].SetActive(false);
                subStageIndex++;
                mainStages[mainStageIndex].SetActive(true);

                mainPortal[mainStageIndex].SetActive(true);
                subPortal[mainStageIndex].SetActive(false);
            }
    }*/
}
