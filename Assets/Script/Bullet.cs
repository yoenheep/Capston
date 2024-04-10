using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    public float damage = 5f;

    private Vector3 moveDirection; // 총알의 이동 방향을 저장하는 변수
    void Start()
    {
        Invoke("DestroyBullet", 2);

        // 플레이어가 움직이는 방향을 기준으로 총알의 이동 방향을 설정합니다.
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        transform.right = moveDirection;

    }
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform
            .right, distance, isLayer);
        if(ray.collider != null)
        {
            if(ray.collider.tag == "Monster")
            {
                ray.collider.GetComponent<Monsters>().TakeDamage(damage);
                Debug.Log("명중");
            }
            DestroyBullet();
        }

        if (transform.rotation.y == 0)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
