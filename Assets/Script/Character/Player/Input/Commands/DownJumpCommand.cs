using UnityEngine;

namespace AshGreen.Character.Player
{
    public class DownJumpCommand : PlayerCommandInit
    {
        public override void Execute(CharacterController player, params object[] objects)
        {
            base.Execute(player);

            _player._movementController.OnDownJumpAction?.Invoke();
        }
    }
}
