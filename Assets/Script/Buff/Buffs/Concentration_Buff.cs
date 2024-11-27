using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Buff
{
    [CreateAssetMenu(fileName = "Concentration_Buff", menuName = "Scriptable Objects/Buff/Concentration")]
    public class Concentration_Buff : BuffData
    {
        // 버프 적용 메서드
        public override void ApplyBuff(PlayerController player, Buff buff)
        {
            player.AddDealDamageCoefficientRpc(buff.baseVal[0] + (buff.stackVal[0] * (buff.currentStacks-1)));// 공격력 증가
        }

        // 버프 업데이트 메서드
        public override void UpdateBuff(PlayerController player, Buff buff)
        {
            
        }

        // 버프 제거 메서드
        public override void RemoveBuff(PlayerController player, Buff buff)
        {
            player.AddDealDamageCoefficientRpc(-(buff.baseVal[0] + (buff.stackVal[0] * (buff.currentStacks - 1))));// 공격력 증가
        }
    }

}
