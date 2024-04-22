using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB :Monsters
{
    Sprite monster_Image;
    Animator monsterA_animator;
    AudioListener monsterA_Audio;


    protected void OnEnable()
    {
        monster_Name = "bbb";
        monster_Armor = 5f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 0.5f;
        monster_Max_Health = 500f;
        monster_Pre_Health = monster_Max_Health;


        monsterA_animator = gameObject.GetComponent<Animator>();
        monsterA_Audio = gameObject.GetComponent<AudioListener>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.monsters_Prefab[1], transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }
}
