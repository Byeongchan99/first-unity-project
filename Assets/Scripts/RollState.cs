using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public static bool isRoll = false;
        public RollState(PlayerController controller) : base(controller) { }

        public override void OnEnterState()
        {
            isRoll = true;
            PlayerStat.Instance.animator.SetBool("Roll", true);
            PlayerStat.Instance.shadowAnimator.SetBool("Roll", true);
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
