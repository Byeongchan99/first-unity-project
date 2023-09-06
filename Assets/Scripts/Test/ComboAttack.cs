using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboAttack : MonoBehaviour
{
    Animator animator;
    public int numOfAttack = 0;
    float lastAttackedTime = 0;
    public float maxComboDelay = 1.3f;

    public Transform pos;
    public Vector2 boxSize;

    public PlayerController playerScript;
    public Transform playerTransform;
    public Vector3 mouseDirection;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponentInParent<PlayerController>();
        playerTransform = GetComponentInParent<Transform>();
    }

    void Update()
    {
        // 연속 공격 시간 지났으면 공격 횟수 초기화
        if (Time.time - lastAttackedTime > maxComboDelay)
        {
            numOfAttack = 0;
        }
    }

    // 마우스 좌클릭 시
    void OnAttack()
    {
        mouseDirection = playerScript.mouseDirection;
        animator.SetBool("IsAttack", true);
        lastAttackedTime = Time.time;
        numOfAttack++;
        if (numOfAttack == 1)
        {
            animator.SetInteger("AttackCombo", 1);
            // mouseAngle에 따라 pos.position을 바꿔야 함, 회전은 일단 없이 하게 하기 위해 boxSize는 정사각형
            pos.position = playerTransform.position + mouseDirection;
            Collider2D[] colider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D colider in colider2Ds)
            {
                Debug.Log(colider.tag);
            }
;
        }
        // 최대 최소 범위 설정
        numOfAttack = Mathf.Clamp(numOfAttack, 0, 3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    public void return1()
    {
        if (numOfAttack >= 2)
        {
            animator.SetInteger("AttackCombo", 2);
        }
        else
        {
            animator.SetInteger("AttackCombo", 0);
            numOfAttack = 0;
        }
    }

    public void return2()
    {
        if (numOfAttack >= 3)
        {
            animator.SetInteger("AttackCombo", 3);
        }
        else
        {
            animator.SetInteger("AttackCombo", 0);
            numOfAttack = 0;
        }
    }

    public void return3()
    {
        animator.SetInteger("AttackCombo", 0);
        numOfAttack = 0;
    }
}
