using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Flame : MonoBehaviour
{
    float speed;
    public float damage;

    private void Start()
    {
        speed = 20f;
    }
    private void FixedUpdate()
    {
        Invoke("DestroyBullet", 10f);
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

    private void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
