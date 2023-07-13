using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    Animator animator;
    public int numOfAttack = 0;
    float lastAttackedTime = 0;
    public float maxComboDelay = 1.3f;

    public Transform pos;
    public Vector2 boxSize;

    public Player playerScript;
    public Transform playerTransform;
    public Vector3 mouseDirection;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponentInParent<Player>();
        playerTransform = GetComponentInParent<Transform>();
    }

    void Update()
    {
        // ���� ���� �ð� �������� ���� Ƚ�� �ʱ�ȭ
        if (Time.time - lastAttackedTime > maxComboDelay)
        {
            numOfAttack = 0;
        }

        // ���콺 ��Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
        {
            // Player�� Player ��ũ��Ʈ�κ��� ���콺 ���� �޾ƿ���
            mouseDirection = playerScript.mouseDirection;
            animator.SetBool("IsAttack", true);
            lastAttackedTime = Time.time;
            numOfAttack++;
            if (numOfAttack == 1)
            {
                animator.SetBool("Attack1", true);
                // mouseAngle�� ���� pos.position�� �ٲ�� ��, ȸ���� �ϴ� ���� �ϰ� �ϱ� ���� boxSize�� ���簢��
                pos.position = playerTransform.position + mouseDirection;
                Collider2D[] colider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D colider in colider2Ds)
                {
                    Debug.Log(colider.tag);
                }
;            }
            // �ִ� �ּ� ���� ����
            numOfAttack = Mathf.Clamp(numOfAttack, 0, 3);
        }
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
