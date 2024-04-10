using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    public float damage = 5f;

    private Vector3 moveDirection; // �Ѿ��� �̵� ������ �����ϴ� ����
    void Start()
    {
        Invoke("DestroyBullet", 2);

        // �÷��̾ �����̴� ������ �������� �Ѿ��� �̵� ������ �����մϴ�.
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
                Debug.Log("����");
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
