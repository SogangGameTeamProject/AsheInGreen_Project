using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Buff
{
    [CreateAssetMenu(fileName = "OverflowingPower_Buff", menuName = "Scriptable Objects/Buff/OverflowingPower")]
    public class OverflowingPower_Buff : BuffData
    {
        // 버프 적용 메서드
        public override void ApplyBuff(PlayerController player, Buff buff)
        {
            player.AddAttackpowerRpc(0, baseVal[0] + (stackIncVal[0] * (buff.currentStacks-1)));// 공격력 증가
            player.AddCriticalRpc(baseVal[1] + (stackIncVal[1] * (buff.currentStacks - 1)));// 크리티컬 확률 증가
        }

        // 버프 업데이트 메서드
        public override void UpdateBuff(PlayerController player, Buff buff)
        {
            
        }

        // 버프 제거 메서드
        public override void RemoveBuff(PlayerController player, Buff buff)
        {
            player.AddAttackpowerRpc(0, -(baseVal[0] + (stackIncVal[0] * (buff.currentStacks - 1))));// 공격력 감소
            player.AddCriticalRpc(-(baseVal[1] + (stackIncVal[1] * (buff.currentStacks - 1))));// 크리티컬 확률 감소
        }
    }

}
