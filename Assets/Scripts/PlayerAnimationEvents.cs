using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using Unity.VisualScripting;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerAttackArea playerAttackArea;
    private Coroutine checkAttackInputCor;
    // ���� ���� �Է� �ð�
    public const float CanReInputTime = 1f;

    // �ڷ�ƾ �� �߰� �Է� ���� �� ���� �޺� ���� ����, �Է��� �������� ������ ���� ���� ���� �� �޺� �ʱ�ȭ
    // �÷��̾ ���� ��ư�� ���� �� ȣ��
    public void CheckAttackInput()
    {
        Debug.Log("CheckAttackInput() called.");
        // ���� �ִϸ��̼� �߰��� �ڷ�ƾ ����
        if (checkAttackInputCor != null)
        {
            StopCoroutine(checkAttackInputCor);
        }
        checkAttackInputCor = StartCoroutine(checkAttackInputCoroutine());
    }

    // CanReInputTime ���� �÷��̾ �߰��� ���� ��ư�� ���������� ����
    private IEnumerator checkAttackInputCoroutine()
    {
        Debug.Log("checkAttackInputCoroutine() called.");
        float currentTime = 0f;
        bool attackReceived = false;

        while (currentTime < CanReInputTime)
        {
            currentTime += Time.deltaTime;

            // ���⿡�� �߰� ���� �Է��� ����
            if (playerController.OnAttackWasTriggered())
            {
                attackReceived = true;
                break;
            }

            yield return null;
        }

        if (attackReceived)
        {
            // ���� ���� ����
            // ���⿡�� ComboCount�� ������Ű�� ���� ���� ���� �ִϸ��̼��� �����մϴ�.
            PlayerStat.Instance.weaponManager.Weapon.ComboCount++;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", PlayerStat.Instance.weaponManager.Weapon.ComboCount);
        }
        else
        {
            // ���� ���� �ʱ�ȭ
            PlayerStat.Instance.weaponManager.Weapon.ComboCount = 0;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", 0);
            FinishedAttack();  // ���� ���� ó��
        }
    }

    // ���� �ִϸ��̼� ���� ��
    public void FinishedAttack()
    {
        playerAttackArea.attackRangeCollider.enabled = false;
    }

    // ���� �ִϸ��̼� �� �ణ ����
    public void MoveForward()
    {
        float advanceDistance = PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance;
        Vector2 targetPos = (Vector2)PlayerStat.Instance.transform.position + playerController.attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");
       
        RaycastHit2D hit = Physics2D.Raycast(PlayerStat.Instance.transform.position, playerController.attackDirection, PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance, layerMask);

        if (hit.collider == null)
        {
            // Debug.Log("No object detected, moving to target position.");
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else
        {
            // Debug.Log("Object hit by Raycast: " + hit.collider.gameObject.name);
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }

    private Coroutine rollCoolTimeCoroutine;

    public void OnFinishedRoll()
    {
        if (RollState.inputVecBuffer.Count > 0)
        {
            PlayerStat.Instance.stateMachine.ChangeState(StateName.ROLL);
            return;
        }

        RollState.CanAddInputBuffer = false;

        if (rollCoolTimeCoroutine != null)
            StopCoroutine(rollCoolTimeCoroutine);
        rollCoolTimeCoroutine = StartCoroutine(RollCooltimeTimer(PlayerStat.Instance.RollCooltime));
    }

    private IEnumerator RollCooltimeTimer(float coolTime)
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > coolTime)
            {
                RollState.IsRoll = false;
                PlayerStat.Instance.animator.SetBool("IsRoll", false);
                PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", false);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
        }

        yield return null;
    }
}
