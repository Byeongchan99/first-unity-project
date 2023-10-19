using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWarrior : MonsterBase
{
    public override IEnumerator AttackPattern()
    {
        attackDirection = moveDirection;
        anim.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackTiming);  // ���� Ÿ�ֿ̹� ���� ���� �ݶ��̴� Ȱ��ȭ
        monsterAttackArea.ActivateAttackRange(attackDirection);   // ���� ���� Ȱ��ȭ

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // ���� ���� �ݶ��̴� ��Ȱ��ȭ

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        anim.SetBool("IsAttack", false);
    }
}
