using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class OneHandWeapon : BaseWeapon
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackInputCor;

    public override void Attack(BaseState state)
    {
        ComboCount++;
        PlayerStat.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerStat.Instance.animator.SetBool(hashIsAttackAnimation, true);
        PlayerStat.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackInput(AttackState.CanReInputTime);
    }

    // ���� Ȱ��ȭ ����
    public override void BeginAttack()
    {
        gameObject.SetActive(true);
    }

    // ���� Ȱ��ȭ ����
    public override void EndAttack()
    {
        gameObject.SetActive(false);
    }

    public override void ChargingAttack(BaseState state)
    {

    }

    public override void Skill(BaseState state)
    {

    }

    // �÷��̾ ���� ��ư�� ���� �� ȣ��
    // ���ο� ���� ��ư �Է��� ���� ������ �̹� ���� ���� �ڷ�ƾ�� ������Ű�� ���� ����
    public void CheckAttackInput(float inputTime)
    {
        if (checkAttackInputCor != null) {
            StopCoroutine(checkAttackInputCor);
        }
        checkAttackInputCor = StartCoroutine(checkAttackInputCoroutine(inputTime));
    }

    // inputTime ���� �÷��̾ ����ؼ� ���� ��ư�� ���������� ����
    private IEnumerator checkAttackInputCoroutine(float inputTime)
    {
        float currentTime = 0f;
        while (true)
        {
            // inputTime ���� �Է��� ������ �ڷ�ƾ ����
            currentTime += Time.deltaTime;
            if (currentTime >= inputTime)
                break;
            yield return null;
        }
        // �޺� �ʱ�ȭ
        ComboCount = 0;
        PlayerStat.Instance.animator.SetInteger(hashAttackAnimation, 0);
    }
}
