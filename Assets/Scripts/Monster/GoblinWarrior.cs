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

        for (float t = 0; t < attackDuration; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

        monsterAttackArea.attackRangeCollider.enabled = false;   // ���� ���� �ݶ��̴� ��Ȱ��ȭ

        yield return new WaitForSeconds(totalAttackTime - attackTiming - attackDuration);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        anim.SetBool("IsAttack", false);
    }
}
