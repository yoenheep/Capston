using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //MonsterDB�� ��� ���� Prefab�� string���� ������ �迭��
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public GameObject hpBar_Prefab;
    public List<string> monsters_Name = new List<string>();

    public GameObject boss_Spawn_Point;
    public GameObject boss_mon;

    //���� DB
    Dictionary<string, GameObject> monstersDB;

    public List<GameObject> monsters_Spawn_Point = new List<GameObject>();

    public GameObject hpBarParent;

    //�̱���
    public static Monster_Spawn_Manager instance;

    //DB �ʱ�ȭ
    private void Awake()
    {
        instance = this;

        monstersDB = new Dictionary<string, GameObject>();

        //pre_fab�� ����� ��ŭ �迭�� �ݺ��Ͽ� monsterDB�� ä��
        for (int i = 0; i < monsters_Prefab.Count; ++i)
        {
            monsters_Name.Add(monsters_Prefab[i].name);
            monstersDB.Add(monsters_Name[i], monsters_Prefab[i]);
        }

        //���� ����
        Summon_Monsters();

        Summon_Mini_Boss();

        Summon_Last_Boss();
    }

    public void Summon_Monsters()
    {
        //���� ���� ����Ʈ�� monsterDB�� ����� ���͸� ��ȯ
        for (int i = 0; i < monsters_Spawn_Point.Count; i++)
        {
            int mon = Random.Range(0, monsters_Prefab.Count-1);

            //Ư������ ������
            //mon = 1;

            //��������Ʈ ��Ȱ��ȭ�Ͻ� ��ȯ���� ����
            if(!monsters_Spawn_Point[i].activeSelf)
            {
                continue;
            }

            //���� ũ�⿡ ���� ��ȯ ��ġ ����
            BoxCollider2D mon_Box = monsters_Prefab[mon].gameObject.GetComponent<BoxCollider2D>();
            Transform mon_Tr = monsters_Prefab[mon].gameObject.GetComponent<Transform>();
            float monster_Height = (mon_Tr.localScale.y / 2.0f) - (mon_Box.size.y * 3f);
            Vector2 position = new Vector2(monsters_Spawn_Point[i].transform.localPosition.x, monsters_Spawn_Point[i].transform.localPosition.y + monster_Height);

            Transform tr = monsters_Spawn_Point[i].transform.parent;
            GameObject createdPrefab = Instantiate(monsters_Prefab[mon], position, Quaternion.identity);
            createdPrefab.transform.SetParent(tr, false);

            Debug.Log("��ȯ");
        }
    }

    //���� ����Ʈ �Ѱ����
    //���� ������ start�� ���� �������� ������ ���� count��
    //����) 0�� �������� 6���� ��� start = 0, count = 6
    //Map 1 s = 0, c = 12 | Map2 s = 12, c = 15 | Map3 s = 27, c = 12 | Map4 s = 39, c = 15
    //TrapMap1 s = 54, c = 4 | TrapMap2 s = 58, c = 3
    //MiniBoss s = 61, c = 1 | LastBoss s =62, c = 1
    public void Turn_On_Off_Spawn_Point(int start, int count)
    {
        for(int i = 0; i < monsters_Spawn_Point.Count; i++)
        {
            monsters_Spawn_Point[i].SetActive(false);
        }

        for(int i = start; i < count; i++)
        {
            monsters_Spawn_Point[i].SetActive(true);
        }
    }

    public void Summon_Mini_Boss()
    {
        Vector2 position = new Vector2(boss_Spawn_Point.transform.localPosition.x, boss_Spawn_Point.transform.localPosition.y);
        Transform tr = boss_Spawn_Point.transform.parent;
        GameObject createdPrefab = Instantiate(boss_mon, position, Quaternion.identity);
        createdPrefab.transform.SetParent(tr, false);
        Debug.Log("�߰� ���� ��ȯ");
    }

    public void Summon_Last_Boss()
    {
        Vector2 position = new Vector2(boss_Spawn_Point.transform.localPosition.x, boss_Spawn_Point.transform.localPosition.y);
        Transform tr = boss_Spawn_Point.transform.parent;
        GameObject createdPrefab = Instantiate(boss_mon, position, Quaternion.identity);
        createdPrefab.transform.SetParent(tr, false);
        Debug.Log("��Ʈ ���� ��ȯ");
    }
}
