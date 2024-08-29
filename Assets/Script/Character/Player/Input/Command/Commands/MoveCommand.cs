using UnityEngine;

namespace AshGreen.Player
{
    public class MoveCommand: PlayerCommandInit
    {
        public override void Execute(PlayerController player)
        {
            //키입력 예외 처리
            PlayerStateType runningState = player.runningStateType;
            if (runningState != PlayerStateType.OnGround && runningState != PlayerStateType.OnAir)
                return;

            base.Execute(player);
        }
    }
}
