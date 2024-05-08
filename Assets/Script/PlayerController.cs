using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //캐릭터hp
    public float charac_MaxHP = 100f;
    public float charac_PreHP;
    bool isHurt = false;
    SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    bool isknockback;

    //Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float Speed;
    private float defaultSpeed;
    private bool isDash;
    public float dashSpeed;
    public float dashCooldown; // 대쉬 쿨타임
    public float dashDuration; // 대쉬 지속 시간
    private float dashTime;
    private float dashCooldownTimer;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 BoxSize;
    public float damage;

    //총알예시
    public GameObject bullet;
    public Transform pos_bullet;
    private float bullet_curtime;
    public float bullet_cooltime;
    public float hor;
    public float pos_gun;

    public float JumpPower;

    // 무기 선택 상태
    private bool isMeleeActive = false; // 근접 무기 활성화 상태
    private bool isRangedActive = false; // 원거리 무기 활성화 상태

    //무기관련 임시코드
    int[] weapon_item = new int[2]; // 인벤토리 참조 [0-0 = 근접 / 0-1 = 원거리 / 1-0 = 근접 / 1-1 = 원거리]
    GameObject nearObject;
    bool iDown;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    int weaponIndex = -1;
    int weapon_Stack = 0; //무기들어온순서

    //싱글톤
    public static PlayerController playerData { get; private set; }

    void Awake()
    {

        playerData = this;

        //animator = GetComponent<Animator>();
        defaultSpeed = Speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        charac_PreHP = charac_MaxHP;
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        GetInput();
        Interation();

        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            // 무기 선택을 전환
            if (Input.GetKeyDown(KeyCode.Alpha1))//근접
            {
                if(weapon_item[0]==0)
                {
                    isMeleeActive = true;
                    isRangedActive = false;
                }
                else if(weapon_item[0]==1)
                {
                    isMeleeActive = false;
                    isRangedActive = true;
                }
                else if(weaponIndex > 0)
                {
                    isMeleeActive = false;
                    isRangedActive = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (weapon_item[1] == 0)
                {
                    isMeleeActive = true;
                    isRangedActive = false;
                }
                else if (weapon_item[1] == 1)
                {
                    isMeleeActive = false;
                    isRangedActive = true;
                }
                else if (weaponIndex > 0)
                {
                    isMeleeActive = false;
                    isRangedActive = false;
                }
            }
            
            //원거리EX
            if (isRangedActive && bullet_curtime <= 0)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 bulletDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
                    Quaternion bulletRotation = spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

                    GameObject newBullet = Instantiate(bullet, pos_bullet.position, bulletRotation);
                    newBullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);
                }
                bullet_curtime = bullet_cooltime;
            }
            else
            {
                bullet_curtime -= Time.deltaTime;
            }
            //'A'어택
            if (isMeleeActive && curTime <= 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    //근접무기
                    damage = 10f;
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, BoxSize, 0);
                    Vector2 collisionPoint = Vector2.zero;

                    foreach (Collider2D collider in collider2Ds)
                    {
                        if (collider.tag == "Monster")
                        {
                            collisionPoint = collider.ClosestPoint(pos.position);
                            collider.GetComponent<Monsters>().GetDamage(damage, collisionPoint);
                        }
                    }

                    //animator.SetTrigger("atk");
                    curTime = coolTime;
                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
            {
                rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
            }

            if (Input.GetKeyDown(KeyCode.D) && dashCooldownTimer <= 0)
            {
                isDash = true;
                dashTime = dashDuration;
                dashCooldownTimer = dashCooldown;
                Debug.Log("Dash");
            }

            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    isDash = false;
                }
            }

            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }

            if (isDash)
            {
                defaultSpeed = dashSpeed;
            }
            else
            {
                defaultSpeed = Speed;
            }

            // 이미지 방향전환
            hor = Input.GetAxisRaw("Horizontal");
            if (hor < 0)
            {
                spriteRenderer.flipX = true; // 왼쪽으로 이동할 때 이미지를 뒤집음
                pos.localPosition = new Vector3(-Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos를 왼쪽으로 이동
                pos_gun = -1;
            }
            else if (hor > 0)
            {
                spriteRenderer.flipX = false; // 오른쪽으로 이동할 때 이미지를 원래대로 돌림
                pos.localPosition = new Vector3(Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos를 오른쪽으로 이동
                pos_gun = 1;
            }
        }
    }
    void FixedUpdate()
    {
        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            float hor = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(hor * defaultSpeed, rigid.velocity.y);
        }
    }

    void GetInput()
    {
        iDown = Input.GetButtonDown("Interation");
    }

    void Interation()
    {
        if(iDown && nearObject != null)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                weaponIndex = item.Weapon;
                hasWeapons[weaponIndex] = true;
                Destroy(nearObject);
                if(weapon_Stack==0)
                {
                    weapon_item[0] = weaponIndex;
                    weapon_Stack++;
                    Debug.Log(weapon_item[0]);
                }
                else if(weapon_Stack==1)
                {
                    weapon_item[1] = weaponIndex;
                    Debug.Log(weapon_item[1]);
                }

                if (weapon_item[0] == 0)
                {
                    isMeleeActive = true;
                    isRangedActive = false;
                }
                else if (weapon_item[0] == 1)
                {
                    isMeleeActive = false;
                    isRangedActive = true;
                }

            }
        }
    }

    public void Hp(float damage, Vector2 pos)
    {
        if (isHurt)
        {
            return; // 무적 기간 동안은 데미지 무시
        } else if(!isHurt)
        {
            isHurt = true; // 무적 시작
                           //charac_PreHP -= damage; // 체력 감소
            charac_PreHP -= damage;
            if (charac_PreHP <= 0)
            {
                // 죽음 처리
            }
            else
            {
                StartCoroutine(Knockback(transform.position.x - pos.x < 0 ? 1 : -1));
                StartCoroutine(HpRoutine()); // 무적 기간 코루틴 시작
                StartCoroutine(alphablink()); // 깜빡임 효과 시작
            }
        }

        
    }
    IEnumerator Knockback(float dir)
    {
        isknockback = true;
        float ctime = 0;
        while (ctime<0.2f)
        {
            if(transform.rotation.y ==0)
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime*dir);
            }
            else
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime * -1f * dir);
            }
            ctime += Time.deltaTime;
            yield return null;
        }
        isknockback = false;
    }

    IEnumerator HpRoutine()
    {
        yield return new WaitForSeconds(3f); // 무적 기간
        isHurt = false; // 무적 해제
    }
    IEnumerator alphablink()
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = halfA;
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, BoxSize);
    }

    bool IsGrounded()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 1.2f, new Color(0, 1, 0));
        //Platform에 닿았는지 확인
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.2f, LayerMask.GetMask("Platform"));
        //null 값인지 확인해서 true,false반환 null이 아니면 true,platform에 닿아있는상태면 false
        return rayHit.collider != null;
    }

    //임시낙사
    void OnCollisionEnter2D(Collision2D collision)
    {

        //몬스터가 추락시 낙사 처리
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("낙사");
            GameUI.UIData.overPopup.SetActive(true);

            gameObject.GetComponent<PlayerController>().enabled = false;
        }
        /*if (collision.collider.CompareTag("Monster"))
        {
            var monster = collision.collider.GetComponentInParent<Monsters>();
            if (monster != null)
            { // `isHurt` 확인
                Debug.Log(monster.monster_Attack_Damage);
                Hp(monster.monster_Attack_Damage, collision.transform.position);
            }
        }*/
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Weapon");
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}