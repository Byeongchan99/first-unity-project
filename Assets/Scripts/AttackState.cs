using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        // ���� ����
        public static bool IsAttack = false;
        // ���� ���� �Է� �ð�
        public const float CanReInputTime = 1f;
        // ���� ����(����)
        public Vector2 attackDirection;
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
            PlayerStat.Instance.weaponManager.Weapon.BeginAttack();
            playerAttackArea.ActivateAttackRange(attackDirection, PlayerStat.Instance.weaponManager.Weapon.AttackRange);
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
            PlayerStat.Instance.weaponManager.Weapon.EndAttack();
        }
    }
}
