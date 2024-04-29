using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monsters
{
    Sprite monster_Image;
    Animator monster_animator;
    AudioListener monster_Audio;

    float range;
    GameObject target;
    public GameObject bullet;

    protected void OnEnable()
    {
        monster_Name = "ccc";
        monster_Armor = 0f;
        monster_Speed = 2f;
        monster_Attack_Damage = 10f;
        monster_Attack_Speed = 1f;
        monster_Max_Health = 100;
        monster_Pre_Health = monster_Max_Health;
        range = 5f;

        monster_animator = gameObject.GetComponent<Animator>();
        monster_Audio = gameObject.GetComponent<AudioListener>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }
    void FixedUpdate()
    {
        Move();
        SearchTarget();
    }

    void SearchTarget()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.right : Vector3.left);
        //확인용
        Debug.DrawRay(gameObject.transform.position, diretion * range, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(gameObject.transform.position, diretion, range, LayerMask.GetMask("Player"));
        
        if(rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            Debug.Log("타겟 발견");
            target = rayHit.collider.gameObject;

            Debug.Log(target.transform.position);
            //GiveDamage(monster_Attack_Damage,target.AddComponent<PlayerController>());
            RangeAttack(monster_Attack_Damage, target);
        }
    }

    protected void RangeAttack(float damage, GameObject obj)
    {

        if (obj != null)
        {
            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if (lastAttackTime >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                Debug.Log(lastAttackTime);
                Vector2 position = this.gameObject.transform.localPosition;
                Transform tr = this.gameObject.transform.parent;
                GameObject createdPrefab = Instantiate(bullet, position, Quaternion.identity);
                createdPrefab.transform.SetParent(tr, false);
                lastAttackTime = Time.time;
            }
        }
    }
}
