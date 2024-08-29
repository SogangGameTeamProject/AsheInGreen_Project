using UnityEngine;

namespace AshGreen.Player
{
    public class DownJumpCommand : PlayerCommandInit
    {
        public override void Execute(PlayerController player)
        {
            //키입력 예외 처리
            PlayerStateType runningState = player.runningStateType;
            if (runningState != PlayerStateType.OnGround && runningState != PlayerStateType.OnAir)
                return;
            Debug.Log("아래 점프 키 입력");
            base.Execute(player);
        }
    }
}
