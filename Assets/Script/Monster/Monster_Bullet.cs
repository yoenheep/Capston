using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet : MonoBehaviour
{
    Animator ani;

    float speed;
    public float damage;
    public Vector2 obj_Position;
    public Vector2 target_Position;
    public int direction;
    LayerMask layer;

    protected void OnEnable()
    {
        speed = 5f;

        ani = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Invoke("DestroyBullet", 2f);
        this.Move();

        Vector2 obj = this.gameObject.transform.position;
        //Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
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
    }
    private void Move()
    {
        transform.Translate(obj_Position  * direction * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
