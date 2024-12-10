using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;
using WebSocketSharp;
using AshGreen.Character.Player;
using System;
using AshGreen.Debuff;

namespace AshGreen.Character.Skill
{
    public class GrayPassiveSkill : NetworkBehaviour
    {
        public PlayerController _player;
        public GameObject LightningObj = null;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _player._damageReceiver.DealDamageAction += SpecialSkillCoolReset;
            _player._damageReceiver.DealDamageAction += AddDamageObjSpwn;
            _player._damageReceiver.DealDamageAction += CollapseDebuff;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _player._damageReceiver.DealDamageAction -= SpecialSkillCoolReset;
            _player._damageReceiver.DealDamageAction -= AddDamageObjSpwn;
            _player._damageReceiver.DealDamageAction -= CollapseDebuff;
        }


        // 특이점 폭발 쿨초기화 이벤트
        private void SpecialSkillCoolReset(CharacterController controller, float arg2, AttackType type, bool arg4)
        {
            if(type == AttackType.MainSkill || type == AttackType.SecondarySkill)
            {
                // 랜덤값을 통해 특이점 폭발 쿨초기화 여부 결정
                var random = new System.Random();
                if (type == AttackType.SecondarySkill && random.NextDouble() > 0.05f) return;
                if (type == AttackType.MainSkill && random.NextDouble() > 0.2f) return;
                // 특이점 폭발 쿨초기화
                _player._characterSkillManager.skillList[2].NowChargeCnt = 1;
                _player._characterSkillManager.skillList[2].currentCoolTime = 0;
            }
        }

        // 전격폭발 이벤트
        private void AddDamageObjSpwn(CharacterController target, float damage, AttackType type, bool arg4)
        {
            // 붕괴 디버프가 걸려있을 때만 전격폭발 발생
            bool hvaeBreakDown = ((EnemyController)target).debuffManager.activeDebuffs.ContainsKey(DebuffType.breakdown);
            if ((type == AttackType.MainSkill || type == AttackType.SpecialSkill) && hvaeBreakDown)
            {
                float damageCoefficient = _player.characterConfig.skills[1].damageCoefficient;
                ProjectileFactory.Instance.RequestProjectileFire(_player, LightningObj, AttackType.Item,
                    damageCoefficient, Vector2.zero, target.transform.position, Quaternion.identity, 0.5f);
            }
        }

        //붕괴 디버프 이벤트
        private void CollapseDebuff(CharacterController target, float damage, AttackType type, bool arg4)
        {
            if(type == AttackType.SecondarySkill)
            {
                // 랜덤값을 통해 특이점 디버프 부여 결정
                var random = new System.Random();
                if (random.NextDouble() > _player.characterConfig.skills[1].utillValue) return;
                float[] baseVal = new float[] {20};
                ((EnemyController)target).debuffManager.AddDebuffRpc(DebuffType.breakdown, 1, baseVal, null);
            }
        }
    }
}