using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last_Boss_Minion : Monsters
{
    public GameObject ammo;
    public GameObject target;

    private bool is_Attacking = false;  // ���� ������ ���θ� üũ�ϴ� ����

    protected void Start()
    {
        monster_Name = "Bone_Archer";
        monster_Armor = 0f;
        monster_Speed = 0f;
        monster_Attack_Damage = 5f;
        monster_Attack_Speed = Random.Range(1.5f, 4f);  // ���� ���� �ð�
        monster_Max_Health = 30f;
        monster_Pre_Health = monster_Max_Health;

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();

        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            target = player.gameObject;
        }
    }

    void FixedUpdate()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false && !is_Attacking)
        {
            StartCoroutine(RangeAttack(monster_Attack_Damage, target));  // ���� �ڷ�ƾ ����
        }
    }

    // ���Ÿ� ���� �ڷ�ƾ
    protected IEnumerator RangeAttack(float damage, GameObject obj)
    {
        if (obj != null && !is_Dead && !is_Attacking)
        {
            is_Attacking = true;  // ���� ������ ����

            monster_Audio.clip = monster_Audio_Clips[2];
            monster_Audio.Play();

            // ���� ����
            Vector2 pos = new Vector2(target.gameObject.transform.position.x, gameObject.transform.position.y + 30f);
            GameObject newBullet = Instantiate(ammo, pos, Quaternion.Euler(0, 0, -90));
            newBullet.GetComponent<Monster_Flame>().SetDamage(damage);

            last_Attack_Time = Time.time;

            yield return new WaitForSeconds(monster_Attack_Speed);  // ���� ���� ���
            is_Attacking = false;  // �ٽ� ���� ���� ���·� ��ȯ
        }
    }

    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        anim.SetTrigger("GetDamage");

        monster_Audio.clip = monster_Audio_Clips[0];
        monster_Audio.Play();

        monster_Pre_Health -= (damage - this.monster_Armor);

        if (monster_Pre_Health <= 0)
        {
            this.Die();
        }
    }

    protected override void Die()
    {
        monster_Audio.clip = monster_Audio_Clips[1];
        monster_Audio.Play();

        hpBar.gameObject.SetActive(false);

        anim.SetBool("isDead", true);
    }
}