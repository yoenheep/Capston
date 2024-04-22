using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;

    //���� �⺻ ����
    protected string monster_Name;
    protected float monster_Max_Health;
    protected float monster_Pre_Health;
    protected float monster_Armor;
    protected float monster_Speed;
    protected float monster_Attack_Speed;
    protected float monster_Attack_Damage;
    protected bool is_dead;
    protected float lastAttackTime;
    protected float pushForce = 80f;    //���� ����

    protected GameObject hpBar;
    protected monHpBar hpBarLogic;

    //���� �̵� ���� ����
    private Rigidbody2D rb;
    private int nextMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Think();
    }

    private void OnEnable()
    {
        monster_Pre_Health = monster_Max_Health;
        is_dead = false;
    }

    void FixedUpdate()
    {
        hpBarLogic.maxHp = monster_Max_Health; // �ִ� hp
        hpBarLogic.nowHp = monster_Pre_Health; // ���� hp
        hpBarLogic.owner = this.transform; // ü�¹� ���� ����

        Move();
    }

    protected void Move()
    {
        rb.velocity = new Vector2(monster_Speed * nextMove, rb.velocity.y);

        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove = 0;
            anim.SetInteger("MoveSpeed", nextMove);
            Turn();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);
        anim.SetInteger("MoveSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        anim.SetInteger("MoveSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        CancelInvoke();
        Invoke("Think", 5);
    }

    public virtual void GetDamage(float damage,  Vector2 attack_Direction)
    {
        if(!is_dead)
        {
            Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

            // �ڷ� �о�� �� ����
            if (obj_Rb != null)
            {
                Vector2 pushDirection = ((obj_Rb.position - attack_Direction)).normalized;
                monster_Pre_Health -= (damage - this.monster_Armor);

                Debug.Log("���� ���� ü�� : " + monster_Pre_Health);

                //obj_Rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                rb.transform.position = new Vector2(rb.position.x + 2f, rb.position.y);
            }
           if(monster_Pre_Health <= 0)
           {
                    Die();
           }
        }
    }

    protected virtual void GiveDamage(float damage, PlayerController obj) {
        obj.charac_PreHP -= damage;
    }

    protected virtual void Die()
    {
        if(!is_dead)
        {
            is_dead = true;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;

            // ü�¹�
            hpBar.gameObject.SetActive(false);
            GameUI.UIData.Clear();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 player_Location = collision.gameObject.transform.position;

            if(player != null)
            {
                //GiveDamage(monster_Attack_Damage, player);
            }
        }
    }
}