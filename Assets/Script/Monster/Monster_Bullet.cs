using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet : MonoBehaviour
{
    Animator ani;
    SpriteRenderer sp;

    float speed;
    public float damage;
    public Vector2 obj_Position;
    public int direction;

    protected void Awake()
    {
        speed = 10f;

        ani = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Invoke("DestroyBullet", 2f);
        this.Move();

        Vector2 obj = this.gameObject.transform.position;

        RaycastHit2D ray = Physics2D.Raycast(obj, Vector3.down, 1, LayerMask.GetMask("Player"));

        if (ray.collider != null)
        {
            if (ray.collider.tag == "Player")
            {
                ray.collider.GetComponent<PlayerController>().Hp(damage, obj);
                //Debug.Log("ИэСп");
                DestroyBullet();
            }
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetMove(Vector2 obj_Position, int direction)
    {
        this.obj_Position = obj_Position;
        this.direction = direction;

        if(direction < 0)
        {
            sp.flipX = true;
        }
    }
    private void Move()
    {
        transform.Translate(obj_Position  * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
