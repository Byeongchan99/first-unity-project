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
        yield return new WaitForSeconds(chargeTime);
        anim.SetBool("IsCharge", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        // 공격
        // 공격 방향 설정
        attackDirection = (target.position - transform.position).normalized;
        anim.SetBool("IsAttack", true);
        yield return new WaitForSeconds(attackDuration);
        Shoot();
        anim.SetBool("IsAttack", false);

        // 경직
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정                                                                                                 
        anim.SetBool("IsStun", true);   // 경직 애니메이션 실행
        yield return new WaitForSeconds(stunTime);
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
