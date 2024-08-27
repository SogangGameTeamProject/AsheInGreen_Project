using System;
using UnityEngine;

namespace AshGreen.Player
{
    public class PlayerStateContext
    {
        public PlayerState CurrentState { get; private set; }
        private PlayerController _player;

        // event to notify other objects of the state change
        public event Action<PlayerState> stateChanged;


        public PlayerStateContext(PlayerController player)
        {
            _player = player;
        }

        // set the starting state
        public void Initialize(PlayerState state)
        {
            CurrentState = state;
            state.Enter(_player);

            // notify other objects that state has changed
            stateChanged?.Invoke(state);
        }

        // exit this state and enter another
        public void TransitionTo(PlayerState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter(_player);

            // notify other objects that state has changed
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
