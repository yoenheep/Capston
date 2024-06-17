using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB :Monsters
{
    protected void OnEnable()
    {
        monster_Name = "BBB";
        monster_Speed = 2f;
        monster_Attack_Speed = 22f;
        monster_Max_Health = 100f;
        monster_Pre_Health = monster_Max_Health;
        quiz_Mon = true;

        monster_Audio = gameObject.GetComponent<AudioSource>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }

    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

        //obj_Rb가 null이 아니고 is_dead가 false일 때
        if (!is_dead && obj_Rb != null)
        {
            anim.SetTrigger("GetDamage");

            monster_Audio.clip = monster_Audio_Clips[2];
            monster_Audio.Play();

            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float pushForce = 1.5f;

            //공격방향과 pushForce값을 계산하여 몬스터가 1f 이상 밀려났을 때 좌표 값
            Vector2 pushAmount = direction * Mathf.Max(pushForce, 1f);

            // 밀린 후의 위치 값
            Vector2 newPosition = new Vector2(present_Position.x + pushAmount.x, present_Position.y);

            // 몬스터를 밀린 후의 위치로 이동
            Debug.DrawRay(newPosition, Vector3.down, new Color(1, 0, 0));
            obj_Rb.MovePosition(Check_Cliff(newPosition, present_Position));

            //if (GameUI.UIData.answerTrueIcon.activeSelf == true) // 이따가 보기로하죠
            //{

            //}

            if (monster_Pre_Health <= 0)
            {
                this.Die();
            }
        }
    }
    protected void Quiz_Attack(PlayerController obj)
    {
        if (obj != null)
        {
            Debug.Log(lastAttackTime);
            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if (Time.time >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                GameUI.UIData.QuizUI.quiz(this.gameObject);

                lastAttackTime = Time.time;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //수정 중
            PlayerController player = collision.GetComponent<PlayerController>();

            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            Quiz_Attack(player);
            Debug.Log("quiz Time!");
        }
    }
}
