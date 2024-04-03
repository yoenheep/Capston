using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : Monsters
{ 
    Sprite monster_Image;
    Animator monsterA_animator;
    AudioListener monsterA_Audio;

    protected void OnEnable()
    {
        monster_Name = "ggg";
        monster_Max_Health = 100f;
        monster_Armor = 10f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 0.5f;

        monsterA_animator = gameObject.GetComponent<Animator>();
        monsterA_Audio = gameObject.GetComponent<AudioListener>();
    }
}
