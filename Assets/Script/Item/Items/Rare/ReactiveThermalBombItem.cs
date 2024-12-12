using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class ReactiveThermalBombItem : ItemEffectInit
    {
        // 디버프 적용 확률
        [SerializeField]
        private float debuffChance = 0.3f;
        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
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
            // 타격 시 버프 적용 이벤트 제거
            if (_stacks <= 0)
                _playerController._damageReceiver.DealDamageAction -= ApplyDebuff;
        }

        private void ApplyDebuff
            (Character.CharacterController controller, float damage, Character.AttackType type, bool isCriticale)
        {
            // 타격 시 메인 스킬 공격인지 체크
            if(type != AttackType.MainSkill) return;
            // 30% 확률로 디버프 적용
            if (UnityEngine.Random.value <= debuffChance)
            {
                EnemyController enemy = controller as EnemyController;
                if (enemy)
                    enemy.debuffManager.AddDebuffRpc(DebuffType.PartDestruction, _stacks);
            }
        }
    }
}
