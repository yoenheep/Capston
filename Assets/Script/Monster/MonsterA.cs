using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterA : Monsters
{ 
    protected void OnEnable()
    {
        monster_Name = "AAA";
        monster_Armor = 0f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 3.5f;
        monster_Max_Health = 100f; 
        monster_Pre_Health = monster_Max_Health;
        
        monster_Audio = gameObject.GetComponent<AudioSource>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 근접공격");
            //수정 필요 사항 : 반응이 좀 늦게 오는 듯함

            MeleeDamage(monster_Attack_Damage, collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
