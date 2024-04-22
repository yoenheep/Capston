using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //캐릭터hp
    public float charac_MaxHP = 100f;
    public float charac_PreHP;

    //Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float Speed;
    private float defaultSpeed;
    private bool isDash;
    public float dashSpeed;
    public float dashCooldown; // 대쉬 쿨타임
    public float dashDuration; // 대쉬 지속 시간
    private float dashTime;
    private float dashCooldownTimer;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 BoxSize;
    public float damage;

    //총알예시
    public GameObject bullet;
    public Transform pos_bullet;
    private float bullet_curtime;
    public float bullet_cooltime;
    public float hor;
    public float pos_gun;

    public float JumpPower;

    // 무기 선택 상태
    private bool isMeleeActive = true; // 근접 무기 활성화 상태
    private bool isRangedActive = false; // 원거리 무기 활성화 상태

    //싱글톤
    public static PlayerController playerData { get; private set; }

    void Awake()
    {

        playerData = this;

        //animator = GetComponent<Animator>();
        defaultSpeed = Speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        charac_PreHP = charac_MaxHP;
    }

    void Update()
    {
        // 무기 선택을 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMeleeActive = true;
            isRangedActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMeleeActive = false;
            isRangedActive = true;
        }
        //원거리EX
        if (isRangedActive && bullet_curtime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Vector3 bulletDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
                Quaternion bulletRotation = spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

                GameObject newBullet = Instantiate(bullet, pos_bullet.position, bulletRotation);
                newBullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);
            }
            bullet_curtime = bullet_cooltime;
        }
        else
        {
            bullet_curtime -= Time.deltaTime;
        }
        //'A'어택
        if (isMeleeActive && curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //근접무기
                damage = 10f;
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, BoxSize, 0);
                Vector2 collisionPoint = Vector2.zero;

                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster")
                    {
                        collisionPoint = collider.ClosestPoint(pos.position);
                        collider.GetComponent<Monsters>().GetDamage(damage, collisionPoint);
                    }
                }

                //animator.SetTrigger("atk");
                curTime = coolTime;
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

        // 이미지 방향전환
        hor = Input.GetAxisRaw("Horizontal");
        if (hor < 0)
        {
            spriteRenderer.flipX = true; // 왼쪽으로 이동할 때 이미지를 뒤집음
            pos.localPosition = new Vector3(-Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos를 왼쪽으로 이동
            pos_gun = -1;
        }
        else if (hor > 0)
        {
            spriteRenderer.flipX = false; // 오른쪽으로 이동할 때 이미지를 원래대로 돌림
            pos.localPosition = new Vector3(Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos를 오른쪽으로 이동
            pos_gun = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, BoxSize);
    }

    void FixedUpdate()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(hor * defaultSpeed, rigid.velocity.y);
    }

    bool IsGrounded()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 1.2f, new Color(0, 1, 0));
        //Platform에 닿았는지 확인
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.2f, LayerMask.GetMask("Platform"));
        //null 값인지 확인해서 true,false반환 null이 아니면 true,platform에 닿아있는상태면 false
        return rayHit.collider != null;
    }
}