using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawn_Manager : MonoBehaviour
{
    //MonsterDB에 담기 위한 Prefab과 string값을 저장할 배열들
    public List<GameObject> monsters_Prefab = new List<GameObject>();
    public GameObject hpBar_Prefab;
    public List<string> monsters_Name = new List<string>();

    //몬스터 DB
    Dictionary<string, GameObject> monstersDB;

    public List<GameObject> monsters_Spawn_Point = new List<GameObject>();

    public GameObject room;

    public GameObject hpBarParent;

    //싱글톤
    public static Monster_Spawn_Manager instance;

    //DB 초기화
    private void Awake()
    {
        instance = this;

        monstersDB = new Dictionary<string, GameObject>();

        //pre_fab에 저장된 만큼 배열을 반복하여 monsterDB를 채움
        for (int i = 0; i < monsters_Prefab.Count; ++i)
        {
            monsters_Name.Add(monsters_Prefab[i].name);
            monstersDB.Add(monsters_Name[i], monsters_Prefab[i]);
        }


        //몬스터 스폰 포인트에 monsterDB에 저장된 몬스터를 소환
        for (int i = 0; i < monsters_Spawn_Point.Count; i++)
        {
            //몬스터 크기에 따른 소환 위치 조정
            //수정 중
            int mon = Random.Range(0, monsters_Prefab.Count);
            //특정몬스터 생성용
            mon = 0;
            BoxCollider2D box = monstersDB[monsters_Name[mon]].gameObject.GetComponent<BoxCollider2D>();
            float monster_Height = monstersDB[monsters_Name[mon]].gameObject.transform.localScale.y;
            Vector2 position = new Vector2(monsters_Spawn_Point[i].transform.localPosition.x, monsters_Spawn_Point[i].transform.localPosition.y+ monster_Height);

            Transform tr = monsters_Spawn_Point[i].transform.parent;
            GameObject createdPrefab = Instantiate(monstersDB[monsters_Name[mon]], position, Quaternion.identity);

            createdPrefab.transform.SetParent(tr, false);
            Debug.Log("소환");
        }
    }
}
