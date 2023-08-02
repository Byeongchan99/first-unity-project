using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public static bool IsAttack = false;
        public const float CanReInputTime = 1f;

        public AttackState(PlayerController controller) : base(controller) { }
       
        public override void OnEnterState()
        {
            IsAttack = true;
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
