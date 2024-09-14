using System;
using UnityEngine;

namespace AshGreen.State
{
    public class StateContext<T>
    {
        public IState<T> CurrentState { get; private set; }
        private T _context;

        // 상태 변경을 다른 객체에 알리기 위한 이벤트
        public event Action<IState<T>> StateChanged;

        public StateContext(T context)
        {
            _context = context;
        }

        // 시작 상태 설정
        public void Initialize(IState<T> state)
        {
            CurrentState = state;
            state.Enter(_context);

            // 상태 변경 알림
            StateChanged?.Invoke(state);
        }

        // 상태 전환
        public void TransitionTo(IState<T> nextState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = nextState;
            nextState.Enter(_context);

            // 상태 변경 알림
            StateChanged?.Invoke(nextState);
        }

        // 상태 업데이트
        public void StateUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.StateUpdate();
            }
        }
    }
}
