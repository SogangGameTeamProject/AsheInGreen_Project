using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.EventBus;

namespace AshGreen.Item
{
    public class ConcentrationItem : ItemEffectInit
    {
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;

            // 스테이지 시작 시 버프 적용 이벤트 추가
            GameFlowEventBus.Subscribe(GameFlowType.StageStart, ApplyBuff);

            // 피격 시 버프 제거 이벤트 추가
            _playerController._damageReceiver.TakeDamageAction += RemoveBuff;
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
                // 스테이지 시작 시 버프 적용 이벤트 제거
                GameFlowEventBus.Unsubscribe(GameFlowType.StageStart, ApplyBuff);
                // 피격 시 버프 제거 이벤트 제거
                _playerController._damageReceiver.TakeDamageAction -= RemoveBuff;
            }
        }

        private void ApplyBuff()
        {
            _playerController.buffManager.AddBuffRpc(BuffType.Concentration,
                _stacks);
        }

        private void RemoveBuff(float damage)
        {
            _playerController.buffManager.RemoveBuffRpc(BuffType.Concentration);
        }
    }
}
