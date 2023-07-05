using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    Animator animator;
    public int numOfAttack = 0;
    float lastAttackedTime = 0;
    public float maxComboDelay = 0.9f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.deltaTime - lastAttackedTime > maxComboDelay)
        {
            numOfAttack = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastAttackedTime = Time.deltaTime;
            numOfAttack++;
            if (numOfAttack == 1)
            {
                animator.SetBool("Attack1", true);
            }
            // 최대 최소 범위 설정
            numOfAttack = Mathf.Clamp(numOfAttack, 0, 3);
        }
    }

    public void return1()
    {
        if (numOfAttack >= 2)
        {
            animator.SetBool("Attack2", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
            numOfAttack = 0;
        }
    }

    public void return2()
    {
        if (numOfAttack >= 3)
        {
            animator.SetBool("Attack3", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            numOfAttack = 0;
        }
    }

    public void return3()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        numOfAttack = 0;
    }
}
