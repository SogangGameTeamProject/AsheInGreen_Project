using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Item
{
    public class HealItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            _playerController.AddHpRpc((int)itemData.baseVal[0]);// 체력 회복
            _playerController.itemManager.RemoveItemRpc(itemData.itemID);// 아이템 제거
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {

        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            _playerController.itemManager.itemInventory.Remove(itemData.itemID);
            Destroy(gameObject);
        }
    }

}
