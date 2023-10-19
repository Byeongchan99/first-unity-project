using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonsterBase
{
    public float chargeTime;   // ���� �ð�
    public float stunTime;   // ���� �ð�

    public int bulletID;   // ����ϴ� �Ѿ��� ������ ID

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
        attackDirection = (target.position - transform.position).normalized;
        anim.SetBool("IsAttack", true);
        yield return new WaitForSeconds(attackDuration);
        Shoot();
        anim.SetBool("IsAttack", false);

        // ����
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����                                                                                                 
        anim.SetBool("IsStun", true);   // ���� �ִϸ��̼� ����
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("IsStun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����
    }

    public void Shoot()
    {
        // �߻�ü �߻� ����
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        MonsterBullet bulletComponent = bulletTransform.GetComponent<MonsterBullet>();

        bulletTransform.position = transform.position;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);

        bulletComponent.Init(1, bulletComponent.originalPer, attackDirection, 1);  // OriginalPer�� ���
    }
}
