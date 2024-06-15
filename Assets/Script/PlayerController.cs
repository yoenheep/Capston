using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ĳ����hp
    public float charac_MaxHP = 100f;
    public float charac_PreHP;
    bool isHurt = false;
    SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    bool isknockback;

    //Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float Speed;
    private float defaultSpeed;
    public bool isDash;
    public float dashSpeed;
    public float dashCooldown; // �뽬 ��Ÿ��
    public float dashDuration; // �뽬 ���� �ð�
    private float dashTime;
    private float dashCooldownTimer;
    private float curTime;
    //public float coolTime = 0.5f;
    public float AttackCoolTime_max;
    public float coolTime = 0.5f;//����������Ÿ��
    public Transform pos;
    public Vector2 BoxSize;
    public float damage;

    //�Ѿ˿���
    public GameObject bullet;
    public Transform pos_bullet;
    private float bullet_curtime;
    //public float bullet_cooltime;
    public float bullet_cooltime;//�Ѿ���Ÿ��
    public float hor;
    public float pos_gun;

    public float JumpPower;

    // ���� ���� ����
    private bool isMeleeActive = true; // ���� ���� Ȱ��ȭ ����
    private bool isRangedActive = false; // ���Ÿ� ���� Ȱ��ȭ ����

    //������� �ӽ��ڵ�
    public int[] weapon_item = new int[2]; // �κ��丮 ���� [0-0 = ���� / 0-1 = ���Ÿ� / 1-0 = ���� / 1-1 = ���Ÿ�]
    public GameObject nearObject;
    bool iDown;
    public bool[] hasWeapons;
    int weaponIndex = -1;
    public int nowWeapon;

    //�ִϸ��̼�
    private Animator animator;

    //�̱���
    public static PlayerController playerData { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerData = this;

        defaultSpeed = Speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        charac_PreHP = charac_MaxHP;
        sr = GetComponent<SpriteRenderer>();

        AttackCoolTime_max = 0.5f;
    }


    void Update()
    {
        GetInput();
        Interation();

        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            if (weapon_item[nowWeapon] == 0 || weapon_item[nowWeapon] == 1 || weapon_item[nowWeapon] == 2) // ����
            {
                AttackCoolTime_max = 0.5f;

                isMeleeActive = true;
                isRangedActive = false;
            }
            else // ���Ÿ�
            {
                AttackCoolTime_max = 0.2f;

                isMeleeActive = false;
                isRangedActive = true;
            }

            // ���� ������ ��ȯ
            if (Input.GetKeyDown(KeyCode.Alpha1)) //1������
            {
                nowWeapon = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && hasWeapons[1] == true) //2������
            {
                nowWeapon = 1;
            }

            //���Ÿ�EX
            if (isRangedActive && bullet_curtime <= 0)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    AttackCoolTime_max = 0.2f;
                    Vector3 bulletDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
                    Quaternion bulletRotation = spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

                    GameObject newBullet = Instantiate(bullet, pos_bullet.position, bulletRotation);
                    newBullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);
                }
                bullet_curtime = AttackCoolTime_max;
            }
            else
            {
                bullet_curtime -= Time.deltaTime;
            }
            //'A'����
            if (isMeleeActive && curTime <= 0)
            {
               
                if (Input.GetKeyDown(KeyCode.A))
                {
                    // ���� ����
                    damage = 10f;
                    AttackCoolTime_max = 0.5f;
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, BoxSize, 0);
                    animator.SetTrigger("StickAttack");

                    foreach (Collider2D collider in collider2Ds)
                    {
                        // ���� ���� �ݶ��̴��� Ʈ���ŷ� �����Ǿ�� �մϴ�.
                        if (collider.CompareTag("Monster"))
                        {
                            // OnTriggerEnter �Լ��� ����Ͽ� ������ �浹�� ó���ϰ� �������� �����մϴ�.
                            collider.GetComponent<Monsters>().GetDamage(damage, pos.position);
                        }
                    }

                    curTime = AttackCoolTime_max;
                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
            {
                rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
            }

            if (Input.GetKeyDown(KeyCode.D) && dashCooldownTimer <= 0)
            {
                isDash = true;
                dashTime = dashDuration;
                dashCooldownTimer = dashCooldown;
                Debug.Log("Dash");
            }

            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    isDash = false;
                }
            }

            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }

            if (isDash)
            {
                defaultSpeed = dashSpeed;
            }
            else
            {
                defaultSpeed = Speed;
            }

            // �̹��� ������ȯ
            hor = Input.GetAxisRaw("Horizontal");
            if (hor < 0)
            {
                spriteRenderer.flipX = true; // �������� �̵��� �� �̹����� ������
                pos.localPosition = new Vector3(-Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos�� �������� �̵�
                pos_gun = -1;
            }
            else if (hor > 0)
            {
                spriteRenderer.flipX = false; // ���������� �̵��� �� �̹����� ������� ����
                pos.localPosition = new Vector3(Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos�� ���������� �̵�
                pos_gun = 1;
            }
        }
    }
    void FixedUpdate()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            float hor = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(hor * defaultSpeed, rigid.velocity.y);
        }
    }

    void GetInput()
    {
        iDown = Input.GetButtonDown("Interation");
    }

    void Interation()
    {
        if (iDown && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                weaponIndex = item.Weapon;
                Destroy(nearObject);
                if (hasWeapons[0] == false)
                {
                    weapon_item[0] = weaponIndex;
                    hasWeapons[0] = true;
                    Debug.Log("first: " + weapon_item[0]);
                }
                else if (hasWeapons[1] == false && hasWeapons[0] == true)
                {
                    weapon_item[1] = weaponIndex;
                    hasWeapons[1] = true;
                    Debug.Log("second: " + weapon_item[1]);
                }
                else if (hasWeapons[1] == true && hasWeapons[0] == true)
                {
                    weapon_item[nowWeapon] = weaponIndex;
                }

            }
            else if (nearObject.tag == "heartItem")
            {
                charac_PreHP += 10;
            }
            else if (nearObject.tag == "mainPortal")
            {
                GameManager.gameMgr.MainNextStage();
            }
            else if(nearObject.tag == "subPortal")
            {
                GameManager.gameMgr.SubNextStage();
            }
            else if(nearObject.tag == "subOutPortal")
            {
                GameManager.gameMgr.SubOutStage();
            }
        }
    }

    public void Hp(float damage, Vector2 pos)
    {
        if (isHurt)
        {
            return; // ���� �Ⱓ ������ ������ ����
        }
        else if (!isHurt)
        {
            isHurt = true; // ���� ����
                           //charac_PreHP -= damage; // ü�� ����
            charac_PreHP -= damage;
            if (charac_PreHP <= 0)
            {
                // ���� ó��
            }
            else
            {
                StartCoroutine(Knockback(transform.position.x - pos.x < 0 ? 1 : -1));
                StartCoroutine(HpRoutine()); // ���� �Ⱓ �ڷ�ƾ ����
                StartCoroutine(alphablink()); // ������ ȿ�� ����
            }
        }


    }
    IEnumerator Knockback(float dir)
    {
        isknockback = true;
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime * dir);
            }
            else
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime * -1f * dir);
            }
            ctime += Time.deltaTime;
            yield return null;
        }
        isknockback = false;
    }

    IEnumerator HpRoutine()
    {
        yield return new WaitForSeconds(3f); // ���� �Ⱓ
        isHurt = false; // ���� ����
    }
    IEnumerator alphablink()
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = halfA;
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, BoxSize);
    }

    bool IsGrounded()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 1.2f, new Color(0, 1, 0));
        //Platform�� ��Ҵ��� Ȯ��
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.2f, LayerMask.GetMask("Platform"));
        //null ������ Ȯ���ؼ� true,false��ȯ null�� �ƴϸ� true,platform�� ����ִ»��¸� false
        return rayHit.collider != null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag == "Weapon")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "mainPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "subPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "subOutPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            //Debug.Log("����");
        }
    }
}