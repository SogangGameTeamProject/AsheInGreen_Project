using AshGreen.Character;
using AshGreen.Character.Player;
using UnityEngine;
namespace AshGreen.Debuff
{
    [CreateAssetMenu(fileName = "Wound_Debuff", menuName = "Scriptable Objects/Debuff/Wound")]
    public class Wound_Debuff : DebuffData
    {
        public override void ApplyDebuff(EnemyController enemy, Debuff debuff)
        {
            enemy.AddTakenDamageCoefficientRpc(debuff.baseVal[0] * (debuff.stackVal[0] * (debuff.currentStacks - 1)));
        }

        public override void RemoveDebuff(EnemyController enemy, Debuff debuff)
        {
            enemy.AddTakenDamageCoefficientRpc(-(debuff.baseVal[0] * (debuff.stackVal[0] * (debuff.currentStacks - 1))));
        }

        public override void UpdateDebuff(EnemyController enemy, Debuff debuff)
        {

        }

        public override void ReapplyDebuff(EnemyController enemy, Debuff debuff)
        {

        }
    }

}
