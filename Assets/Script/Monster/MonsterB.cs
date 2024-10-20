using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB :Monsters
{
    PlayerController target;
    public float range;
    protected Vector2 currentPosition;

    protected void Start()
    {
        monster_Name = "Flying_Skull";
        monster_Attack_Speed = 2f;
        monster_Speed = 2f;
        monster_Armor = 99999f;
        monster_Max_Health = 100f;
        monster_Pre_Health = monster_Max_Health;
        quiz_Mon = true;
        range = 6f;

        monster_Audio = gameObject.GetComponent<AudioSource>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        this.Think();
    }

    private void FixedUpdate()
    {
        if (!is_Dead)
        {
            if (GameUI.UIData.quizPopup.activeSelf == false)
            {
                Move();
                Search_Target();
            } else
            {
                Stop();
            }
        }
    }

    protected override void Move()
    {
        currentPosition = gameObject.transform.position;

        Vector2 newPosition = new Vector2(currentPosition.x + monster_Speed * next_Move * Time.deltaTime, currentPosition.y);

        transform.position = newPosition;

        anim.SetInteger("moveSpeed", next_Move);

        rb.velocity = new Vector2(next_Move, rb.velocity.y);

        Vector2 frontVec = new Vector2(currentPosition.x + (0.9f * next_Move), currentPosition.y - 1f);
        Debug.DrawRay(frontVec, Vector3.down * 2f, new Color(1, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 2f, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Search_Target()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right);
        //사거리 확인용
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z);
        Debug.DrawRay(pos, diretion * range, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(pos, diretion, range, LayerMask.GetMask("Player"));

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            target = rayHit.collider.gameObject.GetComponent<PlayerController>();

            //Debug.Log("목표 발견");

            //몬스터의 공격이 최초이거나 마지막 공격 후에 일정 시간이 지났을 경우
            if (Time.time >= last_Attack_Time + this.monster_Attack_Speed || last_Attack_Time == 0)
            {
                CancelInvoke();
                Debug.Log("속도 증가");
                StartCoroutine(Monter_Charge(3f, monster_Attack_Speed));

                last_Attack_Time = Time.time;
            }
        }
    }

    IEnumerator Monter_Charge(float multiplier, float duration)
    {
        anim.SetBool("attack", true);

        float originalSpeed = monster_Speed;
        monster_Speed *= multiplier;

        yield return new WaitForSeconds(duration);
        monster_Speed = originalSpeed;
        Debug.Log("속도 감소");

        anim.SetBool("attack", false);
        Invoke("Think", 1f);
    }

    protected void Quiz_Attack(PlayerController obj)
    {
        if (obj != null)
        {
            GameUI.UIData.QuizUI.quiz(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //수정 중
            PlayerController player = collision.GetComponent<PlayerController>();

            monster_Audio.clip = monster_Audio_Clips[2];
            monster_Audio.Play();

            Quiz_Attack(player);
        }
    }
}
