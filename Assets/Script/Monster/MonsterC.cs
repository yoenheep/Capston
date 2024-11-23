using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monsters
{
    SpriteRenderer sp;
    protected Vector2 currentPosition;

    float range;
    GameObject target;
    public GameObject ammo;

    protected void Start()
    {
        monster_Name = "Bone_Archer";
        monster_Armor = 0f;
        monster_Speed = 2f;
        monster_Attack_Damage = 5f;
        monster_Attack_Speed = 4.5f;
        monster_Max_Health = 100f;
        monster_Pre_Health = monster_Max_Health;

        range = 5f;
        
        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        this.Think();
    }
    void FixedUpdate()
    {
        if (!is_Dead)
        {
            if (GameUI.UIData.quizPopup.activeSelf == false)
            {
                Move();
                Search_Target();
            }
            else
            {
                Stop();
            }
        }
    }

    protected override void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bone_Archer_Attack"))
        {
            return;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bone_Archer_Hit"))
        {
            return;
        }
        else
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
        Debug.DrawRay(pos, diretion * range, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(pos, diretion, range, LayerMask.GetMask("Player"));
        
        if(rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            CancelInvoke();
            target = rayHit.collider.gameObject;
            RangeAttack(monster_Attack_Damage, target);
        }
    }
    
    protected void RangeAttack(float damage, GameObject obj)
    {
        if (obj != null && !is_Dead)
        {
            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if (Time.time >= last_Attack_Time + monster_Attack_Speed || last_Attack_Time == 0)
            {
                Stop();
                anim.SetTrigger("attack");

                last_Attack_Time = Time.time;
                Think();
            }
        }
    }

    private void Fire_Arrow()
    {
        int direction = sp.flipX ? -1 : 1;
        Vector2 obj_Position = sp.flipX ? Vector2.left : Vector2.right;

        monster_Audio.clip = monster_Audio_Clips[2];
        monster_Audio.Play();

        GameObject newBullet = Instantiate(ammo, this.gameObject.transform.position, Quaternion.identity);
        newBullet.GetComponent<Monster_Bullet>().SetDamage(monster_Attack_Damage);
        newBullet.GetComponent<Monster_Bullet>().SetMove(obj_Position, direction);
    }
}
