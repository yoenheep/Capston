using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr { get; private set; }

    public GameObject[] drop_Item;
    public GameObject item_tr;
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
            Debug.Log("�θ� ã�� ����");
            //return;
        }

        int i = Random_Percentage();

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
}
