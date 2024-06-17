using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursed_Accompanist : Monsters
{
    SpriteRenderer sp;

    float range;
    public GameObject target;
    public GameObject musicNote;
    public GameObject kamikaze_Skull;

    private bool attacking = false;

    private int nextAttack;

    private float note_Damage;

    bool ttt;
    protected void OnEnable()
    {
        monster_Name = "Mini_Boss";
        monster_Armor = 0f;
        monster_Attack_Damage = 5f;
        note_Damage = 5f;
        monster_Attack_Speed = 5f;
        monster_Max_Health = 300f;
        monster_Pre_Health = monster_Max_Health;

        range = 5f;

        sp = gameObject.GetComponent<SpriteRenderer>();

        hpBar = Instantiate(Monster_Spawn_Manager.instance.hpBar_Prefab, transform.position, Quaternion.identity);
        hpBarLogic = hpBar.GetComponent<monHpBar>();
    }

    void Start()
    {
        // PlayerController를 갖고 있는 게임 오브젝트를 찾습니다.
        PlayerController playerController = FindObjectOfType<PlayerController>();

        // playerController가 null이 아닌지 확인합니다.
        if (playerController != null)
        {
            // PlayerController를 갖고 있는 게임 오브젝트를 저장합니다.
            target = playerController.gameObject;
            Debug.Log("PlayerController를 갖고 있는 게임 오브젝트를 찾았습니다: " + target.name);
        }
        else
        {
            Debug.LogError("PlayerController를 갖고 있는 게임 오브젝트를 찾을 수 없습니다.");
        }
    }

    protected override void Think()
    {
        nextAttack = Random.Range(0, 2);
        nextAttack = (nextAttack == 0) ? -1 : 1;

        anim.SetInteger("Attack", nextAttack);
        Debug.Log("실행 " + nextAttack);

        //nextAttack = 1;

        if (nextAttack < 0)
        {
            StartCoroutine(Attack_1());
        } else
        {
            StartCoroutine(Attack_2());
        }
    }

    //음표 공격  
    private IEnumerator Attack_1()
    {
        if(!attacking)
        {
            attacking = true;
            Debug.Log("1번 공격");

            float elapsedTime = 0f;
            float attackDuration = 7f;

            while (elapsedTime < attackDuration)
            {
                //2초에 한 번 음표 공격을 함
                if ((int)elapsedTime % 2 != 0)
                {
                    NoteAttack(note_Damage, target);
                }
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }
            attacking = false;
        }
        Invoke("Think", 5f);
        anim.SetInteger("Attack", 0);
    }

    //자폭 몬스터 소환
    private IEnumerator Attack_2()
    {
        if (!attacking)
        {
            attacking = true;
            Debug.Log("1번 공격");

            float elapsedTime = 0f;
            float attackDuration = 4f;

            while (elapsedTime < attackDuration)
            {
                //2초에 한 번 소환 공격을 함
                if ((int)elapsedTime % 2 != 0)
                {
                    Summon_Kamikaze(target);
                }
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }
            attacking = false;
        }

        Invoke("Think", 5f);
        anim.SetInteger("Attack", 0);
    }

    public override void GetDamage(float damage, Vector2 attack_Direction)
    {
        anim.SetTrigger("GetDamage");

        monster_Audio.clip = monster_Audio_Clips[2];
        monster_Audio.Play();

        monster_Pre_Health -= (damage - this.monster_Armor);

        if (monster_Pre_Health <= 0)
        {
            this.Die();
        }
    }

    protected void NoteAttack(float damage, GameObject obj)
    {
        if (obj != null && !is_dead)
        {
            int direction = sp.flipX ? -1 : 1;
            Vector2 obj_Position = sp.flipX ? Vector2.left : Vector2.left;

            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            GameObject newNote = Instantiate(musicNote, this.gameObject.transform.position, Quaternion.identity);
            newNote.GetComponent<Music_Note>().SetDamage(damage);
            newNote.GetComponent<Music_Note>().SetMove(obj_Position, direction);
        }
    }

    protected void Summon_Kamikaze(GameObject obj)
    {
        if (obj != null && !is_dead)
        {
            int direction = sp.flipX ? -1 : 1;
            Vector2 obj_Position = sp.flipX ? Vector2.left : Vector2.left;

            monster_Audio.clip = monster_Audio_Clips[0];
            monster_Audio.Play();

            GameObject newSkull = Instantiate(kamikaze_Skull, this.gameObject.transform.position, Quaternion.identity);
            newSkull.GetComponent<Kamikaze_Skull>().SetMove(obj_Position, direction);
        }
    }
}
