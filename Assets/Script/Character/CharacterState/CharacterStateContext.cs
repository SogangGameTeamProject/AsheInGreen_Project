using System;
using UnityEngine;

namespace AshGreen.Character
{
    public class CharacterStateContext
    {
        public CharacterState CurrentState { get; private set; }
        private CharacterController _character;

        // 상태 변경을 다른 객체에 알리기 위한 이벤트
        public event Action<CharacterState> stateChanged;

        public CharacterStateContext(CharacterController character)
        {
            _character = character;
        }

        // 시작 상태 설정
        public void Initialize(CharacterState state)
        {
            CurrentState = state;
            state.Enter(_character);

            // 상태 변경 알림
            stateChanged?.Invoke(state);
        }

        // 상태 전환
        public void TransitionTo(CharacterState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter(_character);

            // 상태 변경 알림
            stateChanged?.Invoke(nextState);
        }

        public void StateUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.StateUpdate();
            }
        }
    }
}
