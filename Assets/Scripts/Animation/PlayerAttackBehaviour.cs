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
        if (weapon == null)
        {
            weapon = PlayerStat.Instance.GetComponentInChildren<OneHandSwordBasic>();
        }
        // �ִϸ��̼� ���൵(stateInfo.normalizedTime)�� �������� ������ ����
        if (!hasCheckedInput && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)
        {
            MoveForward();
            CheckAttackInput();
            hasCheckedInput = true;
        }

        if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)
        {
            Debug.Log("�ݶ��̴� ��Ȱ��ȭ " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
            playerAttackArea.attackRangeCollider.enabled = false;
            hasDisabledCollider = true;
        }

        if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)
        {
            hasFinishedAttack = true;
            OnFinishedAttack();
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
