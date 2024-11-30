using AshGreen.Character;
using AshGreen.Character.Player;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Debuff
{
    [CreateAssetMenu(fileName = "PartDestruction_Debuff", menuName = "Scriptable Objects/Debuff/PartDestruction")]
    public class PartDestruction_Debuff : DebuffData
    {
        public override void ApplyDebuff(EnemyController enemy, Debuff debuff)
        {
            enemy._damageReceiver.TakeDamageAction += debuff.saveDamage;
        }

        public override void RemoveDebuff(EnemyController enemy, Debuff debuff)
        {
            // 디버프가 해제될 때 적에게 누적데미지의 비례한 추가 데미지를 입힘
            float damage = debuff.currentDamage * (debuff.baseVal[0] * (debuff.stackVal[0] * (debuff.currentStacks - 1)));
            NetworkObject networkObject = enemy.gameObject?.GetComponent<NetworkObject>();
            enemy.gameObject?.GetComponent<IDamageable>()?.DealDamageRpc(networkObject, damage, AttackType.Debuff);

            enemy._damageReceiver.TakeDamageAction -= debuff.saveDamage;
        }

        public override void UpdateDebuff(EnemyController enemy, Debuff debuff)
        {

        }

        public override void ReapplyDebuff(EnemyController enemy, Debuff debuff)
        {

        }
    }

}
