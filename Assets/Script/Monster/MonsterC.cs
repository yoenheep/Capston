using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monsters
{
    SpriteRenderer sp;
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
        monster_Attack_Damage = 5f;
        monster_Attack_Speed = 3f;
        monster_Max_Health = 100f;
        monster_Pre_Health = monster_Max_Health;

        range = 5f;

        monster_animator = gameObject.GetComponent<Animator>();
        monster_Audio = gameObject.GetComponent<AudioListener>();
        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }
    void FixedUpdate()
    {
        this.Move();
        SearchTarget();
    }

    protected new void Move()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            rb.velocity = new Vector2(monster_Speed * nextMove, rb.velocity.y);

            Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y -1f);
            //Ȯ�ο�
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //rayHit�� null�� �� Pause�� �����ؼ� ��� ���߰� Turn�� �����ؼ� �ݴ�� �̵�
            if (rayHit.collider == null)
            {
                Pause();
                Turn();
            }
        }
    }

    void SearchTarget()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.right : Vector3.left);
        //��Ÿ� Ȯ�ο�
        //Debug.DrawRay(gameObject.transform.position, diretion * range, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(gameObject.transform.position, diretion, range, LayerMask.GetMask("Player"));
        
        if(rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            target = rayHit.collider.gameObject;

            RangeAttack(monster_Attack_Damage, target);
        }
    }

    protected void RangeAttack(float damage, GameObject obj)
    {
        if (obj != null)
        {
            
            //������ ������ �����̰ų� ������ ���� �Ŀ� ���� �ð��� ������ ���
            if (Time.time >= lastAttackTime + monster_Attack_Speed || lastAttackTime == 0)
            {
                int direction = sp.flipX ? -1 : 1;
                Vector2 obj_Position = sp.flipX ? Vector2.left : Vector2.left;

                GameObject newBullet = Instantiate(bullet, this.gameObject.transform.position, Quaternion.identity);
                newBullet.GetComponent<Monster_Bullet>().SetDamage(damage);
                newBullet.GetComponent<Monster_Bullet>().SetMove(obj_Position, direction);

                lastAttackTime = Time.time;
            }
        }
    }
}
