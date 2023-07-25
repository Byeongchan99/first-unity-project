using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class StateMachine
    {
        // 현재 상태
        public BaseState CurrentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        // 상태 추가
        public void AddState(StateName stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        // 상태 가져오기
        public BaseState GetState(StateName stateName)
        {
            // C# Dictionary에서 값 가져오기
            if (states.TryGetValue(stateName, out BaseState state))
                return state;
            return null;
        }

        // 상태 삭제
        public void DeleteState(StateName removeStateName)
        {
            if (!states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            // CurrentState가 null인 아닐 경우 현재 상태 종료 메소드 실행 - null 조건부 연산자(?.)
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            // 다음 상태 진입 메소드 실행
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
