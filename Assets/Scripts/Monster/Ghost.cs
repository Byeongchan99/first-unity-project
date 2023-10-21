using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonsterBase
{
    public float chargeTime;   // 충전 시간
    public float stunTime;   // 경직 시간

    public int bulletID;   // 사용하는 총알의 프리팹 ID

    public override IEnumerator AttackPattern()
    {
        // 충전
        // 충전 시간 동안 충전 애니메이션 실행
        anim.SetBool("IsCharge", true);

        for (float t = 0; t < chargeTime; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsCharge", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
        anim.SetBool("IsCharge", false);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        // 공격
        // 공격 방향 설정
        attackDirection = (target.position - transform.position).normalized;
        anim.SetBool("IsAttack", true);

        for (float t = 0; t < attackDuration; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        Shoot();
        anim.SetBool("IsAttack", false);

        // 경직
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정                                                                                                 
        anim.SetBool("IsStun", true);   // 경직 애니메이션 실행

        for (float t = 0; t < stunTime; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // 상태 확인 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
    }

    public void Shoot()
    {
        // 발사체 발사 로직
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        MonsterBullet bulletComponent = bulletTransform.GetComponent<MonsterBullet>();

        bulletTransform.position = transform.position;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);

        bulletComponent.Init(1, bulletComponent.originalPer, attackDirection, 1);  // OriginalPer를 사용
    }
}
