using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze_Skull : MonoBehaviour
{
    SpriteRenderer sp;

    float speed;
    public Vector2 move_Direction;

    protected void OnEnable()
    {
        speed = 7.5f;

        sp = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(GameUI.UIData.quizPopup.activeSelf == false)
        {
            Invoke("DestroyBullet", 7f);
            this.Move();

            Vector2 obj = this.gameObject.transform.position;

            RaycastHit2D ray = Physics2D.Raycast(obj, Vector3.down, 1, LayerMask.GetMask("Player"));

            if (ray.collider != null)
            {
                if (ray.collider.tag == "Player")
                {
                    GameUI.UIData.QuizUI.quiz(this.gameObject);
                    //Debug.Log("ИэСп");
                    DestroyBullet();
                }
            }
        } else
        {
            CancelInvoke();
        }
    }

    public void SetMove(Vector2 target_Position)
    {
        move_Direction = (target_Position - (Vector2)gameObject.transform.position).normalized;
        sp.flipX = (move_Direction.x < 0);
    }

    private void Move()
    {
        gameObject.transform.position = new Vector2(
         gameObject.transform.position.x + move_Direction.x * speed * Time.deltaTime, 
         gameObject.transform.position.y);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
