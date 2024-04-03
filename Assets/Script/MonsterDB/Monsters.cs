using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    //몬스터 기본 변수
    protected string monster_Name;
    protected float monster_Max_Health;
    protected float monster_Pre_Health;
    protected float monster_Armor;
    protected float monster_Speed;
    protected float monster_Attack_Speed;
    protected float monster_Attack_Damage;
    protected bool is_dead;
    protected float lastAttackTime;
    protected float pushForce = 2f;

    //몬스터 이동 관련 변수
    private Rigidbody2D rb;
    private float x_min = 0;
    private float x_max = 0;
    private bool change_Direction;
    private bool stop = false;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        change_Direction = true;

        monster_Pre_Health = monster_Max_Health;
        is_dead = false;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(!is_dead && !stop)
        {
            Change_Direction(gameObject.transform.localPosition.x);

            if (change_Direction)
            {
                rb.velocity = new Vector2(monster_Speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-monster_Speed, rb.velocity.y);
            }
        }
    }

    private void Change_Direction(float present_Location)
    {
        //x값이 최대 범위를 벗어나면 false값으로 변경
        if (present_Location >= x_max)
        {
            change_Direction = false;
            Turn_A_Round();
        }
        else if (present_Location <= x_min)
        {
            change_Direction = true;
            Turn_A_Round();
        }
        
    }

     private void Turn_A_Round()
     {
            transform.Rotate(Vector3.up * 180f);
     }

    protected virtual void GetDamage(float damage, GameObject obj, Vector2 attack_Direction)
    {
        if(!is_dead)
        {
            Rigidbody2D obj_Rb = obj.GetComponent<Rigidbody2D>();

            // 뒤로 밀어내는 힘 적용
            if (obj_Rb != null)
            {
                Vector2 pushDirection = ((obj_Rb.position - attack_Direction)).normalized;
                
                monster_Pre_Health -= damage;
                stop = true;

                if (pushDirection.x > 0)
                {
                    obj_Rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }
                else if(pushDirection.x < 0)
                {
                    obj_Rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }
                StartCoroutine(StopForSeconds());
            }
           if(monster_Pre_Health < 0)
           {
                    Die();
           }
        }
    }

    IEnumerator StopForSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        stop = false;
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
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌이 발생한 객체의 태그가 "Ground"인 경우
        //오브젝트의 크기와 위치 값을 읽고 몬스터의 크기를 계산하여 이동범위를 설정
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (x_max == x_min)
            {
                float platform_x = collision.gameObject.transform.localPosition.x;
                float length = collision.collider.bounds.size.magnitude;
                float obj_Size = gameObject.GetComponent<Collider2D>().bounds.size.x / 2;

                x_max = platform_x + (length / 2) - obj_Size;
                x_min = platform_x - (length / 2) + obj_Size;
            }
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 player_Location = collision.gameObject.transform.position;

            if(player != null)
            {
                GetDamage(0, gameObject, player_Location);
                //GiveDamage(monster_Attack_Damage, player);
            }
        }
    }
    public int Hp = 3;
    public void TakeDamage(int damage)
    {
        Hp = Hp - damage;
    }
}