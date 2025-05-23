using UnityEngine;
using AshGreen.Character.Player;
namespace AshGreen.Item
{
    public class CannedItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            _playerController.AddHpRpc((int)itemData.baseVal[0], (int)itemData.baseVal[0]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            base.AddEffect();
            if (!_playerController.IsOwner) return;
            _playerController.AddHpRpc((int)itemData.stackIncVal[0], (int)itemData.stackIncVal[0]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            base.RemoveEffect();
            if (!_playerController.IsOwner) return;
            if (_stacks > 0)
            {
                _playerController.AddHpRpc(0, -(int)itemData.stackIncVal[0]);
            }
            else
            {
                _playerController.AddHpRpc(0, -(int)itemData.baseVal[0]);
            }
        }
    }
}
