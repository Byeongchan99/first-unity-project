using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterBase
{
    public float chargeTime;   // ���� �ð�
    public float rushSpeed;   // ���� �ӵ�
    public float rushDuration; // �����ϴ� �ð�
    public float stunTime;   // ���� �ð�

    public override IEnumerator AttackPattern()
    {
        Debug.Log("���� ���� ����");
        // ����
        // ���� �ð� ���� ���� �ִϸ��̼� ����
        anim.SetBool("IsCharge", true);

        for (float t = 0; t < chargeTime; t += Time.deltaTime)
        {
            if (health <= 0)
            {
                anim.SetBool("IsCharge", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

        anim.SetBool("IsCharge", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        // ����
        // ���� ���� ����
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        // �����ϴ� ���� ���� ���� �ݶ��̴� Ȱ��ȭ
        monsterAttackArea.ActivateAttackRange(chargeDirection);
        anim.SetBool("IsAttack", true);
        anim.SetFloat("Direction.X", chargeDirection.x);
        anim.SetFloat("Direction.Y", chargeDirection.y);
        rb.velocity = chargeDirection * rushSpeed;

        for (float t = 0; t < rushDuration; t += Time.deltaTime)
        {
            if (health <= 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ���� �ݶ��̴� ��Ȱ��ȭ
        monsterAttackArea.attackRangeCollider.enabled = false;
        anim.SetBool("IsAttack", false);

        // ����
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����                                                                                                 
        anim.SetBool("IsStun", true);   // ���� �ִϸ��̼� ����

        for (float t = 0; t < stunTime; t += Time.deltaTime)
        {
            if (health <= 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����
    }
}