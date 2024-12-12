using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Buff
{
    [CreateAssetMenu(fileName = "UtteranceZ_Buff", menuName = "Scriptable Objects/Buff/UtteranceZ")]
    public class UtteranceZ_Buff : BuffData
    {
        // 버프 적용 메서드
        public override void ApplyBuff(PlayerController player, Buff buff)
        {
            // 메인 스킬 데미지 증가량 적용
            player.AddMainSkillDamageConfigServerRpc
                (baseVal[0] + (stackIncVal[0] * (buff.currentStacks-1)));
        }

        // 버프 업데이트 메서드
        public override void UpdateBuff(PlayerController player, Buff buff)
        {
            
        }

        // 버프 제거 메서드
        public override void RemoveBuff(PlayerController player, Buff buff)
        {
            // 메인 스킬 데미지 증가량 제거
            player.AddMainSkillDamageConfigServerRpc
                (-(baseVal[0] + (stackIncVal[0] * (buff.currentStacks - 1))));
        }
    }

}
