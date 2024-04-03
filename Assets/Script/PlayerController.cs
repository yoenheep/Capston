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
    public float dashCooldown; // �뽬 ��Ÿ��
    public float dashDuration; // �뽬 ���� �ð�
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

        // �̹��� ������ȯ
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true; // �������� �̵��� �� �̹����� ������
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false; // ���������� �̵��� �� �̹����� ������� ����
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
        //Platform�� ��Ҵ��� Ȯ��
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.2f, LayerMask.GetMask("Platform"));
        //null ������ Ȯ���ؼ� true,false��ȯ null�� �ƴϸ� true,platform�� ����ִ»��¸� false
        return rayHit.collider != null;
    }
}