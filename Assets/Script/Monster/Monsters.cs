using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;

    //���� �⺻ ����
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

    //���� �̵� ���� ����
    private Rigidbody2D rb;
    private int nextMove;

    //���� ���� �ʱ�ȭ
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        monster_Pre_Health = monster_Max_Health;
        is_dead = false;

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
    protected void Move()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            rb.velocity = new Vector2(monster_Speed * nextMove, rb.velocity.y);

            Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);
            //Ȯ�ο�
            //Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //rayHit�� null�� �� Pause�� �����ؼ� ��� ���߰� Turn�� �����ؼ� �ݴ�� �̵�
            if (rayHit.collider == null)
            {
                Pause();
                Turn();
            }
        }
    }

    //���� �̵� ���
    void Think()
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
    void Pause()
    {
        nextMove = 0;
        anim.SetInteger("MoveSpeed", nextMove);
    }

    //nextMove���� �ݴ�� �ϰ� sprite�� ������
    //Invoke�� Think�� �ʱ�ȭ�ϰ� ����
    void Turn()
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
            Vector2 present_Position = obj_Rb.position;
            Vector2 direction = (present_Position - attack_Direction).normalized;

            float pushForce = 1.5f;

            //���ݹ���� pushForce���� ����Ͽ� ���Ͱ� 1f �̻� �з����� �� ��ǥ ��
            Vector2 pushAmount = direction * Mathf.Max(pushForce, 1f);

            monster_Pre_Health -= (damage - this.monster_Armor);

            // �и� ���� ��ġ ��
            Vector2 newPosition = present_Position + pushAmount;

            // ���͸� �и� ���� ��ġ�� �̵�
            obj_Rb.MovePosition(newPosition);

            if (monster_Pre_Health <= 0)
            {
                Die();
            }
        }
    }

    protected virtual void GiveDamage(float damage, PlayerController obj)
    {
        if(obj != null)
        {
            //������ ������ �����̰ų� ������ ���� �Ŀ� ���� �ð��� ������ ���
            if(lastAttackTime >= lastAttackTime + this.monster_Attack_Speed || lastAttackTime == 0)
            {
                Debug.Log(lastAttackTime);
                obj.charac_PreHP -= damage;
                lastAttackTime = Time.time;
            }
        }
    }

    //���Ͱ� �׾��� ��
    protected virtual void Die()
    {
        //is_dead���� true�� �����ϰ� �浹�� ���̻� �Ͼ�� �ʵ��� Rigidbody�� false�� ����
        if (!is_dead)
        {
            is_dead = true;
            //gameObject.GetComponent<Rigidbody2D>().simulated = false;

            // ü�¹�
            hpBar.gameObject.SetActive(false);
            //������ ���� ��ü ����
            this.gameObject.SetActive(false);
            //GameUI.UIData.Clear(); ���߿� �������� ����
        }
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