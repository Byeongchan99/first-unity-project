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

    // 무기 활성화 시작
    public override void BeginAttack()
    {
        gameObject.SetActive(true);
    }

    // 무기 활성화 종료
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

    // 플레이어가 공격 버튼을 누를 때 호출
    // 새로운 공격 버튼 입력이 들어올 때마다 이미 실행 중인 코루틴을 중지시키고 새로 시작
    public void CheckAttackInput(float inputTime)
    {
        if (checkAttackInputCor != null) {
            StopCoroutine(checkAttackInputCor);
        }
        checkAttackInputCor = StartCoroutine(checkAttackInputCoroutine(inputTime));
    }

    // inputTime 동안 플레이어가 계속해서 공격 버튼을 누르는지를 감지
    private IEnumerator checkAttackInputCoroutine(float inputTime)
    {
        float currentTime = 0f;
        while (true)
        {
            // inputTime 동안 입력이 없으면 코루틴 종료
            currentTime += Time.deltaTime;
            if (currentTime >= inputTime)
                break;
            yield return null;
        }
        // 콤보 초기화
        ComboCount = 0;
        PlayerStat.Instance.animator.SetInteger(hashAttackAnimation, 0);
    }
}
