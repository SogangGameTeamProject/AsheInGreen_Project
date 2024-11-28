using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.EventBus;

namespace AshGreen.Item
{
    public class ReinforcedSuitItem : ItemEffectInit
    {
        int subSkillCount = 0;
        
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            subSkillCount = 0;
            //보조스킬 사용 시 이벤트 추가
            _playerController.UseSubSkillEvent += ApplyBuff;
            //메인스킬 사용 시 이벤트 추가
            _playerController.UseMainSkillEvent += RemoveBuff;
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
                //보조스킬 사용 시 이벤트 제거
                _playerController.UseSubSkillEvent -= ApplyBuff;
                //메인스킬 사용 시 이벤트 제거
                _playerController.UseMainSkillEvent -= RemoveBuff;
            }
        }

        private void ApplyBuff(object sender, EventArgs e)
        {
            //보조스킬 ?회 사용시 버프 부여
            subSkillCount++;
            if(subSkillCount >= itemData.cntNum)
            {
                _playerController.buffManager.AddBuffRpc(BuffType.UtteranceZ,
                _stacks, itemData.baseVal.ToArray(), itemData.stackIncVal.ToArray());
                subSkillCount = 0;
            }
        }

        private void RemoveBuff(object sender, EventArgs e)
        {
            //메인스킬 사용 후 버프 제거
            if (_playerController.buffManager.activeBuffs.ContainsKey(BuffType.UtteranceZ))
            {
                _playerController.buffManager.RemoveBuffRpc(BuffType.UtteranceZ);
            }
        }
    }
}
