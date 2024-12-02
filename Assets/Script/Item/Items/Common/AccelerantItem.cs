using UnityEngine;
using AshGreen.Character.Player;
namespace AshGreen.Item
{
    public class AccelerantItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            _playerController.AddSkillAccelerationRpc((int)itemData.baseVal[0]);
            _playerController.AddItemAccelerationRpc((int)itemData.baseVal[1]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            base.AddEffect();
            if (!_playerController.IsOwner) return;
            _playerController.AddSkillAccelerationRpc((int)itemData.stackIncVal[0]);
            _playerController.AddItemAccelerationRpc((int)itemData.stackIncVal[1]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            base.RemoveEffect();
            if (!_playerController.IsOwner) return;
            if (_stacks > 0)
            {
                _playerController.AddSkillAccelerationRpc(-(int)itemData.stackIncVal[0]);
                _playerController.AddItemAccelerationRpc(-(int)itemData.stackIncVal[1]);
            }
            else
            {
                _playerController.AddSkillAccelerationRpc(-(int)itemData.baseVal[0]);
                _playerController.AddItemAccelerationRpc(-(int)itemData.baseVal[1]);
            }
        }
    }
}
