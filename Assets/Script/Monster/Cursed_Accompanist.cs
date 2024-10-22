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
        hpBarLogic.maxHp = monster_Max_Health; // �ִ� hp
        hpBarLogic.nowHp = monster_Pre_Health; // ���� hp
        hpBarLogic.owner = this.transform; // ü�¹� ���� ����

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
            if (!attacking) // ���� ���� �ƴ� ������ ��
            {
                wait_Time += Time.deltaTime; // �ð� ����

                if (wait_Time >= 3f) // ���� �ð� ������
                {
                    Debug.Log("think ��������");
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

        Debug.Log("�ٽ� ���ƿ�");
        next_Attack = Random.Range(0, 2);
        next_Attack = (next_Attack == 0) ? -1 : 1;

        anim.SetInteger("attack", next_Attack);
        Debug.Log("���� " + next_Attack);

        //���� ������
        //next_Attack = 1;

        if (next_Attack < 0)
        {
            //��Ʈ ���� ����
            StartCoroutine(Attack_1());
        }
        else
        {
            //�ذ� ��ȯ ���� ����
            StartCoroutine(Attack_2());
        }
    }

    //��ǥ ���� �ڷ�ƾ
    private IEnumerator Attack_1()
    {
        if (!attacking)
        {
            attacking = true;
            Debug.Log("1�� ����");

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
        Debug.Log("Attack_1 ����, Think ȣ��");
        Invoke("Think", monster_Attack_Speed);
        anim.SetInteger("attack", 0);
    }

    //�ذ� ���� �ڷ�ƾ
    private IEnumerator Attack_2()
    {
        if (!attacking)
        {
            attacking = true;
            Debug.Log("2�� ����");

            float elapsed_Time = 0f;
            float attack_Duration = 2f;

            //attackDuration ���� ����
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
        Debug.Log("Attack_2 ����, Think ȣ��");
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

    //��ǥ�� �÷��̾� ��ġ�� ����
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

    //���� �����ϴ� ���� �ذ� ��ȯ
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
            Debug.Log("���� ����");
            is_Dead = true;

            //���� ��� �ִϸ��̼� ���
            anim.SetBool("isDead", true);

            //���� ��� ���� ���
            monster_Audio.clip = monster_Audio_Clips[1];
            monster_Audio.Play();

            // ü�¹�
            hpBar.gameObject.SetActive(false);
            //������ ���� ��ü ����
            //StartCoroutine(DeactivateAfterSound());

            // ����Ʈ���� ����
            Monster_Spawn_Manager.instance.RemoveMonsterFromList(this);
        }
    }
}