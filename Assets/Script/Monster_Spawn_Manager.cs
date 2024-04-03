using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //몬스터가 저장될 배열
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public List<string> monsters_Name = new List<string>();

    //몬스터 탐색용 리스트
    Dictionary<string, GameObject> monstersDB;

    public List<GameObject> monsters_Spawn_Point = new List<GameObject>();

    public GameObject room;

    private void Start()
    {
        monsters_Name.Add("gog");

        monstersDB = new Dictionary<string, GameObject>();

        //몬스터들 기본 정보 저장
        //키 값으로 몬스터 이름을 가짐
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
            Debug.Log("소환");
        }
    }

    public void FindMonsterData(string key)
    {
        if (monstersDB.ContainsKey(key))
        {
            Debug.Log("탐색 성공");

        }
        else
        {
            Debug.Log("탐색 실패");
        }
    }

     //몬스터 이름값으로 몬스터를 소환
    public void Monster_Spawn(string monster_Name)
    {
        FindMonsterData(monster_Name);
    }

    public void Monster_Spawn_To_SpawnPoint()
    {

    }
}
