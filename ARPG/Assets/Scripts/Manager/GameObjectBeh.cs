using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
1待机
2小跑
3大跑
4跳跃
5前闪避
6后闪避
7左闪避
8右闪避
9前受伤
10后受伤
11攻击
12死亡
13结束下落
14切换场景(销毁自己)
*/
public class GameObjectBeh : MonoBehaviour
{
    private float moveSpeed;
    private float rotaSpeed;
    public Animator animator;
    private int nowWeaponId;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        moveSpeed = 30f;
        rotaSpeed = 30f;
    }
    public void SetPosition(Vector3 vector3)
    {
        transform.position = Vector3.Lerp(transform.position, vector3, Time.deltaTime * moveSpeed);
    }
    public void SetRotation(Quaternion quaternion)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * rotaSpeed);
    }
    public void SetWeapon(int id)
    {
        if (nowWeaponId != id)
        {
            animator.SetInteger("weaponId", id);
            animator.SetTrigger("changeWeapon");
        }
        nowWeaponId = id;
    }
    public void SetAnimator(int beh)
    {
        switch (beh)
        {
            case 1:
                animator.SetFloat("x", 0);
                animator.SetFloat("y", 0);
                break;
            case 2:
                animator.SetFloat("x", 0.4f);
                animator.SetFloat("y", 0.4f);
                break;
            case 3:
                animator.SetFloat("x", 1);
                animator.SetFloat("y", 1);
                break;
            case 4:
                animator.Play("Space", 1);
                break;
            case 5:
                animator.Play("Avoid_F", 1);
                break;
            case 6:
                animator.Play("Avoid_B", 1);
                break;
            case 7:
                animator.Play("Avoid_L", 1);
                break;
            case 8:
                animator.Play("Avoid_R", 1);
                break;
            case 9:
                animator.SetTrigger("fHurt");
                break;
            case 10:
                animator.SetTrigger("bHurt");
                break;
            case 11:
                animator.SetTrigger("isAttack");
                break;
            case 12:
                animator.SetInteger("Die", 1);
                break;
            case 13:
                animator.SetTrigger("JumpDown");
                break;
        }
    }
}
