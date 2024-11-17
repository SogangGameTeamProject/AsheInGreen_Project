using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using AshGreen.Character.Player;

namespace AshGreen.Item
{
    // 아이템 관리자
    public class ItemManager : NetworkBehaviour
    {
        private Dictionary<int, ItemEffectInit> itemInventory = new Dictionary<int, ItemEffectInit>();
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        // 아이템 추가
        [Rpc(SendTo.ClientsAndHost) ]
        public void AddItemRpc(int itemID)
        {
            itemInventory[itemID].AddEffect(playerController);
        }

        // 아이템 제거
        [Rpc(SendTo.ClientsAndHost)]
        public void RemoveItemRpc(int itemID)
        {
            itemInventory[itemID].RemoveEffect();
        }
    }
}
