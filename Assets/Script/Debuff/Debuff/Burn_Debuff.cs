using AshGreen.Character;
using AshGreen.Character.Player;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Debuff
{
    [CreateAssetMenu(fileName = "Burn_Debuff", menuName = "Scriptable Objects/Debuff/Burn_")]
    public class Burn_Debuff : DebuffData
    {
        public override void ApplyDebuff(EnemyController enemy, Debuff debuff)
        {
            
        }

        public override void RemoveDebuff(EnemyController enemy, Debuff debuff)
        {
            
        }

        public override void UpdateDebuff(EnemyController enemy, Debuff debuff)
        {
            // 디버프 활성화 주기가 되면 적에게 데미지를 입힘
            if (debuff.currentTimer >= activationCycle)
            {
                float damage = baseVal[0] + (stackIncVal[0] * (debuff.currentStacks - 1));
                NetworkObject networkObject = enemy.gameObject?.GetComponent<NetworkObject>();
                enemy.gameObject?.GetComponent<IDamageable>()?.DealDamageRpc(networkObject, damage, AttackType.Debuff);

                debuff.currentTimer = 0;
            }
        }

        public override void ReapplyDebuff(EnemyController enemy, Debuff debuff)
        {
            
        }
    }

}
