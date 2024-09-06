using UnityEngine;

namespace AshGreen.Character.Player
{
    public class JumpCommand : PlayerCommandInit
    {
        public override void Execute(CharacterController player, params object[] objects)
        {
            base.Execute(player);
            
            _player._movementController.OnPush(Vector2.up, _player.JumpPower);
        }
    }
}
