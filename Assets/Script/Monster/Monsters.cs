using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    protected Animator anim;
    SpriteRenderer spriteRenderer;

    //���� �⺻ ����
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

    //���� �̵� ���� ����
    protected Rigidbody2D rb;
    protected int nextMove;

    protected AudioSource monster_Audio;
    public AudioClip[] monster_Audio_Clips;

    //���� ���� �ʱ�ȭ
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
        hpBarLogic.maxHp = monster_Max_Health; // �ִ� hp
        hpBarLogic.nowHp = monster_Pre_Health; // ���� hp
        hpBarLogic.owner = this.transform; // ü�¹� ���� ����
    }
    void FixedUpdate()
    {
        Move();
    }

    //���� �⺻ �̵�
    protected virtual void Move()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            rb.velocity = new Vector2(monster_Speed * nextMove, rb.velocity.y);

            Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y - gameObject.transform.localScale.y);
            //Ȯ�ο�
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //rayHit�� null�� �� Pause�� �����ؼ� ��� ���߰� Turn�� �����ؼ� �ݴ�� �̵�
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

    //���� �̵� ���
    protected virtual void Think()
    {
        nextMove = Random.Range(-1, 2);
        //nextMove ���� ���� �ִϸ��̼��� ����
        anim.SetInteger("MoveSpeed", nextMove);

        //�̵� ������ ��� nextMove���� 1�� �� sprite�� ������
        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        float nextThinkTime = Random.Range(1f, 5f);
        Invoke("Think", nextThinkTime);
    }

    //�̵� ����
    protected void Pause()
    {
        nextMove = 0;
        anim.SetInteger("MoveSpeed", nextMove);
    }

    //nextMove���� �ݴ�� �ϰ� sprite�� ������
    //Invoke�� Think�� �ʱ�ȭ�ϰ� ����
    protected void Turn()
    {
        nextMove *= -1;
        anim.SetInteger("MoveSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        CancelInvoke();
        Invoke("Think", 0.5f);
    }

    //������ ���� ���� ���� ���� �޾� ���� ���� �� ������ ����ŭ ���� ü�� ����
    //���ʹ� ���� ������ �ݴ� �������� �з���
    public virtual void GetDamage(float damage, Vector2 attack_Direction)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

        //obj_Rb�� null�� �ƴϰ� is_dead�� false�� ��
        if (!is_dead && obj_Rb != null)
        {
            anim.SetTrigger("GetDamage");

            monster_Audio.clip = monster_Audio_Clips[2];
            monster_Audio.Play();

            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float pushForce = 1.5f;

            //���ݹ���� pushForce���� ����Ͽ� ���Ͱ� 1f �̻� �з����� �� ��ǥ ��
            Vector2 pushAmount = direction * Mathf.Max(pushForce, 1f);

            monster_Pre_Health -= (damage - this.monster_Armor);

            // �и� ���� ��ġ ��
            Vector2 newPosition = new Vector2(present_Position.x + pushAmount.x, present_Position.y);

            // ���͸� �и� ���� ��ġ�� �̵�
            Debug.DrawRay(newPosition, Vector3.down, new Color(1, 0, 0));
            obj_Rb.MovePosition(Check_Cliff(newPosition, present_Position));

            //if (GameUI.UIData.answerTrueIcon.activeSelf == true) // �̵��� ���������
            //{
               
            //}

            if (monster_Pre_Health <= 0)
            {
                this.Die();
            }
        }
    }

    //�з��� ��ġ�� �÷��� ���� ��� x���� ����
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
            //������ ������ �����̰ų� ������ ���� �Ŀ� ���� �ð��� ������ ���
            if(Time.time >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                monster_Audio.clip = monster_Audio_Clips[0];
                monster_Audio.Play();

                obj.Hp(monster_Attack_Damage, rb.position);

                lastAttackTime = Time.time;
            } 
        }
    }

    //���Ͱ� �׾��� ��
    protected virtual void Die()
    {
        //is_dead���� true�� ����
        if (!is_dead)
        {
            is_dead = true;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            //������ ���
            GameManager.gameMgr.Drop_Item(gameObject);

            //���� ��� �ִϸ��̼� ���
            anim.SetBool("isDead", true);

            //���� ��� ���� ���
            monster_Audio.clip = monster_Audio_Clips[1];
            monster_Audio.Play();
            // ü�¹�
            hpBar.gameObject.SetActive(false);
            //������ ���� ��ü ����
            StartCoroutine(DeactivateAfterSound());
            //GameUI.UIData.Clear(); ���߿� �������� ����
        }
    }

    private IEnumerator DeactivateAfterSound()
    {
        // ����� Ŭ�� ���̸�ŭ ���
        yield return new WaitForSeconds(monster_Audio.clip.length);

        // ���� ������Ʈ ��Ȱ��ȭ
        Debug.Log("���� ����");
        this.gameObject.SetActive(false);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //���Ͱ� �߶��� ���� ó��
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("����");
            Die();
        }
    }
}