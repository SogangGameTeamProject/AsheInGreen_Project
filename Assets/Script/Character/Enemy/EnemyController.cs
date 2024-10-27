using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public class EnemyController : CharacterController
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            nowHp.OnValueChanged += UpdateHpHUDRPC;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            nowHp.OnValueChanged -= UpdateHpHUDRPC;
        }

        protected override void OnSetStatusRpc()
        {
            base.OnSetStatusRpc();

            EnemyUIController.Instance.enemyHud.Name.text = characterConfig.characterName;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateHpHUDRPC(int previousValue, int newValue)
        {
            Debug.Log($"MaxHP: {MaxHP} newValue: {newValue}");
            EnemyUIController.Instance.HpUpdate(MaxHP, newValue);
        }

    }
}
