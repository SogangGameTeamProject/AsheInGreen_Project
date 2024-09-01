using UnityEngine;

namespace AshGreen.Character.Player
{
    public class DownJumpCommand : PlayerCommandInit
    {
        public override void Execute(CharacterController player)
        {
            //키입력 예외 처리
            CharacterStateType runningState = player.runningStateType;
            if (runningState != CharacterStateType.Idle && runningState != CharacterStateType.Jump)
                return;

            base.Execute(player);
        }
    }
}
