using UnityEngine;
using AshGreen.Character.Player;
namespace AshGreen.Item
{
    public class SteroidItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            _playerController.AddHpRpc(0, (int)itemData.baseVal[0]);
            _playerController.AddAccelerationRpc(itemData.baseVal[1]);
            _playerController.AddCriticalRpc(itemData.baseVal[2]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            _playerController.AddHpRpc(0, (int)itemData.stackIncVal[0]);
            _playerController.AddAccelerationRpc(itemData.stackIncVal[1]);
            _playerController.AddCriticalRpc(itemData.stackIncVal[2]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            _stacks--;
            if(_stacks > 0)
            {
                _playerController.AddAccelerationRpc(-itemData.stackIncVal[1]);
                _playerController.AddCriticalRpc(-itemData.stackIncVal[2]);
            }
            else
            {
                _playerController.AddAccelerationRpc(-itemData.baseVal[1]);
                _playerController.AddCriticalRpc(-itemData.baseVal[2]);
            }
        }
    }
}
