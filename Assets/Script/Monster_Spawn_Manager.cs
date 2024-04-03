using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //���Ͱ� ����� �迭
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public List<string> monsters_Name = new List<string>();

    //���� Ž���� ����Ʈ
    Dictionary<string, GameObject> monstersDB;

    public List<GameObject> monsters_Spawn_Point = new List<GameObject>();

    public GameObject room;

    private void Start()
    {
        monsters_Name.Add("gog");

        monstersDB = new Dictionary<string, GameObject>();

        //���͵� �⺻ ���� ����
        //Ű ������ ���� �̸��� ����
        for(int i=0; i < monsters_Prefab.Count; i++)
        {
            monstersDB.Add(monsters_Name[i], monsters_Prefab[i]);
        }
        
        for(int i = 0; i < monsters_Spawn_Point.Count; i++)
        {
            Vector2 position = monsters_Spawn_Point[i].transform.localPosition;
            Transform tr = monsters_Spawn_Point[i].transform.parent;
            GameObject createdPrefab = Instantiate(monstersDB[monsters_Name[0]], position, Quaternion.identity);
            createdPrefab.transform.SetParent(tr, false);
            Debug.Log("��ȯ");
        }
    }

    public void FindMonsterData(string key)
    {
        if (monstersDB.ContainsKey(key))
        {
            Debug.Log("Ž�� ����");

        }
        else
        {
            Debug.Log("Ž�� ����");
        }
    }

     //���� �̸������� ���͸� ��ȯ
    public void Monster_Spawn(string monster_Name)
    {
        FindMonsterData(monster_Name);
    }

    public void Monster_Spawn_To_SpawnPoint()
    {

    }
}
