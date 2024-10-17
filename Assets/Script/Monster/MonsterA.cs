using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterA : Monsters
{
    PlayerController target;
    public float range;
    protected Vector2 currentPosition;

    protected void Start()
    {
        monster_Name = "Bone_Sword_Man";
        monster_Armor = 0f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 1.8f;

        range = 1.6f;
        monster_Max_Health = 150f; 
        monster_Pre_Health = monster_Max_Health;

        monster_Audio = gameObject.GetComponent<AudioSource>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        this.Think();
    }

    private void FixedUpdate()
    {
        if (!is_Dead)
        {
            if(GameUI.UIData.quizPopup.activeSelf == false)
            {
                Move();
                Search_Target();
            } else
            {
                Stop();
            }
        }
    }

    protected override void Move()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Bone_Sword_Man_Attack"))
        {
            return;
        } else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Bone_Sword_Man_Damaged"))
        {
            return;
        } else
        {
            currentPosition = gameObject.transform.position;

            Vector2 newPosition = new Vector2(currentPosition.x + monster_Speed * next_Move * Time.deltaTime, currentPosition.y);

            transform.position = newPosition;

            anim.SetInteger("moveSpeed", next_Move);

            rb.velocity = new Vector2(next_Move, rb.velocity.y);

            Vector2 frontVec = new Vector2(currentPosition.x + (0.9f * next_Move), currentPosition.y - 1f);
            Debug.DrawRay(frontVec, Vector3.down * 2f, new Color(1, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 2f, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null)
            {
                Turn();
            }
        }
    }

    void Search_Target()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right);
        //사거리 확인용
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z);
        //Debug.DrawRay(pos, diretion * range, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(pos, diretion, range, LayerMask.GetMask("Player"));

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            target = rayHit.collider.gameObject.GetComponent<PlayerController>();

            //Debug.Log("목표 발견");

            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if (Time.time >= last_Attack_Time + this.monster_Attack_Speed || last_Attack_Time == 0)
            {
                Stop();
                //monster_Audio.clip = monster_Audio_Clips[0];
                //monster_Audio.Play();
                anim.SetTrigger("attack");

                CancelInvoke();
                Invoke("Think", 1f);
                last_Attack_Time = Time.time;
            }
        }
    }

    public void Melee_Attack()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right);
        //사거리 확인용
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z);
        Debug.DrawRay(pos, diretion * range, new Color(0, 0, 1));
        RaycastHit2D rayHit = Physics2D.Raycast(pos, diretion, range, LayerMask.GetMask("Player"));

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            if (target != null)
            {
                target.Hp(monster_Attack_Damage, rb.position);
            }
        }
    }
}
