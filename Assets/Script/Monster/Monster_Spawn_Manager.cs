using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //MonsterDB�� ��� ���� Prefab�� string���� ������ �迭��
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public GameObject hpBar_Prefab;
    public List<string> monsters_Name = new List<string>();

    public GameObject mini_Boss_Spawn_Point;
    public GameObject mini_Boss_Mon;

    public GameObject last_Boss_Spawn_Point;
    public GameObject last_Boss_Mon;


    //���� DB
    Dictionary<string, GameObject> monstersDB;

    public List<GameObject> monsters_Spawn_Point = new List<GameObject>();

    public GameObject hpBarParent;

    // ������ ���� ����Ʈ
    public List <GameObject> map1_spawnedMonsters = new List<GameObject>();
    public List<GameObject> map2_spawnedMonsters = new List<GameObject>();
    public List<GameObject> map3_spawnedMonsters = new List<GameObject>();
    public List<GameObject> map4_spawnedMonsters = new List<GameObject>();
    public List<GameObject> trap1_spawnedMonsters = new List<GameObject>();
    public List<GameObject> boss_spawnedMonsters = new List<GameObject>();

    //�̱���
    public static Monster_Spawn_Manager instance;

    //DB �ʱ�ȭ
    private void Start()
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
        Summon_Monsters_For_Map(1, 0, 12);

        Summon_Monsters_For_Map(2, 12, 15);

        Summon_Monsters_For_Map(3, 27, 12);

        Summon_Monsters_For_Map(4, 39, 15);

        if(GameManager.gameMgr.subStageIndex == 0)
        {
            Summon_Monsters_For_Map(5, 54, 4);
        }

        Summon_Mini_Boss();

        //Summon_Last_Boss();
    }

    private void Update()
    {
        if (GameManager.gameMgr.nowStage == 6)
        {
            Summon_Mini_Boss();
        }
        else if (GameManager.gameMgr.nowStage == 8) 
        {
            Summon_Last_Boss();
        }
    }

    public void Summon_Monsters_For_Map(int mapIndex, int startIndex, int monsterCount)
    {
        //���� ���� ����Ʈ�� monsterDB�� ����� ���͸� ��ȯ
        for (int i = startIndex; i < (monsterCount + startIndex); i++)
        {
            int mon = Random.Range(0, monsters_Prefab.Count);

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

            switch (mapIndex)
            {
                case 1:
                    map1_spawnedMonsters.Add(createdPrefab);
                    break;

                case 2:
                    map2_spawnedMonsters.Add(createdPrefab);
                    break;

                case 3:
                    map3_spawnedMonsters.Add(createdPrefab);
                    break;

                case 4:
                    map4_spawnedMonsters.Add(createdPrefab);
                    break;

                case 5:
                    trap1_spawnedMonsters.Add(createdPrefab);
                    break;
            }

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
        Vector2 position = new Vector2(mini_Boss_Spawn_Point.transform.localPosition.x, mini_Boss_Spawn_Point.transform.localPosition.y);
        Transform tr = mini_Boss_Spawn_Point.transform.parent;
        GameObject createdPrefab = Instantiate(mini_Boss_Mon, position, Quaternion.identity);
        createdPrefab.transform.SetParent(tr, false);

        boss_spawnedMonsters.Add(createdPrefab);

        Debug.Log("�߰� ���� ��ȯ");
    }

    public void Summon_Last_Boss()
    {
        Vector2 position = new Vector2(last_Boss_Spawn_Point.transform.localPosition.x, last_Boss_Spawn_Point.transform.localPosition.y);
        Transform tr = last_Boss_Spawn_Point.transform.parent;
        GameObject createdPrefab = Instantiate(last_Boss_Mon, position, Quaternion.identity);
        createdPrefab.transform.SetParent(tr, false);

        boss_spawnedMonsters.Add(createdPrefab);

        Debug.Log("��Ʈ ���� ��ȯ");
    }

    public void RemoveMonsterFromList(Monsters monster)
    {
        if (map1_spawnedMonsters.Contains(monster.gameObject))
        {
            map1_spawnedMonsters.Remove(monster.gameObject);
        }
        else if (map2_spawnedMonsters.Contains(monster.gameObject))
        {
            map2_spawnedMonsters.Remove(monster.gameObject);
        }
        else if (map3_spawnedMonsters.Contains(monster.gameObject))
        {
            map3_spawnedMonsters.Remove(monster.gameObject);
        }
        else if (map4_spawnedMonsters.Contains(monster.gameObject))
        {
            map4_spawnedMonsters.Remove(monster.gameObject);
        }
        else if (trap1_spawnedMonsters.Contains(monster.gameObject))
        {
            trap1_spawnedMonsters.Remove(monster.gameObject);
        }
        else if (boss_spawnedMonsters.Contains(monster.gameObject))
        {
            boss_spawnedMonsters.Remove(monster.gameObject);
        }

        Debug.Log("���Ͱ� ����Ʈ���� ���ŵ�: " + monster.gameObject.name);
    }
}
