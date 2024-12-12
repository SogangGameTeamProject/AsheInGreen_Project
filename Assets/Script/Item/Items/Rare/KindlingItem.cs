using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class KindlingItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;


            // 타격 시 디버프 적용 이벤트 추가
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

            // 타격 시 디버프 적용 이벤트 제거
            if (_stacks <= 0)
                _playerController._damageReceiver.DealDamageAction -= ApplyDebuff;
        }

        private void ApplyDebuff
            (Character.CharacterController controller, float damage, Character.AttackType type, bool isCriticale)
        {
            EnemyController enemy = controller as EnemyController;
            if(enemy)
                enemy.debuffManager.AddDebuffRpc(DebuffType.Burn,
                    _stacks);
        }
    }
}
