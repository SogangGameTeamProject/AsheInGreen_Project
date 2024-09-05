using UnityEngine;

namespace AshGreen.Character.Player
{
    public class DownJumpCommand : PlayerCommandInit
    {
        public override void Execute(CharacterController player, params object[] objects)
        {
            //키입력 예외 처리
            MovementStateType runningState = player.runningMovementStateType;
            if (runningState != MovementStateType.Idle && runningState != MovementStateType.Jump)
                return;

            base.Execute(player);
        }
    }
}
