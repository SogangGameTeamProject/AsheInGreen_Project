using UnityEngine;

namespace AshGreen.Player
{
    public interface PlayerState
    {
        public void Enter(PlayerController palyer, params object[] parameters)
        {
            // code that runs when we first enter the state
        }

        public void StateUpdate()
        {
            // per-frame logic, include condition to transition to a new state
        }

        public void Exit()
        {
            // code that runs when we exit the state
        }
    }
}
