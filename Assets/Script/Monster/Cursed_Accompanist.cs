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
    private bool is_Stopped = false; // 행동 멈춤 여부를 나타내는 변수

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
        monster_Attack_Speed = 2f;
        monster_Max_Health = 300f;
        monster_Pre_Health = monster_Max_Health;
        is_Elite = true;

        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        this.Think();
    }

    private void FixedUpdate()
    {
        target_Position = trace_Target();

        if(GameUI.UIData.quizPopup.activeSelf == true)
        {
            is_Stopped = true;
        }
    }

    private Vector2 trace_Target()
    {
        return target.transform.position;
    }

    private IEnumerator InvokeThinkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!is_Stopped) // 행동 멈춤 상태가 아니라면 Think 호출
        {
            Think();
        }
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
        StartCoroutine(InvokeThinkAfterDelay(monster_Attack_Speed));
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
        StartCoroutine(InvokeThinkAfterDelay(monster_Attack_Speed));
        anim.SetInteger("attack", 0);
    }

    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        anim.SetTrigger("GetDamage");

        monster_Audio.clip = monster_Audio_Clips[2];
        monster_Audio.Play();

        monster_Pre_Health -= (damage - this.monster_Armor);

        if (monster_Pre_Health <= 0)
        {
            this.Die();
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

            GameObject newSkull = Instantiate(kamikaze_Skull, this.gameObject.transform.position, Quaternion.identity);
            newSkull.GetComponent<Kamikaze_Skull>().SetMove(target_Position);
        }
    }
}