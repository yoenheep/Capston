using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Move : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private float x_min = 0;
    private float x_max = 0;
    private bool change_Direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        change_Direction = true;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Change_Direction(gameObject.transform.localPosition.x);
        
        if (change_Direction)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    private void Change_Direction(float present_Location)
    {
        if(present_Location >= x_max)
        {
            change_Direction = false;
        } else if (present_Location <= x_min)
        {
            change_Direction = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌이 발생한 객체의 태그가 "Ground"인 경우
        //
        if (collision.gameObject.CompareTag("Ground"))
        {
            
           if(x_max == x_min)
            {
                float platform_x = collision.gameObject.transform.localPosition.x;
                float length = collision.collider.bounds.size.magnitude;
                float obj_Size = gameObject.GetComponent<Collider2D>().bounds.size.x / 2;

                x_max = platform_x + (length / 2) - obj_Size;
                x_min = platform_x - (length / 2) + obj_Size;
            }
        }
    }

    
}
