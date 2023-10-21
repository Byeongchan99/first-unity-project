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

        for (float t = 0; t < chargeTime; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsCharge", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }
        anim.SetBool("IsCharge", false);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        // ����
        // ���� ���� ����
        attackDirection = (target.position - transform.position).normalized;
        anim.SetBool("IsAttack", true);

        for (float t = 0; t < attackDuration; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

        Shoot();
        anim.SetBool("IsAttack", false);

        // ����
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����                                                                                                 
        anim.SetBool("IsStun", true);   // ���� �ִϸ��̼� ����

        for (float t = 0; t < stunTime; t += Time.deltaTime)
        {
            if (health < 0)
            {
                anim.SetBool("IsAttack", false);
                yield break; // ���� Ȯ�� �� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }

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
