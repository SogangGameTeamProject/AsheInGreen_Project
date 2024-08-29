using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Player
{
    public class PlayerCommandInit : MonoBehaviour, ICommand
    {
        public PlayerStateType transitionState = PlayerStateType.Null;//전환할 상태
        public virtual void Execute(PlayerController player)
        {
            //상태 전환
            if(transitionState != PlayerStateType.Null)
                player.StateTransition(transitionState);
        }
    }
}

