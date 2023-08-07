using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        // 공격 여부
        public static bool IsAttack = false;
        // 연속 공격 입력 시간
        public const float CanReInputTime = 1f;
        // 공격 각도(방향)
        public Vector2 attackDirection;

        public AttackState(PlayerController controller) : base(controller) { }
       
        public override void OnEnterState()
        {
            IsAttack = true;
            attackDirection = Controller.mouseDirection;
            PlayerStat.Instance.animator.SetFloat("AttackDirection.X", attackDirection.x);
            PlayerStat.Instance.animator.SetFloat("AttackDirection.Y", attackDirection.y);

            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.weaponManager.Weapon.BeginAttack();
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
