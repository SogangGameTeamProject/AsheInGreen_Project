using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Buff
{
    [CreateAssetMenu(fileName = "Adrenaline_Buff", menuName = "Scriptable Objects/Buff/Adrenaline")]
    public class Adrenaline_Buff : BuffData
    {
        // 버프 적용 메서드
        public override void ApplyBuff(PlayerController player, Buff buff)
        {
            Debug.Log("ApplyBuff");
            player.AddMovespeedRpc(0, buff.baseVal + (buff.stackVal * (buff.currentStacks-1)));// 이동 속도 증가
            player.AddJumpRpc(0, buff.baseVal + (buff.stackVal * (buff.currentStacks - 1)));// 점프력 증가
        }

        // 버프 업데이트 메서드
        public override void UpdateBuff(PlayerController player, Buff buff)
        {
            
        }

        // 버프 제거 메서드
        public override void RemoveBuff(PlayerController player, Buff buff)
        {
            Debug.Log("RemoveBuff");
            player.AddMovespeedRpc(0, -(buff.baseVal + (buff.stackVal * (buff.currentStacks - 1))));// 이동 속도 감소
            player.AddJumpRpc(0, -(buff.baseVal + (buff.stackVal * (buff.currentStacks - 1))));// 점프력 감소
        }
    }

}
