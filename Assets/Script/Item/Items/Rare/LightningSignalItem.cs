using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class LightningSignalItem : ItemEffectInit
    {
        private bool isStatusUp = false;

        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            isStatusUp = false;
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
        }

        private void Update()
        {
            if(!_playerController.IsOwner) return;

            // 보스 디버프 보유여부 확인
            bool isDebuff = false;
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].debuffManager.activeDebuffs.Count > 0)
                {
                    isDebuff = true;
                    break;
                }
            }

            // 보스가 디버프 있으면 스킬가속 증가
            float acceleration = itemData.baseVal[0] + (itemData.stackIncVal[0] * (_stacks - 1));
            if (isDebuff && !isStatusUp && _stacks > 0)
            {
                isStatusUp = true;
                _playerController.AddAccelerationRpc(acceleration);
            }
            else if(isStatusUp)
            {
                isStatusUp = false;
                _playerController.AddAccelerationRpc(-acceleration);
            }
        }
    }
}
