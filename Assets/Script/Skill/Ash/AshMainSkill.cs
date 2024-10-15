using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshMainSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/메인 스킬")]
    public class AshMainSkill : CharacterSkill
    {
        [Header("메인스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public int bulletPreIndex = 0;
        public float bulletSpeed = 200f;
        public float bulletDestroyTime = 2;
        public float chargingTime = 0.3f;//차징 단계별 시간
        public int maxChargingCnt = 3;//최대 차징 횟수
        public int energyIncrease = 1; //충전 량
        public float ChargingDamageCoefficient = 0.2f;//차징별 데미지 계수

        public override IEnumerator Charging(SkillHolder holder)
        {
            Debug.Log("스킬 차징");
            
            return base.Charging(holder);
        }

        public override IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            Debug.Log("메인스킬 사용");

            // 차징 단계 구하기
            int chargeCnt = 
                Mathf.Clamp((int)(chargeTime / (chargingTime*100f/(100f+holder._caster.SkillAcceleration)))
                , 0, maxChargingCnt); 

            Debug.Log("충전량: " + chargeCnt);

            // 차징 정도에 따른 에너지 충전
            holder._caster._characterSkillManager.skillList[2].NowEnergy += energyIncrease * chargeCnt; // 특수스킬 에너지 충전

            // 서버에 총알 생성을 요청하는 RPC 호출
            Vector3 firePointPosition = holder._caster.firePoint.position;//투사체 발사 위치 조정
            Quaternion firePointRotation = holder._caster.firePoint.rotation;//투사체 회전 조정
            NetworkObject owner = holder._caster.GetComponent<NetworkObject>();//공격자 설정
            float damage = damageCoefficient * (ChargingDamageCoefficient * chargeCnt);//데미지 설정
            Vector2 fireDir = new Vector2((int)holder._caster.CharacterDirection, 0) * bulletSpeed;//발사 방향 조정

            holder._caster._characterProjectileFactory.RequestProjectileFireServerRpc(
               owner, bulletPreIndex, AttackType.MainSkill, damage, fireDir, 
               firePointPosition, firePointRotation, bulletDestroyTime
               );

            return base.Use(holder);
        }
    }
}