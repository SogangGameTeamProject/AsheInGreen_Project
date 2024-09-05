using UnityEngine;

namespace AshGreen.Character.Player
{
    public class MoveCommand: PlayerCommandInit
    {
        public override void Execute(CharacterController player, params object[] objects)
        {
            base.Execute(player);
            //키입력 예외 처리
            MovementStateType runningState = _player.runningMovementStateType;
            if (runningState != MovementStateType.Idle && runningState != MovementStateType.Jump)
                return;

            Vector2 moveVec = (Vector2)objects[0];

            _player._movementController.OnMove(moveVec, _player.MoveSpeed);
        }
    }
}
