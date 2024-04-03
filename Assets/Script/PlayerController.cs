using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float damage = 10f;
    public float Speed;
    private float defaultSpeed;
    private bool isDash;
    public float dashSpeed;
    public float dashCooldown; // 대쉬 쿨타임
    public float dashDuration; // 대쉬 지속 시간
    private float dashTime;
    private float dashCooldownTimer;

    public float JumpPower;

    void Awake()
    {
        defaultSpeed = Speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
        }

        if (Input.GetKeyDown(KeyCode.A) && dashCooldownTimer <= 0)
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
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true; // 왼쪽으로 이동할 때 이미지를 뒤집음
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false; // 오른쪽으로 이동할 때 이미지를 원래대로 돌림
        }
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