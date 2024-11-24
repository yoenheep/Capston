using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    //몬스터 기본 변수
    protected string monster_Name;

    public float monster_Max_Health;
    public float monster_Pre_Health;
    public float monster_Armor;
    protected float monster_Speed;
    protected float monster_Attack_Speed;
    public float monster_Attack_Damage;

    private float damaged_Time;

    protected float last_Attack_Time;
    protected bool is_Dead;
    
    protected bool quiz_Mon;
    protected bool is_Elite;

    protected GameObject hpBar;
    protected monHpBar hpBarLogic;

    //몬스터 이동 관련 변수
    protected Rigidbody2D rb;
    protected BoxCollider2D box_Collider;
    protected SpriteRenderer sprite_Renderer;
    public int next_Move;
    protected Animator anim;
    
    protected AudioSource monster_Audio;
    public AudioClip[] monster_Audio_Clips;  /*[0]monster damaged [1]monster die ... */

    //몬스터 변수 초기화
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite_Renderer = GetComponent<SpriteRenderer>();
        box_Collider = GetComponent<BoxCollider2D>();
        monster_Audio = gameObject.GetComponent<AudioSource>();

        monster_Pre_Health = monster_Max_Health;
        is_Dead = false;
        quiz_Mon = false;
        is_Elite = false;
        last_Attack_Time = 0;
    }
    private void Update()
    {
        hpBarLogic.maxHp = monster_Max_Health; // 최대 hp
        hpBarLogic.nowHp = monster_Pre_Health; // 현재 hp
        hpBarLogic.owner = this.transform; // 체력바 주인 설정

        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        monster_Audio.volume = sfxVolume;
    }

    //몬스터 기본 이동
    protected virtual void Move()
    {
       
    }
    protected void Stop()
    {
        rb.velocity = Vector2.zero;
        anim.SetInteger("moveSpeed", 0);
    }

    //랜덤 이동 기능
    protected virtual void Think()
    {
        next_Move = Random.Range(-1, 2);

        //nextMove 값에 따라 애니메이션을 변경
        anim.SetInteger("moveSpeed", next_Move);

        //이동 상태일 경우 sprite를 뒤집음
        if (next_Move != 0)
        {
            sprite_Renderer.flipX = (next_Move == -1);
        }

        float next_Think_Time;

        if (next_Move == 0)
        {
            next_Think_Time = Random.Range(3f, 5f);
        }
        else
        {
            next_Think_Time = Random.Range(2f, 7f);
        }

        Invoke("Think", next_Think_Time);
    }

    //nextMove값을 반대로 하고 sprite를 뒤집음
    //Invoke의 Think를 초기화하고 실행
    protected void Turn()
    {
        next_Move *= -1;
        anim.SetInteger("moveSpeed", next_Move);

        if (next_Move != 0)
            sprite_Renderer.flipX = (next_Move != 1);

        CancelInvoke();
        Invoke("Think", 1.5f);
    }

    //데미지 값과 공격 방향 값을 받아 방어력 값을 뺀 데미지 값만큼 현재 체력 감소
    //몬스터는 공격 방향의 반대 방향으로 밀려남
    public virtual void GetDamage(float damage, Vector2 attack_Direction)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

        if (!is_Dead)
        {
            Debug.Log("피격");
            Stop();

            anim.SetTrigger("getDamaged");

            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float push_Force = 6f;
            monster_Pre_Health -= (damage - this.monster_Armor);

            StartCoroutine(Push_Back(direction, push_Force));

            if (monster_Pre_Health <= 0)
            {
                this.Die();
            }
        }
    }

    //0.3초 동안 뒤로 물러남
    protected IEnumerator Push_Back(Vector2 direction, float push_Force)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();
        float elapsedTime = 0f;
        float pushDuration = 0.3f; // 초기 밀리는 시간

        while (elapsedTime < pushDuration)
        {
            elapsedTime += Time.deltaTime;

            // 밀린 후 예상 위치 계산
            Vector2 predictedPosition = obj_Rb.position + new Vector2(direction.x * push_Force * 0.1f, 0);

            // 예상 위치 아래에 Platform 태그 확인
            Vector2 rayOrigin = new Vector2(predictedPosition.x, predictedPosition.y - 1f);
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null || !rayHit.collider.CompareTag("Platform"))
            {
                //Debug.Log("다음 위치에 Platform 없음");
                pushDuration -= 0.1f; // 밀리는 시간 감소

                if (pushDuration <= 0f)
                {
                    break; // 밀리는 시간을 모두 소진하면 루프 종료
                }
            }

            // 방향으로 밀기
            obj_Rb.velocity = new Vector2(direction.x * push_Force, 0);

            yield return null;
        }

        // 이동 중지
        obj_Rb.velocity = Vector2.zero;
    }

    //퀴즈 몬스터일 경우 정답일 시 몬스터 제거
    //엘리트 몬스터일 경우는 체력 감소
    public void Quiz_Mon_Die()
    {
        if(this.quiz_Mon)
        {
            Die();
        } 
    }

    //몬스터가 죽었을 때
    protected virtual void Die()
    {
        //is_dead값을 true로 변경
        if (!is_Dead)
        {
            is_Dead = true;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;

            //아이템 드롭
            if(!is_Elite)
            {
                GameManager.gameMgr.Drop_Item(gameObject);
            }

            //몬스터 사망 애니메이션 재생
            anim.SetBool("isDead", true);

            //몬스터 사망 사운드 재생
            monster_Audio.clip = monster_Audio_Clips[1];
            monster_Audio.Play();
            // 체력바
            hpBar.gameObject.SetActive(false);
            //죽으면 몬스터 객체 삭제
            //StartCoroutine(DeactivateAfterSound());

            // 리스트에서 제거
            Monster_Spawn_Manager.instance.RemoveMonsterFromList(this);
        }
    }

    public void Object_Destroy()
    {
        Destroy(this.gameObject);
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