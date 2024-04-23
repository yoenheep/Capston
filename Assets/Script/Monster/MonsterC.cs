using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monsters
{
    Sprite monster_Image;
    Animator monsterA_animator;
    AudioListener monsterA_Audio;


    protected void OnEnable()
    {
        monster_Name = "ccc";
        monster_Armor = 0f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 0.5f;
        monster_Max_Health = 100;
        monster_Pre_Health = monster_Max_Health;


        monsterA_animator = gameObject.GetComponent<Animator>();
        monsterA_Audio = gameObject.GetComponent<AudioListener>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }
}
