using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static BackPackManager;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioSource;
    private float hurtTime;
    private float posTime;
    private int hurtSum;//受击次数 主要处理音效播放
    public Transform Ray;//射线检测位置
    [Header("跳跃参数")]
    [SerializeField] public float jumpHeight = 5f; // 跳跃最高高度
    private float gravity = -19.62f; // 重力加速度（负值向下）
    private Vector3 _moveDirection; // 存储移动方向（含垂直速度）
    [SerializeField, Header("当前角色的id")] public int id;
    private static PlayerController instance;
    public static PlayerController Instance => instance;
    private CharacterController cc;
    private Animator animator;
    private bool isW, isS, isA, isD, _isGrounded, isDie;
    public bool isB;
    public bool isShift, nowShift, isSpace, isAtk;
    [SerializeField,Header("武器的挂载点")] public Transform ts;
    [SerializeField, Header("闪避消耗的耐力值")] public int cost = 5;
    public CapsuleCollider cap;
    private GameObject weapon;//当前的武器
    private Collider[] colliders;
    private Transform enemyPosition;
    private float angle;
    public bool isTalk;
    public int nowBeh;
    public List<object> list;
    private bool ground_ok;
    private bool m;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
    }
    void Start()
    {
        nowBeh = 1;
        list = new List<object>();
        char[] a = new char[BackPackManager.Instance.playerData.playerName.Length + (56 - Encoding.UTF8.GetBytes(BackPackManager.Instance.playerData.playerName).Length)];
        print(BackPackManager.Instance.playerData.playerName.Length);
        for (int i = 0; i < BackPackManager.Instance.playerData.playerName.Length; i++) a[i] = BackPackManager.Instance.playerData.playerName[i];
        list.Add(TCPClick.Instance.user_ID);
        list.Add(a);
        list.Add(0);
        list.Add(this.transform.position.x);
        list.Add(this.transform.position.y);
        list.Add(this.transform.position.z);
        list.Add(transform.rotation.x);
        list.Add(transform.rotation.y);
        list.Add(transform.rotation.z);
        list.Add(transform.rotation.w);
        list.Add(0);
        list.Add(0);
        colliders = new Collider[1024];
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }
    

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    m = true;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    m = false;
        //}
        if (isDie) return;
        if (BackPackManager.Instance.playerData.hp == 0)
        {
            isDie = true;
            animator.SetInteger("Die", 1);
            SpawnMenu.Instance.Show();
            return;
        }
        _isGrounded = Physics.Raycast(new Ray(Ray.position, -Ray.up), 0.2f, 1 << LayerMask.NameToLayer("Ground"));
        if (_isGrounded && _moveDirection.y < 0)
        {
            if (ground_ok)
            {
                _moveDirection.y = -2f;
                isSpace = false;
                animator.SetTrigger("JumpDown");
                nowBeh = 13;
                ground_ok = false;
            }
        }
        else if (!_isGrounded) ground_ok = true;
        posTime += Time.deltaTime;
        if (!m && posTime >= 0.1f)
        {
            list[2] = BackPackManager.Instance.playerData.hp;
            list[3] = this.transform.position.x;
            list[4] = this.transform.position.y;
            list[5] = this.transform.position.z;
            list[6] = this.transform.rotation.x;
            list[7] = this.transform.rotation.y;
            list[8] = this.transform.rotation.z;
            list[9] = this.transform.rotation.w;
            list[10] = nowBeh;
            list[11] = animator.GetInteger("weaponId");
            posTime = 0;
            TCPClick.Instance.SendMessage(13, list);
        }
        nowBeh = 1;
        if (hurtSum != 0)
        {
            hurtTime += Time.deltaTime;
            if (hurtTime >= 3f)
            {
                hurtSum = 0;
                hurtTime = 0;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            BackPack.Instance.Show();
            EventSystemManager.Instance.Publish('b');
            isB = true;
        }
        if (isB) return;
        if(Input.GetKeyDown(KeyCode.F) && PlayerStateShow.Instance.NPC_id != 0 && !isTalk)
        {
            isTalk = true;
            TalkMenu.Instance.Show1(PlayerStateShow.Instance.NPC_id);
        }
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    SaveMenu.Instance.Show();
        //}
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !nowShift && !isTalk && !isDie)
        {
            animator.Play("Space", 1);
            isSpace = true;
            //起跳初速度
            _moveDirection.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            nowBeh = 4;
        }
        if (isSpace)
        {
            _moveDirection.y += Time.deltaTime * gravity;
            cc.Move(_moveDirection * Time.deltaTime);
        }
        Collsion();
        if (Input.GetMouseButtonDown(0) && animator.GetInteger("weaponId") != 0 && !isTalk && !isAtk)
        {
            if (enemyPosition != null) this.transform.rotation = Quaternion.LookRotation(enemyPosition.position - transform.position);
            animator.SetTrigger("isAttack");
            nowBeh = 11;
        }
        if(Input.GetAxis("Mouse X") != 0 && !isTalk)
        {
            transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X"), transform.up);
        }
        if (Input.GetKeyDown(KeyCode.W)) isW = true;
        if (Input.GetKeyDown(KeyCode.S)) isS = true;
        if (Input.GetKeyUp(KeyCode.W)) isW = false;
        if (Input.GetKeyUp(KeyCode.S)) isS = false;
        if (!isW && !isS) animator.SetFloat("y", 0);
        if (Input.GetKeyDown(KeyCode.D)) isD = true;
        if (Input.GetKeyDown(KeyCode.A)) isA = true;
        if (Input.GetKeyUp(KeyCode.A)) isA = false;
        if (Input.GetKeyUp(KeyCode.D)) isD = false;
        if (!isA && !isD) animator.SetFloat("x", 0);
        if (!isW && !isS && !isA && !isD && !nowShift) isShift = false;
        if (isW && isS)
        {
            isW = false;
            isS = false;
            animator.SetFloat("y", 0);
        }
        if (isA && isD)
        {
            isA = false;
            isD = false;
            animator.SetFloat("x", 0);
        }
        if (animator.GetInteger("Hurt") == 0 && !nowShift && !isTalk && !isAtk)//没在攻击且没在闪避时才能移动
        {
            if(isA || isW || isD || isS) nowBeh = 2;
            MovePending();
        }

        //处理疾跑,耐力不能为0
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isTalk && !PlayerStateShow.Instance.ok && !nowShift && PlayerStateShow.Instance.shift >= cost && _isGrounded && !isDie)
        {
            nowBeh = 3;
            animator.SetInteger("Hurt", 0);
            nowShift = true;
            isShift = true;
            PlayerStateShow.Instance.shift -= cost;
            if (!isW && !isS && !isA && !isD)
            {
                animator.Play("Avoid_F", 1);
                nowBeh = 5;
            }
            else
            {
                if (isW)
                {
                    if (isA)
                    {
                        animator.Play("Avoid_L", 1);
                        nowBeh = 7;
                    }
                    else if (isD)
                    {
                        animator.Play("Avoid_R", 1);
                        nowBeh = 8;
                    }
                    else
                    {
                        animator.Play("Avoid_F", 1);
                        nowBeh = 5;
                    }
                }
                else if (isS)
                {
                    if (isA)
                    {
                        animator.Play("Avoid_L", 1);
                        nowBeh = 7;
                    }

                    else if (isD)
                    {
                        animator.Play("Avoid_R", 1);
                        nowBeh = 8;
                    }
                    else
                    {
                        animator.Play("Avoid_B", 1);
                        nowBeh = 6;
                    }
                }
                else if (isA)
                {
                    animator.Play("Avoid_L", 1);
                    nowBeh = 7;
                }
                else if (isD)
                {
                    animator.Play("Avoid_R", 1);
                    nowBeh = 8;
                }
            }
            list[2] = BackPackManager.Instance.playerData.hp;
            list[3] = this.transform.position.x;
            list[4] = this.transform.position.y;
            list[5] = this.transform.position.z;
            list[6] = this.transform.rotation.x;
            list[7] = this.transform.rotation.y;
            list[8] = this.transform.rotation.z;
            list[9] = this.transform.rotation.w;
            list[10] = nowBeh;
            list[11] = animator.GetInteger("weaponId");
            posTime = 0;
            TCPClick.Instance.SendMessage(13, list);

        }

    }

    public void ChangeScene()
    {
        list[2] = BackPackManager.Instance.playerData.hp;
        list[3] = this.transform.position.x;
        list[4] = this.transform.position.y;
        list[5] = this.transform.position.z;
        list[6] = this.transform.rotation.x;
        list[7] = this.transform.rotation.y;
        list[8] = this.transform.rotation.z;
        list[9] = this.transform.rotation.w;
        list[10] = 14;
        list[11] = animator.GetInteger("weaponId");
        posTime = 0;
        TCPClick.Instance.SendMessage(13, list);
    }

    public void NoDie()
    {
        cc.enabled = false;
        transform.position = CameraController.Instance.pos;
        transform.rotation = Quaternion.identity;
        cc.enabled = true;
        animator.SetInteger("Die", 0);
        isDie = false;
    }

    public void SpaceExit()
    {
        isSpace = false;
    }


    //处理范围碰撞逻辑
    public void Collsion()
    {

        angle = 500f;
        float a;
        enemyPosition = null;
        int num = Physics.OverlapSphereNonAlloc(transform.position + transform.up, 10, colliders, 1 << LayerMask.NameToLayer("Enemy"));
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                AiEnemy ai = colliders[i].GetComponent<AiEnemy>();
                if (ai.Check()) continue;
                a = Mathf.Acos(Vector3.Dot(Vector3.Normalize(transform.position), Vector3.Normalize(colliders[i].transform.position)));
                if (a < angle)
                {
                    angle = a;
                    enemyPosition = colliders[i].transform;
                }
            }
        }
        CameraController.Instance.Enemy(enemyPosition);
    }

    //闪避退出逻辑
    public void OnShiftExit()
    {
        nowShift = false;
    }
    //设置当前武器id
    public void SetWeapon(int id, int damage, int per)
    {
        if (weapon != null)
            weapon.SetActive(false);
        animator.SetInteger("weaponId", id);
        weapon = ts.Find(id.ToString()).gameObject;
        if (id == 2) cap = weapon.transform.Find("default").GetComponent<CapsuleCollider>();
        else cap = weapon.GetComponent<CapsuleCollider>();
        weapon.SetActive(true);
        BackPackManager.Instance.weapon = weapon.GetComponent<Weapon>();
        animator.SetTrigger("changeWeapon");
        BackPackManager.Instance.playerData.damage = damage;
        BackPackManager.Instance.playerData.piercing = per;
        Weapon w = weapon.GetComponent<Weapon>();
        w.damage = damage;
        w.value = per;
    }

    public void HurtQuit()
    {
        animator.SetInteger("Hurt", 0);
    }

    //提供方法给动画处理退出逻辑
    public void QuitMove()
    {
        isD = false;
        isA = false;
        isS = false;
        isW = false;
        animator.SetFloat("x", 0);
        animator.SetFloat("y", 0);
    }
    //处理移动逻辑
    private void MovePending()
    {
        Vector3 vector3 = new Vector3();
        //处理移动
        if (isW)
        {
            if (!isShift)//在前进但没疾跑
            {
                if (isA || isD)
                {
                    vector3 += transform.forward.normalized * 2f;
                    animator.SetFloat("y", 0.25f);
                }
                else
                {
                    vector3 += transform.forward.normalized * 4f;
                    animator.SetFloat("y", 0.5f);
                }
            }
            else
            {
                if (isA || isD)
                {
                    vector3 += transform.forward.normalized * 4f;
                    animator.SetFloat("y", 0.5f);
                }
                else
                {
                    vector3 += transform.forward.normalized * 8f;
                    animator.SetFloat("y", 1f);
                }
            }
        }
        if(isS)
        {
            if (!isShift)//在后退但没疾跑
            {
                if (isA || isD)
                {
                    vector3 += -transform.forward.normalized * 2f;
                    animator.SetFloat("y", 0.25f);
                }
                else
                {
                    vector3 += -transform.forward.normalized * 4f;
                    animator.SetFloat("y", 0.5f);
                }
            }
            else
            {
                if (isA || isD)
                {
                    vector3 += -transform.forward.normalized * 4f;
                    animator.SetFloat("y", 0.5f);
                }
                else
                {
                    vector3 += -transform.forward.normalized * 8f;
                    animator.SetFloat("y", 1f);
                }
            }
        }
        if(isA)
        {
            if (!isShift)//在向左走但没疾跑
            {
                if (isW || isS)
                {
                    vector3 += -transform.right.normalized * 2f;
                    animator.SetFloat("x", 0.25f);
                }
                else
                {
                    vector3 += -transform.right.normalized * 4f;
                    animator.SetFloat("x", 0.5f);
                }
            }
            else
            {
                if (isW || isS)
                {
                    vector3 += -transform.right.normalized * 4f;
                    animator.SetFloat("x", 0.5f);
                }
                else
                {
                    vector3 += -transform.right.normalized * 8f;
                    animator.SetFloat("x", 1f);
                }
            }
        }
        if (isD)
        {
            if (!isShift)//在向右走但没疾跑
            {
                if (isW || isS)
                {
                    vector3 += transform.right.normalized * 2f;
                    animator.SetFloat("x", 0.25f);
                }
                else
                {
                    vector3 += transform.right.normalized * 4f;
                    animator.SetFloat("x", 0.5f);
                }
            }
            else
            {
                if (isW || isS)
                {
                    vector3 += transform.right.normalized * 4f;
                    animator.SetFloat("x", 0.5f);
                }
                else
                {
                    vector3 += transform.right.normalized * 8f;
                    animator.SetFloat("x", 1f);
                }
            }
        }
        cc.SimpleMove(vector3);

        
    }

    //刚进入动画
    public void OnAnimatorStart()
    {
        isAtk = true;
    }
    //刚结束动画
    public void OnAnimatorEnd()
    {
        isAtk = false;
    }

    //动画击中事件
    public void OnAnimationAttackEvent()
    {
        cap.enabled = true;
    }
    public void OnAnimationAttackEventQuit()
    {
        cap.enabled = false;
    }
    //处理受伤逻辑
    public void Hurt(Transform ts,int damage,int id)
    {
        if (this.id != id)
        {
            if (isShift) return;
            switch (hurtSum)
            {
                case 0:
                    audioSource.clip = GameData.Instance.hit0;
                    break;
                case 1:
                    audioSource.clip = GameData.Instance.hit1;
                    break;
                case 2:
                    audioSource.clip = GameData.Instance.hit2;
                    break;
                case 3:
                    audioSource.clip = GameData.Instance.hit3;
                    hurtSum = -1;
                    break;
            }
            hurtSum++;
            audioSource.Play();
            BackPackManager.Instance.playerData.hp -= damage;
            animator.SetInteger("Hurt", 1);
            if (Vector3.Dot(transform.position, ts.position) > 0) animator.SetTrigger("fHurt");
            else animator.SetTrigger("bHurt");
        }
    }

    public void ShowW00()
    {
        audioSource.clip = GameData.Instance.w0_0;
        audioSource.Play();
    }
    public void ShowW01()
    {
        audioSource.clip = GameData.Instance.w0_1;
        audioSource.Play();
    }
    public void ShowW02()
    {
        audioSource.clip = GameData.Instance.w0_2;
        audioSource.Play();
    }
    public void ShowW03()
    {
        audioSource.clip = GameData.Instance.w0_3;
        audioSource.Play();
    }
    public void ShowW04()
    {
        audioSource.clip = GameData.Instance.w0_4;
        audioSource.Play();
    }

    public void ShowW10()
    {
        audioSource.clip = GameData.Instance.w1_0;
        audioSource.Play();
    }
    public void ShowW11()
    {
        audioSource.clip = GameData.Instance.w1_1;
        audioSource.Play();
    }
    public void ShowW12()
    {
        audioSource.clip = GameData.Instance.w1_2;
        audioSource.Play();
    }

    public void OnAnimationExit()
    {

    }
}
