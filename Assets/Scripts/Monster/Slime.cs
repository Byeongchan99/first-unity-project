using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonsterBase
{
    public override IEnumerator AttackPattern()
    {
        attackDirection = moveDirection;
        anim.SetBool("IsAttack", true);
        anim.SetFloat("Direction.X", attackDirection.x);
        anim.SetFloat("Direction.Y", attackDirection.y);

        yield return new WaitForSeconds(attackTiming);  // 공격 타이밍에 공격 범위 콜라이더 활성화
        monsterAttackArea.ActivateAttackRange(attackDirection);   // 공격 범위 활성화

        for (float t = 0; t < attackDuration; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        monsterAttackArea.attackRangeCollider.enabled = false;   // 공격 범위 콜라이더 비활성화

        yield return new WaitForSeconds(totalAttackTime - attackTiming - attackDuration);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        anim.SetBool("IsAttack", false);
    }
}
