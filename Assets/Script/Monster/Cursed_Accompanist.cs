using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursed_Accompanist : Monsters
{
    SpriteRenderer sp;

    public GameObject target;
    public GameObject music_Note;
    public GameObject kamikaze_Skull;

    private bool attacking = false;
    private bool is_Stopped = false;
    private float wait_Time;

    private int next_Attack;
    private float note_Damage;
    Vector2 target_Position;

    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            target = player.gameObject;
            target_Position = trace_Target();
        }

        monster_Armor = 3f;
        note_Damage = 6f;
        monster_Attack_Speed = 3f;
        monster_Max_Health = 300f;
        monster_Pre_Health = monster_Max_Health;
        wait_Time = 0;
        is_Elite = true;

        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        this.Think();
    }

    private void Update()
    {
        hpBarLogic.maxHp = monster_Max_Health; // 최대 hp
        hpBarLogic.nowHp = monster_Pre_Health; // 현재 hp
        hpBarLogic.owner = this.transform; // 체력바 주인 설정

        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        monster_Audio.volume = sfxVolume;
    }
    private void FixedUpdate()
    {
        target_Position = trace_Target();

        if(is_Dead)
        {
            return;
        }

        if(GameUI.UIData.quizPopup.activeSelf == true)
        {
            is_Stopped = true;
        } else
        {
            if (!attacking) // 공격 중이 아닌 상태일 때
            {
                wait_Time += Time.deltaTime; // 시간 누적

                if (wait_Time >= 3f) // 일정 시간 지나면
                {
                    Debug.Log("think 강제실행");
                    Think(); // Think 호출
                    wait_Time = 0f; // 타이머 초기화
                }
            }
            else
            {
                wait_Time = 0f; // attacking 상태라면 타이머 초기화
            }
        }
    }

    private Vector2 trace_Target()
    {
        return target.transform.position;
    }

    protected override void Think()
    {
        if (is_Stopped) // 멈춘 상태라면 Think를 실행하지 않음
        {
            return;
        }

        Debug.Log("다시 돌아옴");
        next_Attack = Random.Range(0, 2);
        next_Attack = (next_Attack == 0) ? -1 : 1;

        anim.SetInteger("attack", next_Attack);
        Debug.Log("실행 " + next_Attack);

        //공격 조정용
        //next_Attack = 1;

        if (next_Attack < 0)
        {
            //노트 공격 실행
            StartCoroutine(Attack_1());
        }
        else
        {
            //해골 소환 공격 실행
            StartCoroutine(Attack_2());
        }
    }

    //음표 공격 코루틴
    private IEnumerator Attack_1()
    {
        if (!attacking)
        {
            attacking = true;
            Debug.Log("1번 공격");

            float elapsed_Time = 0f;
            float attack_Duration = 7f;

            while (elapsed_Time < attack_Duration)
            {
                if ((int)elapsed_Time % 1 == 0)
                {
                    Note_Attack(note_Damage, target);
                }
                yield return new WaitForSeconds(1f);
                elapsed_Time += 1f;
            }
        }
        attacking = false;
        Debug.Log("Attack_1 종료, Think 호출");
        Invoke("Think", monster_Attack_Speed);
        anim.SetInteger("attack", 0);
    }

    //해골 공격 코루틴
    private IEnumerator Attack_2()
    {
        if (!attacking)
        {
            attacking = true;
            Debug.Log("2번 공격");

            float elapsed_Time = 0f;
            float attack_Duration = 2f;

            //attackDuration 동안 공격
            while (elapsed_Time < attack_Duration)
            {
                if ((int)elapsed_Time % 1 == 0)
                {
                    Summon_Kamikaze(target);
                }
                yield return new WaitForSeconds(1f);
                elapsed_Time += 1f;
            }
        }
        attacking = false;
        Debug.Log("Attack_2 종료, Think 호출");
        Invoke("Think", monster_Attack_Speed);
        anim.SetInteger("attack", 0);
    }

    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        if(!is_Dead)
        {
            if(!attacking)
            {
                anim.SetTrigger("GetDamage");
            }

            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            monster_Pre_Health -= (damage - this.monster_Armor);

            if (monster_Pre_Health <= 0)
            {
                this.Die();
            }
        }
    }

    //음표를 플레이어 위치로 날림
    protected void Note_Attack(float damage, GameObject target)
    {
        if (target != null && !is_Dead)
        {
            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            GameObject newNote = Instantiate(music_Note, this.gameObject.transform.position, Quaternion.identity);
            newNote.GetComponent<Music_Note>().SetDamage(damage);
            newNote.GetComponent<Music_Note>().SetMove(target_Position);
        }
    }

    //문제 공격하는 자폭 해골 소환
    protected void Summon_Kamikaze(GameObject target)
    {
        if (target != null && !is_Dead)
        {
            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            GameObject newSkull = Instantiate(kamikaze_Skull, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y -1f), Quaternion.identity);
            newSkull.GetComponent<Kamikaze_Skull>().SetMove(target_Position);
        }
    }

    protected override void Die()
    {
        if (!is_Dead)
        {
            Debug.Log("죽음 실행");
            is_Dead = true;

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
}