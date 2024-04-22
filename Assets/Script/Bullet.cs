using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if(PlayerController.playerData.pos_gun > 0)
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
        
        transform.Translate(moveDirection * pos* speed * Time.deltaTime);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, moveDirection, distance,isLayer);
        if(ray.collider != null)
        {
            if(ray.collider.tag == "Monster")
            {
                ray.collider.GetComponent<Monsters>().GetDamage(damage, gameObject.transform.position);
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
