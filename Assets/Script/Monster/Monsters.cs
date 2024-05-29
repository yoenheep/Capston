using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;

    //몬스터 기본 변수
    protected string monster_Name;
    protected float monster_Max_Health;
    protected float monster_Pre_Health;
    protected float monster_Armor;
    protected float monster_Speed;
    protected float monster_Attack_Speed;
    public float monster_Attack_Damage;
    protected bool is_dead;
    protected float lastAttackTime;

    protected GameObject hpBar;
    protected monHpBar hpBarLogic;

    //몬스터 이동 관련 변수
    protected Rigidbody2D rb;
    protected int nextMove;

    //몬스터 변수 초기화
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        monster_Pre_Health = monster_Max_Health;
        is_dead = false;
        lastAttackTime = 0;

        Think();
        //아이템 드롭 확인용
        //Die();
    }
    private void Update()
    {
        hpBarLogic.maxHp = monster_Max_Health; // 최대 hp
        hpBarLogic.nowHp = monster_Pre_Health; // 현재 hp
        hpBarLogic.owner = this.transform; // 체력바 주인 설정
    }
    void FixedUpdate()
    {
        Move();
    }

    //몬스터 기본 이동
    protected void Move()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            rb.velocity = new Vector2(monster_Speed * nextMove, rb.velocity.y);

            Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y - gameObject.transform.localScale.y);
            //확인용
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //rayHit가 null일 때 Pause를 실행해서 잠시 멈추고 Turn을 실행해서 반대로 이동
            if (rayHit.collider == null)
            {
                Pause();
                Turn();
            }
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    //랜덤 이동 기능
    protected void Think()
    {
        nextMove = Random.Range(-1, 2);
        //nextMove 값에 따라 애니메이션을 변경
        anim.SetInteger("MoveSpeed", nextMove);

        //이동 상태일 경우 nextMove값이 1일 때 sprite를 뒤집음
        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        float nextThinkTime = Random.Range(1f, 5f);
        Invoke("Think", nextThinkTime);
    }

    //이동 중지
    protected void Pause()
    {
        nextMove = 0;
        anim.SetInteger("MoveSpeed", nextMove);
    }

    //nextMove값을 반대로 하고 sprite를 뒤집음
    //Invoke의 Think를 초기화하고 실행
    protected void Turn()
    {
        nextMove *= -1;
        anim.SetInteger("MoveSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        CancelInvoke();
        Invoke("Think", 0.5f);
    }

    //데미지 값과 공격 방향 값을 받아 방어력 값을 뺀 데미지 값만큼 현재 체력 감소
    //몬스터는 공격 방향의 반대 방향으로 밀려남
    public virtual void GetDamage(float damage, Vector2 attack_Direction)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

        //obj_Rb가 null이 아니고 is_dead가 false일 때
        if (!is_dead && obj_Rb != null)
        {
            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float pushForce = 1.5f;

            //공격방향과 pushForce값을 계산하여 몬스터가 1f 이상 밀려났을 때 좌표 값
            Vector2 pushAmount = direction * Mathf.Max(pushForce, 1f);

            monster_Pre_Health -= (damage - this.monster_Armor);

            Debug.Log(monster_Pre_Health);
            // 밀린 후의 위치 값
            Vector2 newPosition = new Vector2(present_Position.x + pushAmount.x, present_Position.y);

            // 몬스터를 밀린 후의 위치로 이동
            //몬스터 피격시 이상이 생길경우 주석처리
            obj_Rb.MovePosition(Check_Cliff(newPosition, present_Position));

            //if (GameUI.UIData.answerTrueIcon.activeSelf == true) // 이따가 보기로하죠
            //{
            //    monster_Pre_Health = 0;
            //}

            if (monster_Pre_Health <= 0)
            {
                Die();
            }
        }  
    }

    //밀려난 위치가 플랫폼 끝일 경우 x값을 변경
    private Vector2 Check_Cliff(Vector2 after, Vector2 before)
    {
        float trans;
        Vector2 final = after;
        RaycastHit2D rayHit = Physics2D.Raycast(after, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(after.x < before.x)
        {
            trans = 0.01f;
        } else
        {
            trans = -0.01f;
        }
        do
        {
            final = new Vector2(after.x + trans, after.y);
        } while (rayHit.collider == null);
        return final;
    }

    protected virtual void MeleeDamage(float damage, PlayerController obj)
    {
        if (obj != null)
        {
            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if(Time.time >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                obj.Hp(monster_Attack_Damage, rb.position);

                lastAttackTime = Time.time;
            }
        }
    }

    //몬스터가 죽었을 때
    protected virtual void Die()
    {
        //is_dead값을 true로 변경하고 충돌이 더이상 일어나지 않도록 Rigidbody를 false로 변경
        if (!is_dead)
        {
            is_dead = true;
            //gameObject.GetComponent<Rigidbody2D>().simulated = false;
            //아이템 드롭
            GameManager.gameMgr.Drop_Item(gameObject);

            // 체력바
            //hpBar.gameObject.SetActive(false);
            //죽으면 몬스터 객체 삭제
            this.gameObject.SetActive(false);
            //GameUI.UIData.Clear(); 나중에 보스에게 쓸것
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //몬스터가 추락시 낙사 처리
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("낙사");
            Die();
        }
    }
}