using UnityEngine;
using AshGreen.Character.Player;
namespace AshGreen.Item
{
    public class LightFeathers_Item : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            _playerController.AddMovespeedRpc(0, (int)itemData.baseVal[0]);
            _playerController.AddGravityServerRpc(-(int)itemData.baseVal[0]);
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            _playerController.AddMovespeedRpc(0, (int)itemData.stackIncVal[0]);
            _playerController.AddGravityServerRpc(-(int)itemData.stackIncVal[0]);
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            _stacks--;
            if(_stacks > 0)
            {
                _playerController.AddMovespeedRpc(0, -(int)itemData.stackIncVal[0]);
                _playerController.AddGravityServerRpc(itemData.stackIncVal[0]);
            }
            else
            {
                _playerController.AddMovespeedRpc(0, -(int)itemData.baseVal[0]);
                _playerController.AddGravityServerRpc(itemData.baseVal[0]);
            }
        }
    }
}
