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
            _playerController.AddAccelerationRpc((int)itemData.baseVal[0], (int)itemData.baseVal[1]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            _playerController.AddAccelerationRpc((int)itemData.stackIncVal[0], (int)itemData.stackIncVal[1]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            _stacks--;
            if(_stacks > 0)
            {
                _playerController.AddAccelerationRpc(-(int)itemData.stackIncVal[0], -(int)itemData.stackIncVal[1]);
            }
            else
            {
                _playerController.AddAccelerationRpc(-(int)itemData.baseVal[0], -(int)itemData.baseVal[1]);
            }
        }
    }
}
