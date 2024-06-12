using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB :Monsters
{
    Sprite monster_Image;
    Animator monster_animator;
    AudioListener monster_Audio;
    protected void OnEnable()
    {
        monster_Name = "bbb";
        monster_Armor = 10f;
        monster_Speed = 2f;
        monster_Max_Health = 500f;
        monster_Pre_Health = monster_Max_Health;

        monster_animator = gameObject.GetComponent<Animator>();
        monster_Audio = gameObject.GetComponent<AudioListener>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //¼öÁ¤ Áß
            GameUI.UIData.QuizUI.quiz();
            Debug.Log("quiz Time!");
        }
    }
}
