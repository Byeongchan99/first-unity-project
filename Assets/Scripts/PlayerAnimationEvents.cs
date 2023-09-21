using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerAttackArea playerAttackArea;

    // 공격 애니메이션 끝날 때
    public void FinishedAttack()
    {
        AttackState.IsAttack = false;
        playerAttackArea.attackRangeCollider.enabled = false;
        PlayerStat.Instance.animator.SetBool("IsAttack", false);
        PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    // 공격 애니메이션 중 약간 전진
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
}
