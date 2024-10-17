using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Note : MonoBehaviour
{
    Animator ani;

    float speed;
    public float damage;

    private Vector2 move_Direction;

    
    protected void Awake()
    {
        int rand = Random.Range(0, 4);
        speed = 10f;

        ani = gameObject.GetComponent<Animator>();
        ani.SetInteger("Note", rand);
    }

    private void FixedUpdate()
    {
        if(GameUI.UIData.quizPopup.activeSelf == false)
        {
            Invoke("DestroyBullet", 7f);

            Move();

            Vector2 obj = this.gameObject.transform.position;

            RaycastHit2D ray = Physics2D.Raycast(obj, Vector3.one, 1, LayerMask.GetMask("Player"));

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
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetMove(Vector2 target_Position)
    {
        move_Direction = (target_Position - (Vector2)gameObject.transform.position).normalized;
    }

    private void Move()
    {
        gameObject.transform.position = new Vector2(
         gameObject.transform.position.x + move_Direction.x * speed  * Time.deltaTime,
         gameObject.transform.position.y + move_Direction.y * speed  * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
