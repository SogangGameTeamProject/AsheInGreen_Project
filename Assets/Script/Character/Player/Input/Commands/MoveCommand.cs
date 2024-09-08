using UnityEngine;
namespace AshGreen.Character.Player
{
    public class MoveCommand: PlayerCommandInit
    {
        private Vector2 moveVec = Vector2.zero;


        public override void Execute(CharacterController player, params object[] objects)
        {
            base.Execute(player);

            moveVec = (Vector2)objects[0];
            _player._movementController.OnMoveAction?.Invoke(moveVec, _player.MoveSpeed);
        }
    }
}
