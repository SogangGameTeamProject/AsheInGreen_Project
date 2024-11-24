using UnityEngine;
using AshGreen.Character.Player;
namespace AshGreen.Item
{
    public class NailStuddedClubItem_Item : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            _playerController.AddAttackpowerRpc((int)itemData.baseVal[0], itemData.baseVal[1]);
            _playerController.AddCriticalRpc((int)itemData.baseVal[2]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            base.AddEffect();
            if (!_playerController.IsOwner) return;
            _playerController.AddAttackpowerRpc((int)itemData.stackIncVal[0], itemData.stackIncVal[1]);
            _playerController.AddCriticalRpc((int)itemData.stackIncVal[2]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            base.RemoveEffect();
            if (!_playerController.IsOwner) return;
            if (_stacks > 0)
            {
                _playerController.AddAttackpowerRpc(-(int)itemData.stackIncVal[0], -itemData.stackIncVal[1]);
                _playerController.AddCriticalRpc((int)itemData.stackIncVal[2]);
            }
            else
            {
                _playerController.AddAttackpowerRpc(-(int)itemData.baseVal[0], -itemData.baseVal[1]);
                _playerController.AddCriticalRpc(-(int)itemData.baseVal[2]);
            }
        }
    }
}
