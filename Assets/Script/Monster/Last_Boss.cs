using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last_Boss : Monsters
{
    SpriteRenderer sp;

    public GameObject target;
    public GameObject music_Note;
    public GameObject minion;

    private bool attacking = false;
    protected bool is_Stopped = false;
    private float wait_Time;
    private int shield;

    private int next_Attack;
    private float range_Damage;
    Vector2 boss_Position;

    float move_Duration;
    float summon_Minion_Time;
    float summon_Minion_Cool_Time;

    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            target = player.gameObject;
        }

        monster_Name = "IAmYourDeath";
        monster_Attack_Damage = 5f;
        monster_Armor = 2f;
        range_Damage = 6f;
        monster_Attack_Speed = 3.5f;
        monster_Max_Health = 650f;
        monster_Pre_Health = monster_Max_Health;
        shield = 3;
        summon_Minion_Time = -100f;
        summon_Minion_Cool_Time = 15f;
        is_Elite = true;

        move_Duration = Get_Animation_Clip_Length("Death_Teleport");

        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        StartCoroutine(Teleport());
    }

    private void FixedUpdate()
    {
        if (GameUI.UIData.quizPopup.activeSelf == true)
        {
            is_Stopped = true;
        } else
        {
            is_Stopped = false;

            if (!attacking) // ���� ���� �ƴ� ������ ��
            {
                wait_Time += Time.deltaTime; // �ð� ����

                if (wait_Time >= 3f) // ���� �ð� ������
                {
                    Think(); // Think ȣ��
                    wait_Time = 0f; // Ÿ�̸� �ʱ�ȭ
                }
            }
            else
            {
                wait_Time = 0f; // attacking ���¶�� Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    private Vector2 trace_Target()
    {
        return target.transform.position;
    }

    protected override void Think()
    {
        if (is_Stopped) // ���� ���¶�� Think�� �������� ����
        {
            return;
        }

        next_Attack = Random.Range(1, 4);

        while(next_Attack == 2)
        {
            next_Attack = Random.Range(1, 4);
        }

        if (monster_Pre_Health < (monster_Max_Health / 3f) * (shield - 1))
        {
            next_Attack = 4;
            shield -= 1;
        }

        //���� ������
        //next_Attack = 4;

        anim.SetInteger("attack", next_Attack);
        Debug.Log("���� " + next_Attack);

        //1�� 2���� ���ֵθ��� | 2�� �� ������� | 3�� �ϼ��� ��ȯ | 4�� �߾� ���� ����
        //���� �������� 2���� ��� �̱���
        switch (next_Attack)
        {
            case 1:
                StartCoroutine(Attack_1());
                break;
            case 3:
                if(Time.deltaTime > summon_Minion_Time + summon_Minion_Cool_Time)
                {
                    StartCoroutine(Attack_3());
                } else
                {
                    Think();
                }
                
                break;
            case 4:
                StartCoroutine(Attack_4());
                break;
        }
    }

    //�÷��̾�Է� �ڷ���Ʈ �ڷ�ƾ
    private IEnumerator Teleport()
    {
        anim.SetTrigger("move");

        yield return new WaitForSeconds(move_Duration);

        Think();
    }

    //���� ���� �÷��̾������� �̵�
    public void Boss_To_Player()
    {
        Vector2 player_Pos = trace_Target();
        gameObject.transform.position = new Vector2(player_Pos.x, player_Pos.y + 3.1f);
        int direction = (player_Pos.x - rb.position.x) < 0 ? -1 : 1;

        sp.flipX = direction < 0;

        if (sp.flipX)
            box_Collider.offset = new Vector2(Mathf.Abs(box_Collider.offset.x), box_Collider.offset.y);
        else
            box_Collider.offset = new Vector2(-box_Collider.offset.x, box_Collider.offset.y);

        if (sp.flipX == true)
        {
            boss_Position = new Vector2(rb.position.x + Mathf.Abs(box_Collider.offset.x), rb.position.y);
        }
        else
        {
            boss_Position = new Vector2(rb.position.x + box_Collider.offset.x, rb.position.y);
        }
    }

    //2���� �� �ֵθ��� �ڷ�ƾ
    private IEnumerator Attack_1()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack_01"))
        {
            yield return null;  //�ٸ� �ִϸ��̼��� ���� ������ ���
        }

        float attack_Duration = Get_Animation_Clip_Length("Death_Attack_01");

        if (!attacking)
        {
            attacking = true;
            Debug.Log("1�� ����");

            yield return new WaitForSeconds(attack_Duration);

            anim.SetInteger("attack", 0);
        }
        attacking = false;

        yield return new WaitForSeconds(monster_Attack_Speed);
        StartCoroutine(Teleport());
    }

    //������ ������� �ڷ�ƾ
    //�ð� ������ ���
    private IEnumerator Attack_2(Vector2 player_pos)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack_02"))
        {
            yield return null;
        }

        float attack_Duration = Get_Animation_Clip_Length("Death_Attack_02");

        if (!attacking)
        {
            attacking = true;
            Debug.Log("2�� ����");

            yield return new WaitForSeconds(attack_Duration);

            anim.SetInteger("attack", 0);
        }

        attacking = false;

        yield return new WaitForSeconds(monster_Attack_Speed);
        StartCoroutine(Teleport());
    }

    //�ϼ��� ��ȯ �ڷ�ƾ
    private IEnumerator Attack_3()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack_03"))
        {
            yield return null;
        }

        float attack_Duration = Get_Animation_Clip_Length("Death_Attack_03");

        summon_Minion_Time = Time.deltaTime;
        if (!attacking)
        {
            attacking = true;
            Debug.Log("3�� ����");


            yield return new WaitForSeconds(attack_Duration);
        }
        attacking = false;
        anim.SetInteger("attack", 0);

        yield return new WaitForSeconds(monster_Attack_Speed);
        StartCoroutine(Teleport());
    }

    //�ϼ��� ��ȯ �ڷ�ƾ
    private IEnumerator Attack_4()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack_04"))
        {
            yield return null;
        }

        float attack_Duration = Get_Animation_Clip_Length("Death_Attack_04");

        if (!attacking)
        {
            attacking = true;
            Debug.Log("4�� ����");

            yield return new WaitForSeconds(attack_Duration);

            anim.SetInteger("attack", 0);
        }

        attacking = false;
        StartCoroutine(Teleport());
    }

    //�ǰ�
    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        //�ִϸ��̼� ������ ����
        //anim.SetTrigger("getDamage");

        monster_Audio.clip = monster_Audio_Clips[0];
        monster_Audio.Play();

        Debug.Log(monster_Name + " ���� ü�� = " + monster_Pre_Health);
        monster_Pre_Health -= (damage - this.monster_Armor);
        

        if (monster_Pre_Health <= 0)
        {
            this.Die();
            //Ŭ���� �޼ҵ� ���� ����
        }
    }

    //���� �����ϴ� ���� �ذ� ��ȯ
    protected void Summon_Minion()
    {
        if (target != null && !is_Dead)
        {
            //monster_Audio.clip = monster_Audio_Clips[0];
            //monster_Audio.Play();

            int summon_count = Random.Range(1, 3);

            for(int i = 0; i < summon_count; i++)
            {
                int random_Range = Random.Range(-10, 11);
                Vector2 pos = new Vector2(rb.position.x + random_Range, rb.position.y - 2.3f);

                GameObject newMinion = Instantiate(minion, pos, Quaternion.identity);
            }
        }
    }

    //�� ����
    protected void Scythe_Attack()
    {
        Vector3 diretion = (gameObject.GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right);
        float range = 6f;
        float direction_X = (sp.flipX ? 3f : -3f);
        //��Ÿ� Ȯ�ο�
        Vector3 pos_1 = new Vector3(gameObject.transform.position.x + direction_X, gameObject.transform.position.y - 3f, gameObject.transform.position.z);
        Debug.DrawRay(pos_1, diretion * range, new Color(1, 0, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(pos_1, diretion, range, LayerMask.GetMask("Player"));

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            if (target != null)
            {
                Debug.Log("�÷��̾�");
                target.GetComponent<PlayerController>().Hp(monster_Attack_Damage, boss_Position);
            }
        }
    }

    //�÷��̾ �������Է� �̵�
    public void Player_To_Boss()
    {
        target.GetComponent<Rigidbody2D>().position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 3f);
    }

    //�÷��̾�� ���� ���� ����
    public void Direct_Quiz_Attack()
    {
        GameUI.UIData.QuizUI.quiz(this.gameObject);
    }

    float Get_Animation_Clip_Length(string clipName)
    {
        // AnimatorController���� ��� Ŭ�� ������ ������
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;

        // Animator�� ����� ��� �ִϸ��̼� Ŭ���� ��ȸ
        foreach (AnimationClip clip in ac.animationClips)
        {
            // Ŭ�� �̸��� ��ġ�ϴ��� Ȯ��
            if (clip.name == clipName)
            {
                // Ŭ���� ��� ���̸� ��ȯ
                return clip.length;
            }
        }

        // Ŭ���� ã�� ���ϸ� -1�� ��ȯ
        return -1;
    }

}
