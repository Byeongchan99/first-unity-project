using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        // 공격 여부
        public static bool IsAttack = false;
        // 공격 각도(방향)
        public Vector2 attackDirection;
        // 플레이어 공격 콜라이더
        private PlayerAttackArea playerAttackArea;

        public AttackState(PlayerController controller) : base(controller) {
            playerAttackArea = controller.gameObject.GetComponentInChildren<PlayerAttackArea>();
        }
       
        public override void OnEnterState()
        {
            IsAttack = true;
            attackDirection = Controller.mouseDirection;
            PlayerStat.Instance.animator.SetFloat("AttackDirection.X", attackDirection.x);
            PlayerStat.Instance.animator.SetFloat("AttackDirection.Y", attackDirection.y);

            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            playerAttackArea.ActivateAttackRange(attackDirection);
            PlayerStat.Instance.weaponManager.Weapon?.Attack(this);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            IsAttack = false;
        }
    }
}
