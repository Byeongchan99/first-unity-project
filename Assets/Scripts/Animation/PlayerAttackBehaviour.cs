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

        if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 3)   // �� ��° ���� ����� �� - 1Ÿ�� 2Ÿ�� ����
        {
            // �ִϸ��̼� ���൵(stateInfo.normalizedTime)�� �������� ������ ����
            // �ִϸ��̼� ���൵ 30% ~ 40% ������ ��
            if (!hasDisabledCollider && stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime <= 0.4f)
            {
                MoveForward();
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 1Ÿ ���� ���� �ݶ��̴� ��Ȱ��ȭ
                hasDisabledCollider = true;
            }

            // 3��° ���� ��ǿ����� �߰� �Է� X
            // �ִϸ��̼� ���൵ 40% ~ 50%�� �� 2Ÿ ���� ���� Ȱ��ȭ(triggerPercentage�� ���� 0.4�� ����)
            if (hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)  
            {
                playerAttackArea.ActivateAttackRange(PlayerController.Instance.attackDirection);   // 2Ÿ ���� ���� �ݶ��̴� Ȱ��ȭ
                hasDisabledCollider = false;
            }

            // �ִϸ��̼� ���൵ 50% ~ 60%�� �� 2Ÿ ���� ���� ��Ȱ��ȭ
            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)   
            {
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 2Ÿ ���� ���� �ݶ��̴� ��Ȱ��ȭ
                hasDisabledCollider = true;
            }

            // �ִϸ��̼� ���൵ 95% �̻��� �� ���� ����
            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)   // ����° ������ �ִϸ��̼� = 2��, ��� �ӵ� 2.5�� -> 0.8��
            {
                hasFinishedAttack = true;
                PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.04f));  // (0.8���� 5�� = 0.04)�� ���� �� OnFinishedAttack ȣ��
            }
        }
        else   // ù ��°�� �� ��° ���� ����� ��
        {
            // �ִϸ��̼� ���൵(stateInfo.normalizedTime)�� �������� ������ ����
            // �ִϸ��̼� ���൵ 50% ~ 60% ������ ��(triggerPercentage�� ���� 0.5�� ����)
            if (!hasCheckedInput && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)
            {
                MoveForward();
                CheckAttackInput();
                hasCheckedInput = true;
            }

            // �ִϸ��̼� ���൵ 60% ~ 70% ������ ��
            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)
            {
                // Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;
                hasDisabledCollider = true;
            }

            // �ִϸ��̼� ���൵ 95% �̻��� ��
            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)
            {
                hasFinishedAttack = true;
                // ���� �޺� ī��Ʈ�� ���� ���� �޺� ���� ����
                if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 1)   // ù��° ������ �ִϸ��̼� = 1.5��, ��� �ӵ� 3�� -> 0.5��
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.025f));  // (0.55���� 5�� = 0.025)�� ���� �� OnFinishedAttack ȣ��
                }
                else if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 2)   // �ι�° ������ �ִϸ��̼� = 1��, ��� �ӵ� 2�� -> 0.5��
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.05f));  // (0.5���� 5�� = 0.025)�� ���� �� OnFinishedAttack ȣ��
                }
            }
        }
    }

    // State ���� �� ȣ��Ǵ� �޼���
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �÷��� �ʱ�ȭ
        hasCheckedInput = false;
        hasDisabledCollider = false;
        hasFinishedAttack = false;
        attackReceived = false;
    }

    // �ڷ�ƾ �� �߰� �Է� ���� �� ���� �޺� ���� ����, �Է��� �������� ������ ���� ���� ���� �� �޺� �ʱ�ȭ
    // �÷��̾ ���� ��ư�� ���� �� ȣ��
    public void CheckAttackInput()
    {
        // Debug.Log("CheckAttackInput ����");
        // �ڷ�ƾ �ߺ� ����
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
