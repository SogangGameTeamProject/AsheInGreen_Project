using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Buff
{
    [CreateAssetMenu(fileName = "Adrenaline_Buff", menuName = "Scriptable Objects/Buff/Adrenaline")]
    public class Adrenaline_Buff : BuffData
    {
        public float buffValue = 0.1f;// 버프 값
        // 버프 적용 메서드
        public override void ApplyBuff(PlayerController player, int stack)
        {
            
        }

        // 버프 업데이트 메서드
        public override void UpdateBuff(PlayerController player, int stack, float elapsedTime)
        {

        }

        // 버프 제거 메서드
        public override void RemoveBuff(PlayerController player, int stack)
        {

        }
    }

}
