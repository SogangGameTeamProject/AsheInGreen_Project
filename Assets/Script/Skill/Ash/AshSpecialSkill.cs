using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;
using WebSocketSharp;

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

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            //스킬 애니메이션 처리
            if (!animationTrigger.IsNullOrEmpty())
            {
                holder._caster.PlayerSkillAni(animationTrigger);
            }

            int nowEnergy = holder.NowEnergy;
            holder.NowEnergy = 0;//에너지 비우기

            //스킬 시작 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가
            if (!holder._caster._movementController.isGrounded)
            {
                Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
                casterRbody.linearVelocity = Vector2.zero;
                casterRbody.gravityScale = 0;//중력 설정
            }

            //에너지 수치 만큼 총알 발싸
            for (int i = 0; i < nowEnergy; i++)
            {
                float damage = damageCoefficient;//데미지 설정
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
                        holder._caster.CharacterDirection = CharacterDirection.Right;
                    else
                        holder._caster.CharacterDirection = CharacterDirection.Left;
                }
                else
                {
                    fireDir = new Vector2((int)holder._caster.CharacterDirection, 0) * bulletSpeed;
                }

                ProjectileFactory.Instance.RequestProjectileFire(holder._caster, bulletPrefab, AttackType.SpecialSkill, damage,
                    fireDir, holder._caster.firePoint.position, holder._caster.firePoint.rotation, bulletDestroyTime);

                holder._caster.OnUseSpecialSkillEvent();//특수스킬 사용 이벤트 호출

                yield return new WaitForSeconds(fireDelay);
            }

            yield return End(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            if (casterRbody.gravityScale != holder._caster.Gravity)
                casterRbody.gravityScale = holder._caster.Gravity;//중력 설정

            return base.End(holder);
        }
    }
}