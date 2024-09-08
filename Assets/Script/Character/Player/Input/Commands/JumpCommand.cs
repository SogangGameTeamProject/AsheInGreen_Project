using UnityEngine;

namespace AshGreen.Character.Player
{
    public class JumpCommand : PlayerCommandInit
    {
        public override void Execute(CharacterController player, params object[] objects)
        {
            base.Execute(player);
            
            _player._movementController.OnJumpAction?.Invoke(_player.JumpPower);
        }
    }
}
