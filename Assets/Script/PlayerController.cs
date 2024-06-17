using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //캐릭터hp
    public float charac_MaxHP = 5f;
    public float charac_PreHP;
    public bool isHurt = false;
    private bool isReInvoked = false;
    public bool isDead = false;

    //피격
    //SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    bool isknockback;

    //Animator animator;
    Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public float Speed;
    private float defaultSpeed;
    public bool isDash;
    public float dashSpeed;
    public float dashCooldown; // 대쉬 쿨타임
    public float dashDuration; // 대쉬 지속 시간
    private float dashTime;
    private float dashCooldownTimer;
    private float curTime;
    //public float coolTime = 0.5f;
    public float AttackCoolTime_max;
    public float coolTime = 0.5f;//근접무기쿨타임
    public Transform pos;
    public Vector2 BoxSize;
    public float damage;

    //총알예시
    public GameObject bullet;
    public Transform pos_bullet;
    private float bullet_curtime;
    //public float bullet_cooltime;
    public float bullet_cooltime;//총알쿨타임
    public float hor;
    public float pos_gun;

    public float JumpPower;

    // 무기 선택 상태
    private bool isMeleeActive = true; // 근접 무기 활성화 상태
    private bool isRangedActive = false; // 원거리 무기 활성화 상태

    //무기관련 임시코드
    public int[] weapon_item = new int[2]; // 인벤토리 참조 [-1 = 물약 / 0 = 지팡이 / 1 = 도끼 / 2 = 칼 / 3 = 총 / 4 = 마법봉]
    public GameObject nearObject;
    bool iDown;
    public bool[] hasWeapons;
    int weaponIndex = -1;
    public int nowWeapon;

    // 오디오
    AudioSource audioSource;
    public AudioClip pickUpItem;
    public AudioClip magicAttack;
    public AudioClip a_Weapon;
    
    //애니메이션
    private Animator animator;

    //싱글톤
    public static PlayerController playerData { get; private set; }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerData = this;

        defaultSpeed = Speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        charac_PreHP = charac_MaxHP;
        //sr = GetComponent<SpriteRenderer>();

        AttackCoolTime_max = 0.5f;
    }


    void Update()
    {
        GetInput();
        Interation();

        if (GameUI.UIData.quizPopup.activeSelf == false)
        {
            if (weapon_item[nowWeapon] == 0 || weapon_item[nowWeapon] == 1 || weapon_item[nowWeapon] == 2) // 근접
            {
                AttackCoolTime_max = 0.5f;

                isMeleeActive = true;
                isRangedActive = false;
            }
            else // 원거리
            {
                AttackCoolTime_max = 0.2f;

                isMeleeActive = false;
                isRangedActive = true;
            }

            // 무기 선택을 전환
            if (Input.GetKeyDown(KeyCode.Alpha1)) //1번무기
            {
                nowWeapon = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && hasWeapons[1] == true) //2번무기
            {
                nowWeapon = 1;
            }

            //원거리EX
            if (isRangedActive && bullet_curtime <= 0)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    AttackCoolTime_max = 0.2f;
                    Vector3 bulletDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
                    Quaternion bulletRotation = spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

                    GameObject newBullet = Instantiate(bullet, pos_bullet.position, bulletRotation);
                    newBullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);

                    if (weapon_item[nowWeapon] == 4)
                    {
                        audioSource.clip = magicAttack;
                        audioSource.Play();
                    }
                }
                bullet_curtime = AttackCoolTime_max;
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
                    // 근접 무기
                    if (weapon_item[nowWeapon]==0)
                    {
                        audioSource.clip = a_Weapon;
                        audioSource.Play();
                    }
                    damage = 10f;
                    AttackCoolTime_max = 0.5f;
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, BoxSize, 0);
                    animator.SetTrigger("StickAttack");

                    foreach (Collider2D collider in collider2Ds)
                    {
                        // 이제 몬스터 콜라이더는 트리거로 설정되어야 합니다.
                        if (collider.CompareTag("Monster"))
                        {
                            // OnTriggerEnter 함수를 사용하여 몬스터의 충돌을 처리하고 데미지를 적용합니다.
                            collider.GetComponent<Monsters>().GetDamage(damage, pos.position);
                        }
                    }

                    curTime = AttackCoolTime_max;
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
                animator.SetBool("Walk", true);
            }
            else if (hor > 0)
            {
                spriteRenderer.flipX = false; // 오른쪽으로 이동할 때 이미지를 원래대로 돌림
                pos.localPosition = new Vector3(Mathf.Abs(pos.localPosition.x), pos.localPosition.y, pos.localPosition.z); // pos를 오른쪽으로 이동
                pos_gun = 1;
                animator.SetBool("Walk", true);
            }
            else if (hor == 0)
            {
                animator.SetBool("Walk", false);
            }
        }

        if (charac_PreHP <= 0)
        {
            animator.SetBool("Death", true);
        }
        else
        {
            animator.SetBool("Death", false);
            isDead = false;
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
        if (iDown && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                weaponIndex = item.Weapon;
                audioSource.clip = pickUpItem;
                audioSource.Play();
                Destroy(nearObject);

                if (weaponIndex != -1)
                {
                    if (hasWeapons[0] == false)
                    {
                        weapon_item[0] = weaponIndex;
                        hasWeapons[0] = true;
                        Debug.Log("first: " + weapon_item[0]);
                    }
                    else if (hasWeapons[1] == false && hasWeapons[0] == true)
                    {
                        weapon_item[1] = weaponIndex;
                        hasWeapons[1] = true;
                        Debug.Log("second: " + weapon_item[1]);
                    }
                    else if (hasWeapons[1] == true && hasWeapons[0] == true)
                    {
                        weapon_item[nowWeapon] = weaponIndex;
                    }
                }
                else
                    charac_PreHP += 10;
            }
            else if (nearObject.tag == "mainPortal")
            {
                GameManager.gameMgr.MainNextStage();
                nearObject = null;
            }
            else if(nearObject.tag == "subPortal")
            {
                GameManager.gameMgr.SubNextStage();
                nearObject = null;
            }
            else if(nearObject.tag == "subOutPortal")
            {
                GameManager.gameMgr.SubOutStage();
                nearObject = null;
            }
        }
    }

    public void Hp(float damage, Vector2 pos)
    {
        if (isHurt)
        {
            return; // 무적 기간 동안은 데미지 무시
        }
        else if (!isHurt)
        {
            isHurt = true; // 무적 시작
                           //charac_PreHP -= damage; // 체력 감소
            charac_PreHP -= damage;
            if (charac_PreHP <= 0)
            {
                animator.SetBool("Death",true);
            }
            else
            {
                animator.SetBool("Death", false);
                isDead = false;
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
        while (ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime * dir);
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
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;
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

    void Re()
    {
        charac_PreHP -= 20;
        GameUI.UIData.ReGame();
        isReInvoked = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag == "Weapon")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "mainPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "subPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "subOutPortal")
        {
            nearObject = collision.gameObject;
            Debug.Log(nearObject);
        }
        else if (collision.tag == "DeadZone" && !isReInvoked)
        {
            isReInvoked = true;
            Invoke("Re", 1);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            //Debug.Log("접촉");
        }
    }
}