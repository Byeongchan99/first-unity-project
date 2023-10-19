using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonsterBase
{
    public float chargeTime;   // ���� �ð�
    public float rushSpeed;   // ���� �ӵ�
    public float rushDuration; // �����ϴ� �ð�
    public float stunTime;   // ���� �ð�

    public override IEnumerator AttackPattern()
    {
        // ����
        // ���� �ð� ���� ���� �ִϸ��̼� ����
        anim.SetBool("IsCharge", true);
        yield return new WaitForSeconds(chargeTime);
        anim.SetBool("IsCharge", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        // ����
        // ���� ���� ����
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        // �����ϴ� ���� ���� ���� �ݶ��̴� Ȱ��ȭ
        monsterAttackArea.ActivateAttackRange(attackDirection);
        anim.SetBool("IsAttack", true);
        rb.velocity = chargeDirection * rushSpeed;
        yield return new WaitForSeconds(rushDuration);
        // ���� ���� �ݶ��̴� ��Ȱ��ȭ
        monsterAttackArea.attackRangeCollider.enabled = false;
        anim.SetBool("IsAttack", false);

        // ����
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����                                                                                                 
        anim.SetBool("IsStun", true);   // ���� �ִϸ��̼� ����
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����
    }
}
