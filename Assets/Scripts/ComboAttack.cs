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
        // ���� ���� �ð� �������� ���� Ƚ�� �ʱ�ȭ
        if (Time.time - lastAttackedTime > maxComboDelay)
        {
            numOfAttack = 0;
        }
    }

    // ���콺 ��Ŭ�� ��
    void OnAttack()
    {
        mouseDirection = playerScript.mouseDirection;
        animator.SetBool("IsAttack", true);
        lastAttackedTime = Time.time;
        numOfAttack++;
        if (numOfAttack == 1)
        {
            animator.SetInteger("AttackCombo", 1);
            // mouseAngle�� ���� pos.position�� �ٲ�� ��, ȸ���� �ϴ� ���� �ϰ� �ϱ� ���� boxSize�� ���簢��
            pos.position = playerTransform.position + mouseDirection;
            Collider2D[] colider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D colider in colider2Ds)
            {
                Debug.Log(colider.tag);
            }
;
        }
        // �ִ� �ּ� ���� ����
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
