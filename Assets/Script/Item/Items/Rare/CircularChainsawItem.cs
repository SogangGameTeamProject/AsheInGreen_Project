using UnityEngine;
using AshGreen.Character.Player;
using System;
using AshGreen.Buff;
using AshGreen.Character;
using AshGreen.Debuff;

namespace AshGreen.Item
{
    public class CircularChainsawItem : ItemEffectInit
    {
        // 데미지 오브젝트
        [SerializeField]
        private GameObject damageObj;
        [SerializeField]
        private float lifeTime = 0.25f;
        private float currentTime = 0;

        //아이템 효과를 적용하는 함수
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            if (!_playerController.IsOwner) return;
            currentTime = 0;
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
            if(!_playerController.IsOwner && _stacks > 0) return;

            // 아이템 사용 시 데미지 오브젝트 생성
            currentTime += Time.deltaTime;
            if(currentTime >= itemData.cooldownTime * (100 / (100 + _playerController.ItemAcceleration)))
            {
                currentTime = 0;
                
                float damage = itemData.baseVal[0] + (itemData.stackIncVal[0] * (_stacks-1));
                Vector2 spawnPos = _playerController.transform.position;
                ProjectileFactory.Instance.RequestProjectileFire
                    (_playerController, damageObj, AttackType.Item, damage, Vector2.zero, spawnPos, Quaternion.identity, lifeTime);
            }
        }
    }
}
