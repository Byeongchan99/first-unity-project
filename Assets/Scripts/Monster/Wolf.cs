using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterBase
{
    public float chargeTime;   // 충전 시간
    public float rushSpeed;   // 돌진 속도
    public float rushDuration; // 돌진하는 시간
    public float stunTime;   // 경직 시간

    public override IEnumerator AttackPattern()
    {
        Debug.Log("공격 패턴 실행");
        // 충전
        // 충전 시간 동안 충전 애니메이션 실행
        anim.SetBool("IsCharge", true);

        for (float t = 0; t < chargeTime; t += Time.deltaTime)
        {
            if (health <= 0)
            {
                anim.SetBool("IsCharge", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        anim.SetBool("IsCharge", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        // 돌진
        // 돌진 방향 설정
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        // 돌진하는 동안 공격 범위 콜라이더 활성화
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
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        // 공격 범위 콜라이더 비활성화
        monsterAttackArea.attackRangeCollider.enabled = false;
        anim.SetBool("IsAttack", false);

        // 경직
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정                                                                                                 
        anim.SetBool("IsStun", true);   // 경직 애니메이션 실행

        for (float t = 0; t < stunTime; t += Time.deltaTime)
        {
            if (health <= 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
    }
}