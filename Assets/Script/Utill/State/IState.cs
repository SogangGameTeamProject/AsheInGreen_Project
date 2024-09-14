using UnityEngine;

namespace AshGreen.State
{
    public interface IState<T>
    {
        // 상태 진입 시 호출
        void Enter(T context);

        // 매 프레임마다 호출
        void StateUpdate();

        // 상태를 종료할 때 호출
        void Exit();
    }
}
