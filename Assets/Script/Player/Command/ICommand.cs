using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Player
{
    public interface ICommand
    {
        void Execute(PlayerController player);
    }
}
