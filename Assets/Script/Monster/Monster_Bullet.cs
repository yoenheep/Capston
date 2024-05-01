using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet : Monsters
{
    Sprite monster_Image;
    Animator monster_animator;
    AudioListener monster_Audio;


    protected void OnEnable()
    {
        monster_Name = "bullet";
        monster_Armor = 0f;
        monster_Speed = 3f;

        monster_animator = gameObject.GetComponent<Animator>();
        monster_Audio = gameObject.GetComponent<AudioListener>();
    }

    Monster_Bullet(float damage)
    {
        monster_Attack_Damage = damage;
    }

    private void Move(Vector2 target)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡¢√À");
        }
    }
}
