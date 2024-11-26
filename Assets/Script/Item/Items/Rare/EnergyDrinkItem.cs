using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;

namespace AshGreen.Item
{
    public class EnergyDrinkItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            // 타격 시 버프 적용 이벤트 추가
            _playerController._damageReceiver.DealDamageAction += ApplyBuff;
        }

        // 아이템 효과를 추가하는 함수
        public override void AddEffect()
        {
            base.AddEffect();
            if (!_playerController.IsOwner) return;
        }

        // 아이템 효과를 제거하는 함수
        public override void RemoveEffect()
        {
            base.RemoveEffect();
            if (!_playerController.IsOwner) return;
            // 타격 시 버프 적용 이벤트 제거
            if (_stacks <= 0)
                _playerController._damageReceiver.DealDamageAction -= ApplyBuff;
        }

        private void ApplyBuff(Character.CharacterController controller, float arg2, Character.AttackType type, bool arg4)
        {
            _playerController.buffManager.AddBuffRpc(BuffType.Adrenaline,
                _stacks, itemData.baseVal[0], itemData.stackIncVal[0]);
        }
    }
}
