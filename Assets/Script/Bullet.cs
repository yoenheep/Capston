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

    private Vector3 moveDirection; // �Ѿ��� �̵� ������ �����ϴ� ����
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

        // �Ѿ��� ���� ��ġ���� ���̸� �߻��Ͽ� �浹�� ����
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, distance, isLayer);

        // ����׷� ���̸� �ð������� ǥ��
        Debug.DrawRay(transform.position, moveDirection * distance, Color.green);

        // ���̰� � ��ü�� �浹���� ��
        if (hit.collider != null)
        {
            // �浹�� ��ü�� "Monster" �±׸� ������ �ִٸ�
            if (hit.collider.CompareTag("Monster"))
            {
                // �ش� ���Ϳ��� ���ظ� ������ �Ѿ� �ı�
                hit.collider.GetComponent<Monsters>().GetDamage(damage, transform.position);
                Debug.Log("����");
            }
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
