using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSpecialSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/스페셜 스킬")]
    public class AshSpecialSkill : CharacterSkill
    {
        [Header("특수스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public float bulletSpeed = 200f;
        public float bulletDestroyTime = 2;
        public float fireDelay = 0.05f;
        public float casterGrvity = 5;

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("특수스킬 사용");
            Debug.Log("소모 에너지: " + holder.NowEnergy);
            int nowEnergy = holder.NowEnergy;
            holder.NowEnergy = 0;//에너지 비우기

            //스킬 시작 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.linearVelocity = Vector2.zero;
            casterRbody.gravityScale = 0;//중력 설정

            //에너지 수치 만큼 총알 발싸
            for (int i = 0; i < nowEnergy; i++)
            {
                float damage = damageCoefficient;//데미지 설정
                Vector2 fireDir = new Vector2((int)holder._caster.CharacterDirection, 0) * bulletSpeed;//발사 방향 조정
                ProjectileFactory.Instance.RequestProjectileFire(holder._caster, bulletPrefab, AttackType.MainSkill, damage,
                    fireDir, holder._caster.firePoint.position, holder._caster.firePoint.rotation, bulletDestroyTime);

                yield return new WaitForSeconds(fireDelay);
            }

            yield return End(holder);
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