using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    private PlayerController playerController;
    private PlayerAttackArea playerAttackArea;
    [SerializeField] private float triggerPercentage;   // ���൵ ����
    [SerializeField] private float CanReInputTime;   // ���� ���� �Է� �ð�

    private OneHandSwordBasic weapon;
    private Coroutine checkAttackInputCor;

    // ���� �߰� �Է� Ȯ�� �÷���
    bool attackReceived = false;
    // �ڷ�ƾ �ߺ� ȣ�� ����
    private bool hasCheckedInput = false;
    private bool isCoroutineRunning = false;
    // �ݶ��̴� ��Ȱ��ȭ �÷���
    private bool hasDisabledCollider = false;
    // ���� ���� Ȯ�� �÷���
    private bool hasFinishedAttack = false;


    public void SetTriggerPercentage(float value)
    {
        triggerPercentage = value;
    }

    // State ���� �� ȣ��Ǵ� �޼���
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController == null)
        {
            playerController = animator.GetComponent<PlayerController>();
        }
        if (playerAttackArea == null)
        {
            playerAttackArea = animator.gameObject.transform.Find("AttackArea").GetComponent<PlayerAttackArea>();
        }
        // �ٸ� �ʱ�ȭ ������ �ʿ��ϸ� ���⿡ �߰�

    }

    // State ������Ʈ �� ȣ��Ǵ� �޼���
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (Weapon == null)
        {
            weapon = PlayerStat.Instance.GetComponentInChildren<OneHandSwordBasic>();
        }
        */

        if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 3)   // ����° ���� ����� �� - 1Ÿ�� 2Ÿ�� ����
        {
            // �ִϸ��̼� ���൵(stateInfo.normalizedTime)�� �������� ������ ����
            if (!hasDisabledCollider && stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime <= 0.4f)
            {
                MoveForward();
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 1Ÿ ���� ���� �ݶ��̴� ��Ȱ��ȭ
                hasDisabledCollider = true;
            }

            if (hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)   // 3��° ���� ��ǿ����� �߰� �Է� X
            {
                playerAttackArea.ActivateAttackRange(PlayerController.Instance.attackDirection);   // 2Ÿ ���� ���� �ݶ��̴� Ȱ��ȭ
                hasDisabledCollider = false;
            }

            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)   
            {
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 2Ÿ ���� ���� �ݶ��̴� ��Ȱ��ȭ
                hasDisabledCollider = true;
            }

            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)   // ����° ������ �ִϸ��̼� = 2��
            {
                hasFinishedAttack = true;
                PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.1f));  // (2���� 5�� = 0.1)�� ���� �� OnFinishedAttack ȣ��
            }
        }
        else   // ù��°�� �ι�° ���� ����� ��
        {
            // �ִϸ��̼� ���൵(stateInfo.normalizedTime)�� �������� ������ ����
            if (!hasCheckedInput && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)
            {
                MoveForward();
                CheckAttackInput();
                hasCheckedInput = true;
            }

            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)
            {
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;
                hasDisabledCollider = true;
            }

            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)
            {
                hasFinishedAttack = true;
                if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 1)   // ù��°
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.075f));  // (1.5���� 5�� = 0.075)�� ���� �� OnFinishedAttack ȣ��
                }
                else if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 2)   // �ι�°
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.05f));  // (1���� 5�� = 0.05)�� ���� �� OnFinishedAttack ȣ��
                }
            }
        }
    }

    // State ���� �� ȣ��Ǵ� �޼���
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasCheckedInput = false; // Reset the flag
        hasDisabledCollider = false; // Reset the flag
        hasFinishedAttack = false;
        attackReceived = false;
    }

    // �ڷ�ƾ �� �߰� �Է� ���� �� ���� �޺� ���� ����, �Է��� �������� ������ ���� ���� ���� �� �޺� �ʱ�ȭ
    // �÷��̾ ���� ��ư�� ���� �� ȣ��
    public void CheckAttackInput()
    {
        // Debug.Log("CheckAttackInput ����");

        if (isCoroutineRunning)
        {
            // Debug.Log("checkAttackInputCoroutine �̹� ���� ��");
            return;
        } 
            
        if (checkAttackInputCor != null)
        {
            PlayerStat.Instance.StopCoroutine(checkAttackInputCor);
        }

        // Debug.Log("checkAttackInputCoroutine ����");
        checkAttackInputCor = PlayerStat.Instance.StartCoroutine(checkAttackInputCoroutine());
    }

    // CanReInputTime ���� �÷��̾ �߰��� ���� ��ư�� ���������� ����
    private IEnumerator checkAttackInputCoroutine()
    {
        // Debug.Log("checkAttackInputCoroutine() called.");
        float currentTime = 0f;      
        isCoroutineRunning = true;

        while (currentTime < CanReInputTime)
        {
            currentTime += Time.deltaTime;

            // ���⿡�� �߰� ���� �Է��� ����
            if (PlayerController.Instance.OnAttackWasTriggered())
            {
                attackReceived = true;
                break;
            }

            yield return null;
        }

        isCoroutineRunning = false;
    }

    // ������ OnFinishedAttack ȣ���� ���� �ڷ�ƾ
    private IEnumerator DelayedOnFinishedAttack(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ ���� �ð���ŭ ��ٸ�
        OnFinishedAttack(); // ���� �� OnFinishedAttack ȣ��
    }

    private void OnFinishedAttack()
    {
        Debug.Log("���� ����");

        // �߰� �Է� O
        if (attackReceived)
        {
            // ���� ���� ����
            Debug.Log("���� ���� ����");
            PlayerStat.Instance.stateMachine.ChangeState(StateName.ATTACK);
        }
        else   // �߰� �Է� X
        {
            // ���� ���� �ʱ�ȭ
            Debug.Log("���� ���� �ʱ�ȭ");
            PlayerStat.Instance.weaponManager.Weapon.ComboCount = 0;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", 0);
            PlayerStat.Instance.animator.SetBool("IsAttack", false);
            PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
        }
    }

    // ������ �� �ణ �����ϴ� ���
    private void MoveForward()
    {
        // Debug.Log("�ణ ����");
        float advanceDistance = PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance;
        Vector2 targetPos = (Vector2)PlayerStat.Instance.transform.position + playerController.attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");

        RaycastHit2D hit = Physics2D.Raycast(PlayerStat.Instance.transform.position, playerController.attackDirection, PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance, layerMask);

        if (hit.collider == null)
        {
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else
        {
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }
}
