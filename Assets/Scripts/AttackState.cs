using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public bool IsAttack { get; set; }   // 공격 여부
        public float CanReInputTime { get; set; } = 1f;

        public AttackState(PlayerController controller) : base(controller) { }
       
        public override void OnEnterState()
        {
            IsAttack = true;

        }
        public override void OnUpdateState()
        {

        }
        public override void OnFixedUpdateState()
        {

        }
        public override void OnExitState()
        {

        }
    }
}
