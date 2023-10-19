using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWarrior : MonsterBase
{
    public override IEnumerator AttackPattern()
    {
        attackDirection = moveDirection;
        anim.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackTiming);  // 공격 타이밍에 공격 범위 콜라이더 활성화
        monsterAttackArea.ActivateAttackRange(attackDirection);   // 공격 범위 활성화

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // 공격 범위 콜라이더 비활성화

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        anim.SetBool("IsAttack", false);
    }
}
