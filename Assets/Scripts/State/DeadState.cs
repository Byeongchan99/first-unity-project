using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DeadState : BaseState
    {
        public DeadState(PlayerController controller) : base(controller) { }

        public override void OnEnterState()
        {
            GameManager.instance.isLive = false;
            Dead();
        }

        public void Dead()
        {         
            PlayerStat.Instance.animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
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
