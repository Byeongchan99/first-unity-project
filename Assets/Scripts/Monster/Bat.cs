using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonsterBase
{
    public float chargeTime;   // 충전 시간
    public float rushSpeed;   // 돌진 속도
    public float rushDuration; // 돌진하는 시간
    public float stunTime;   // 경직 시간

    public override IEnumerator AttackPattern()
    {
        // 충전
        // 충전 시간 동안 충전 애니메이션 실행
        anim.SetBool("IsCharge", true);
        yield return new WaitForSeconds(chargeTime);
        anim.SetBool("IsCharge", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        // 돌진
        // 돌진 방향 설정
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        // 돌진하는 동안 공격 범위 콜라이더 활성화
        monsterAttackArea.ActivateAttackRange(attackDirection);
        anim.SetBool("IsAttack", true);
        rb.velocity = chargeDirection * rushSpeed;
        yield return new WaitForSeconds(rushDuration);
        // 공격 범위 콜라이더 비활성화
        monsterAttackArea.attackRangeCollider.enabled = false;
        anim.SetBool("IsAttack", false);

        // 경직
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정                                                                                                 
        anim.SetBool("IsStun", true);   // 경직 애니메이션 실행
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
    }
}
