using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class ChargeState : BaseState
    {
        // ���� ����
        public static bool IsCharge = false;
        // ���� ����(����)
        public Vector2 attackDirection;

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
           
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
