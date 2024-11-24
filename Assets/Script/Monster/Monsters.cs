using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    //���� �⺻ ����
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

    //���� �̵� ���� ����
    protected Rigidbody2D rb;
    protected BoxCollider2D box_Collider;
    protected SpriteRenderer sprite_Renderer;
    public int next_Move;
    protected Animator anim;
    
    protected AudioSource monster_Audio;
    public AudioClip[] monster_Audio_Clips;  /*[0]monster damaged [1]monster die ... */

    //���� ���� �ʱ�ȭ
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
        hpBarLogic.maxHp = monster_Max_Health; // �ִ� hp
        hpBarLogic.nowHp = monster_Pre_Health; // ���� hp
        hpBarLogic.owner = this.transform; // ü�¹� ���� ����

        float sfxVolume = PlayerPrefs.GetFloat("MusicVol");

        monster_Audio.volume = sfxVolume;
    }

    //���� �⺻ �̵�
    protected virtual void Move()
    {
       
    }
    protected void Stop()
    {
        rb.velocity = Vector2.zero;
        anim.SetInteger("moveSpeed", 0);
    }

    //���� �̵� ���
    protected virtual void Think()
    {
        next_Move = Random.Range(-1, 2);

        //nextMove ���� ���� �ִϸ��̼��� ����
        anim.SetInteger("moveSpeed", next_Move);

        //�̵� ������ ��� sprite�� ������
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

    //nextMove���� �ݴ�� �ϰ� sprite�� ������
    //Invoke�� Think�� �ʱ�ȭ�ϰ� ����
    protected void Turn()
    {
        next_Move *= -1;
        anim.SetInteger("moveSpeed", next_Move);

        if (next_Move != 0)
            sprite_Renderer.flipX = (next_Move != 1);

        CancelInvoke();
        Invoke("Think", 1.5f);
    }

    //������ ���� ���� ���� ���� �޾� ���� ���� �� ������ ����ŭ ���� ü�� ����
    //���ʹ� ���� ������ �ݴ� �������� �з���
    public virtual void GetDamage(float damage, Vector2 attack_Direction)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();

        if (!is_Dead)
        {
            Debug.Log("�ǰ�");
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

    //0.3�� ���� �ڷ� ������
    protected IEnumerator Push_Back(Vector2 direction, float push_Force)
    {
        Rigidbody2D obj_Rb = gameObject.GetComponent<Rigidbody2D>();
        float elapsedTime = 0f;
        float pushDuration = 0.3f; // �ʱ� �и��� �ð�

        while (elapsedTime < pushDuration)
        {
            elapsedTime += Time.deltaTime;

            // �и� �� ���� ��ġ ���
            Vector2 predictedPosition = obj_Rb.position + new Vector2(direction.x * push_Force * 0.1f, 0);

            // ���� ��ġ �Ʒ��� Platform �±� Ȯ��
            Vector2 rayOrigin = new Vector2(predictedPosition.x, predictedPosition.y - 1f);
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null || !rayHit.collider.CompareTag("Platform"))
            {
                //Debug.Log("���� ��ġ�� Platform ����");
                pushDuration -= 0.1f; // �и��� �ð� ����

                if (pushDuration <= 0f)
                {
                    break; // �и��� �ð��� ��� �����ϸ� ���� ����
                }
            }

            // �������� �б�
            obj_Rb.velocity = new Vector2(direction.x * push_Force, 0);

            yield return null;
        }

        // �̵� ����
        obj_Rb.velocity = Vector2.zero;
    }

    //���� ������ ��� ������ �� ���� ����
    //����Ʈ ������ ���� ü�� ����
    public void Quiz_Mon_Die()
    {
        if(this.quiz_Mon)
        {
            Die();
        } 
    }

    //���Ͱ� �׾��� ��
    protected virtual void Die()
    {
        //is_dead���� true�� ����
        if (!is_Dead)
        {
            is_Dead = true;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;

            //������ ���
            if(!is_Elite)
            {
                GameManager.gameMgr.Drop_Item(gameObject);
            }

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

    public void Object_Destroy()
    {
        Destroy(this.gameObject);
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