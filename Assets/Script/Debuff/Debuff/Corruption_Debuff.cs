using AshGreen.Character;
using AshGreen.Character.Player;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Debuff
{
    [CreateAssetMenu(fileName = "Corruption_Debuff", menuName = "Scriptable Objects/Debuff/Corruption")]
    public class Corruption_Debuff : DebuffData
    {
        public override void ApplyDebuff(EnemyController enemy, Debuff debuff)
        {
            
        }

        public override void RemoveDebuff(EnemyController enemy, Debuff debuff)
        {
            // 디버프가 해제될 때 적에게 적용된 디버프 당 데미지를 입힘
            int debuffCnt = enemy.debuffManager.activeDebuffs.Count;

            float damage = debuffCnt * (debuff.baseVal[0] * (debuff.stackVal[0] * (debuff.currentStacks - 1)));
            NetworkObject networkObject = enemy.gameObject?.GetComponent<NetworkObject>();
            enemy.gameObject?.GetComponent<IDamageable>()?.DealDamageRpc(networkObject, damage, AttackType.Debuff);
        }

        public override void UpdateDebuff(EnemyController enemy, Debuff debuff)
        {

        }

        public override void ReapplyDebuff(EnemyController enemy, Debuff debuff)
        {

        }
    }

}
