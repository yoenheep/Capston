using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    protected Animator anim;
    SpriteRenderer spriteRenderer;

    //몬스터 기본 변수
    protected string monster_Name;
    protected float monster_Max_Health;
    public float monster_Pre_Health;
    protected float monster_Armor;
    protected float monster_Speed;
    protected float monster_Attack_Speed;
    public float monster_Attack_Damage;
    protected bool is_dead;
    protected float lastAttackTime;
    protected bool quiz_Mon;

    protected GameObject hpBar;
    protected monHpBar hpBarLogic;

    //몬스터 이동 관련 변수
    protected Rigidbody2D rb;
    protected int nextMove;

    protected AudioSource monster_Audio;
    public AudioClip[] monster_Audio_Clips;

    //몬스터 변수 초기화
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monster_Audio = gameObject.GetComponent<AudioSource>();

        monster_Pre_Health = monster_Max_Health;
        is_dead = false;
        quiz_Mon = false;
        lastAttackTime = 0;

        Think();
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
    protected virtual void Move()
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
    protected virtual void Think()
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
            anim.SetTrigger("GetDamage");

            monster_Audio.clip = monster_Audio_Clips[2];
            monster_Audio.Play();

            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float pushForce = 1.5f;

            //공격방향과 pushForce값을 계산하여 몬스터가 1f 이상 밀려났을 때 좌표 값
            Vector2 pushAmount = direction * Mathf.Max(pushForce, 1f);

            monster_Pre_Health -= (damage - this.monster_Armor);

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

    //밀려난 위치가 플랫폼 끝일 경우 x값을 변경
    protected Vector2 Check_Cliff(Vector2 after, Vector2 before)
    {
        Vector2 final;

        RaycastHit2D checkRay = Physics2D.Raycast(after, Vector2.down, 2.0f);
        Debug.DrawRay(after, Vector2.down * 2.0f, new Color(0, 1, 0));

        if (checkRay.collider != null)
        {
            final = after;
        }
        else
        {
            final = before;
        }

        return final;
    }

    public void Quiz_Mon_Die()
    {
        if(this.quiz_Mon)
        {
            Die();
        }
    }

    protected virtual void MeleeDamage(float damage, PlayerController obj)
    {
        if (obj != null)
        {
            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if(Time.time >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                monster_Audio.clip = monster_Audio_Clips[0];
                monster_Audio.Play();

                obj.Hp(monster_Attack_Damage, rb.position);

                lastAttackTime = Time.time;
            } 
        }
    }

    //몬스터가 죽었을 때
    protected virtual void Die()
    {
        //is_dead값을 true로 변경
        if (!is_dead)
        {
            is_dead = true;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            //아이템 드롭
            GameManager.gameMgr.Drop_Item(gameObject);

            //몬스터 사망 애니메이션 재생
            anim.SetBool("isDead", true);

            //몬스터 사망 사운드 재생
            monster_Audio.clip = monster_Audio_Clips[1];
            monster_Audio.Play();
            // 체력바
            hpBar.gameObject.SetActive(false);
            //죽으면 몬스터 객체 삭제
            StartCoroutine(DeactivateAfterSound());
            //GameUI.UIData.Clear(); 나중에 보스에게 쓸것
        }
    }

    private IEnumerator DeactivateAfterSound()
    {
        // 오디오 클립 길이만큼 대기
        yield return new WaitForSeconds(monster_Audio.clip.length);

        // 게임 오브젝트 비활성화
        Debug.Log("몬스터 삭제");
        this.gameObject.SetActive(false);
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