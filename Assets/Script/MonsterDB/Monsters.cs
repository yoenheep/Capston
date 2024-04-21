using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
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
    private int next_Move;

    protected GameObject hpBar;
    protected monHpBar hpBarLogic;

    //���� �̵� ���� ����
    private Rigidbody2D rb;
    private bool stop = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Think();
    }

    private void OnEnable()
    {
        monster_Pre_Health = monster_Max_Health;
        is_dead = false;
    }

    void FixedUpdate()
    {
        Move();
        hpBarLogic.maxHp = monster_Max_Health; // �ִ� hp
        hpBarLogic.nowHp = monster_Pre_Health; // ���� hp
        hpBarLogic.owner = this.transform; // ü�¹� ���� ����
    }

    public void Move()
    {
        if(!is_dead && !stop)
        {
            rb.velocity = new Vector2(monster_Speed, rb.velocity.y);

            Vector2 frontVec = new Vector2(rb.position.x + monster_Speed * 0.3f, rb.position.y);
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null)
            {
                monster_Speed *= -1;
                //Turn_A_Round();
                
            }
        }
    }

     private void Turn_A_Round()
     {
            transform.Rotate(Vector3.up * 180f);
     }

    void Think()
    {
        next_Move = Random.Range(-1, 2);

        Think();
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
                monster_Pre_Health -= damage;

                Debug.Log("���� ���� ü�� : " + monster_Pre_Health);
                Debug.Log("���� ���� : " + pushDirection);

                //obj_Rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                rb.transform.position = new Vector2(rb.position.x + 2f, rb.position.y);
            }
           if(monster_Pre_Health <= 0)
           {
                    Die();
           }
        }
    }

    protected virtual void GiveDamage(float damage, Player_Controller obj) {
        obj.health -= damage;
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