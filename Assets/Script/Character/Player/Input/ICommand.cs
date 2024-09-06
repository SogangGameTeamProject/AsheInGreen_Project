using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public interface ICommand
    {
        void Execute(CharacterController player, params object[] objects);
    }
}
