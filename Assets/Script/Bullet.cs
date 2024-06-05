using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    public float damage = 5f;
    public float pos;

    private Vector3 moveDirection; // 총알의 이동 방향을 저장하는 변수
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
        transform.right = direction;

    }
    void Start()
    {
        Invoke("DestroyBullet", 2);
        if (PlayerController.playerData.pos_gun > 0)
        {
            pos = 1;
        }
        else
        {
            pos = -1;
        }
    }
    void Update()
    {
        transform.Translate(moveDirection * pos * speed * Time.deltaTime);

        // 총알의 현재 위치에서 레이를 발사하여 충돌을 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, distance, isLayer);

        // 디버그로 레이를 시각적으로 표시
        Debug.DrawRay(transform.position, moveDirection * distance, Color.green);

        // 레이가 어떤 물체와 충돌했을 때
        if (hit.collider != null)
        {
            // 충돌한 물체가 "Monster" 태그를 가지고 있다면
            if (hit.collider.CompareTag("Monster"))
            {
                // 해당 몬스터에게 피해를 입히고 총알 파괴
                hit.collider.GetComponent<Monsters>().GetDamage(damage, transform.position);
                Debug.Log("명중");
            }
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
