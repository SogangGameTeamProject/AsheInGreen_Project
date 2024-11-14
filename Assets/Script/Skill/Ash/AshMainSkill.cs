using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;
using WebSocketSharp;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshMainSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/메인 스킬")]
    public class AshMainSkill : CharacterSkill
    {
        [Header("메인스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public float bulletSpeed = 200f;
        public float bulletDestroyTime = 2;
        public float chargingTime = 0.3f;//차징 단계별 시간
        public int maxChargingCnt = 3;//최대 차징 횟수
        public int energyIncrease = 1; //충전 량
        public float ChargingDamageCoefficient = 0.2f;//차징별 데미지 계수
        public float casterGrvity = 5; // 캐릭터 중력

        public override IEnumerator Charging(SkillHolder holder)
        {
            
            return base.Charging(holder);
        }

        public override IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {

            //스킬 애니메이션 처리
            if (!animationTrigger.IsNullOrEmpty())
            {
                holder._caster.PlayerSkillAni(animationTrigger);
            }

            //스킬 시작 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.linearVelocity = Vector2.zero;
            casterRbody.gravityScale = 0;//중력 설정

            // 차징 단계 구하기
            int chargeCnt = 
                Mathf.Clamp((int)(chargeTime / (chargingTime*100f/(100f+holder._caster.SkillAcceleration)))
                , 0, maxChargingCnt); 

            // 차징 정도에 따른 에너지 충전
            holder._caster._characterSkillManager.skillList[2].NowEnergy += energyIncrease * chargeCnt; // 특수스킬 에너지 충전

            //총알 발사
            float damage = damageCoefficient + (ChargingDamageCoefficient * chargeCnt);//데미지 설정
            //보스 타겟
            Vector2 fireDir = Vector2.zero;//발사 방향 조정
            EnemyController target = GameObject.FindAnyObjectByType<EnemyController>();
            if (target)
            {
                Vector2 casterPos = (Vector2)holder._caster.gameObject.transform.position;
                Vector2 targetPos = (Vector2)target.gameObject.transform.position;
                fireDir = targetPos - casterPos;
                fireDir = fireDir.normalized * bulletSpeed;
                if (fireDir.x > 0)
                    holder._caster.RequsetOnFlipRpc(CharacterDirection.Right);
                else
                    holder._caster.RequsetOnFlipRpc(CharacterDirection.Left);
            }
            else
            {
                fireDir = new Vector2((int)holder._caster.CharacterDirection, 0) * bulletSpeed;
            }
            ProjectileFactory.Instance.RequestProjectileFire(holder._caster, bulletPrefab, AttackType.MainSkill, damage,
                fireDir, holder._caster.firePoint.position, holder._caster.firePoint.rotation, bulletDestroyTime);


            //시간 경과
            yield return new WaitForSeconds(activeTime);

            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.gravityScale = casterGrvity;//중력 설정

            return base.End(holder);
        }
    }
}