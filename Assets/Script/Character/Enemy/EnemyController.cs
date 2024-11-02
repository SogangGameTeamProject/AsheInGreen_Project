using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public class EnemyController : CharacterController
    {
        [SerializeField]
        private EnemyConfig enemyConfig;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                OnSetStatusRpc();//스테이터스 값 초기화
            }

            nowHp.OnValueChanged += UpdateHpHUDRPC;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            nowHp.OnValueChanged -= UpdateHpHUDRPC;
        }

        //캐릭터 스테이터스값 초기 설정
        [Rpc(SendTo.Server)]
        private void OnSetStatusRpc()
        {
            if (enemyConfig)
            {
                Debug.Log("데이터 초기화");
                baseMaxHP.Value = enemyConfig.MaxHP;
                nowHp.Value = baseMaxHP.Value;
                GrowthAttackPower.Value = enemyConfig.GrowthAttackPower;
                GrowthPerAttackPower.Value = enemyConfig.GrowthPerAttackPower;
                baseAttackPower.Value = enemyConfig.AttackPower;
                EnemyUIController.Instance.enemyHud.Name.text = enemyConfig.characterName;
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateHpHUDRPC(int previousValue, int newValue)
        {
            Debug.Log($"MaxHP: {MaxHP} newValue: {newValue}");
            EnemyUIController.Instance.HpUpdate(MaxHP, newValue);
        }
    }
}
