using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public class PlayerCommandInit : MonoBehaviour, ICommand
    {
        protected CharacterController _player = null;
        public virtual void Execute(CharacterController player, params object[] objects)
        {
            if(_player == null)
                _player = player;

        }
    }
}

