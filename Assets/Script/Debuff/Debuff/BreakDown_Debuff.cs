using AshGreen.Character;
using AshGreen.Character.Player;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Debuff
{
    [CreateAssetMenu(fileName = "BreakDown_Debuff", menuName = "Scriptable Objects/Debuff/BreakDown")]
    public class BreakDown_Debuff : DebuffData
    {
        public override void ApplyDebuff(EnemyController enemy, Debuff debuff)
        {
            
        }

        public override void RemoveDebuff(EnemyController enemy, Debuff debuff)
        {
            BreakDownDamage(enemy, debuff);
        }

        public override void UpdateDebuff(EnemyController enemy, Debuff debuff)
        {

        }

        public override void ReapplyDebuff(EnemyController enemy, Debuff debuff)
        {
            BreakDownDamage(enemy, debuff);
        }

        public void BreakDownDamage(EnemyController enemy, Debuff debuff)
        {
            float damage = debuff.baseVal[0];
            NetworkObject networkObject = enemy.gameObject?.GetComponent<NetworkObject>();
            enemy.gameObject?.GetComponent<IDamageable>()?.DealDamageRpc(networkObject, damage, AttackType.Debuff);
        }
    }

}
