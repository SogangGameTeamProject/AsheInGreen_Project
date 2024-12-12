using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.EventBus;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class PileDriverItem : ItemEffectInit
    {
        float activeCnt = 0;
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            activeCnt = 0;

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
        }

        private void ApplyDebuff
            (Character.CharacterController controller, float damage, Character.AttackType type, bool isCriticale)
        {
            activeCnt++;
            Debug.Log((itemData.baseVal[1] - itemData.baseVal[1] * (1 - 1 / Mathf.Sqrt(_stacks))));
            if (activeCnt < (itemData.baseVal[1]- itemData.baseVal[1] * (1 - 1 / Mathf.Sqrt(_stacks)))) return;
            Debug.Log(activeCnt);
            activeCnt = 0;
            EnemyController enemy = controller as EnemyController;
            if (enemy)
                enemy.debuffManager.AddDebuffRpc(DebuffType.Wound,
                    _stacks);
        }
    }
}
