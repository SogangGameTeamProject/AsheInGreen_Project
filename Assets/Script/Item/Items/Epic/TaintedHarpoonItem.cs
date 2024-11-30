using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.EventBus;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class TaintedHarpoonItem : ItemEffectInit
    {
        float currentTime = 0;
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            currentTime = 0;

            _playerController._damageReceiver.DealDamageAction += ApplyDebuff;
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
            if (_stacks <= 0)
            {
                _playerController._damageReceiver.DealDamageAction -= ApplyDebuff;
            }
        }

        private void Update()
        {
            if (!_playerController.IsOwner) return;
            if (_stacks >0)
                currentTime += Time.deltaTime;
        }

        private void ApplyDebuff
            (Character.CharacterController controller, float damage, Character.AttackType type, bool isCriticale)
        {
            if (currentTime >= (itemData.cooldownTime / (100 + _playerController.ItemAcceleration))) return;

            EnemyController enemy = controller as EnemyController;
            if (enemy)
                enemy.debuffManager.AddDebuffRpc(DebuffType.Corruption,
                    _stacks, itemData.baseVal.ToArray(), itemData.stackIncVal.ToArray());
        }
    }
}
