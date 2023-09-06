using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class StateMachine
    {
        // ���� ����
        public BaseState CurrentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        // ���� �߰�
        public void AddState(StateName stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        // ���� ��������
        public BaseState GetState(StateName stateName)
        {
            // C# Dictionary���� �� ��������
            if (states.TryGetValue(stateName, out BaseState state))
                return state;
            return null;
        }

        // ���� ����
        public void DeleteState(StateName removeStateName)
        {
            if (!states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            // CurrentState�� null�� �ƴ� ��� ���� ���� ���� �޼ҵ� ���� - null ���Ǻ� ������(?.)
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            // ���� ���� ���� �޼ҵ� ����
            CurrentState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
    }
}
