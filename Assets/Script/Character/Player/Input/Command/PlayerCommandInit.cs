using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public class PlayerCommandInit : MonoBehaviour, ICommand
    {
        public CharacterStateType transitionState = CharacterStateType.Null;//전환할 상태
        public virtual void Execute(CharacterController player)
        {
            //상태 전환
            if(transitionState != CharacterStateType.Null)
                player.StateTransition(transitionState);
        }
    }
}

