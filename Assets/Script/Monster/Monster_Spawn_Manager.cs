using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //MonsterDB�� ��� ���� Prefab�� string���� ������ �迭��
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public GameObject hpBar_Prefab;
    public List<string> monsters_Name = new List<string>();

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

            Debug.Log("����");
        }

        Summon_Monsters();
    }

    public void Summon_Monsters()
    {
        //���� ���� ����Ʈ�� monsterDB�� ����� ���͸� ��ȯ
        for (int i = 0; i < monsters_Spawn_Point.Count; i++)
        {
            //���� ũ�⿡ ���� ��ȯ ��ġ ����
            //���� ��
            //3���� �߰� �����̹Ƿ� ��ȯ�� �����ʵ��� ����
            int mon = Random.Range(0, monsters_Prefab.Count-1);

            //Ư������ ������
            //mon = 3;

            //BoxCollider2D box = monstersDB[monsters_Name[mon]].gameObject.GetComponent<BoxCollider2D>();
            float monster_Height = monstersDB[monsters_Name[mon]].gameObject.transform.localScale.y;
            Vector2 position = new Vector2(monsters_Spawn_Point[i].transform.localPosition.x, monsters_Spawn_Point[i].transform.localPosition.y + monster_Height);

            Transform tr = monsters_Spawn_Point[i].transform.parent;
            GameObject createdPrefab = Instantiate(monstersDB[monsters_Name[mon]], position, Quaternion.identity);
            createdPrefab.transform.SetParent(tr, false);

            Debug.Log("��ȯ");
        }
    }
}
