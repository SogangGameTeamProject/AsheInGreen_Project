using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using AshGreen.Character.Player;

namespace AshGreen.Item
{
    // 아이템 관리자
    public class ItemManager : NetworkBehaviour
    {
        public ItemDataList itemDataList;
        private List<ItemData> itemList = new List<ItemData>();
        public Dictionary<int, ItemEffectInit> itemInventory = new Dictionary<int, ItemEffectInit>();
        private PlayerController playerController;

        private void Awake()
        {
            itemList = itemDataList.DataList;
            playerController = GetComponentInParent<PlayerController>();
        }

        // 아이템 추가
        [Rpc(SendTo.ClientsAndHost) ]
        public void AddItemRpc(int itemID)
        {
            //아이템 체크 후 있으면 스택 추가 없으면 오브젝트 생성
            if(itemInventory.ContainsKey(itemID))
                itemInventory[itemID].AddEffect();
            else
            {
                ItemData itemData = itemList.Find(item => item.itemID == itemID);
                GameObject itemObj = Instantiate(itemData.itemObj, transform);
                ItemEffectInit itemEffect = itemObj.GetComponent<ItemEffectInit>();
                itemInventory.Add(itemID, itemEffect);
                itemEffect.ApplyEffect(playerController);//아이템 효과 적용
            }
        }

        // 아이템 제거
        [Rpc(SendTo.ClientsAndHost)]
        public void RemoveItemRpc(int itemID)
        {
            if (itemInventory.ContainsKey(itemID))
            {
                itemInventory[itemID].RemoveEffect();
                itemInventory.Remove(itemID);
            }
        }
    }
}
