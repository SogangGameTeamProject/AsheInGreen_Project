using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "GraySecondarySkill", menuName = "Scriptable Objects/스킬/플레이어/그래이/보조 스킬")]
    public class GraySecondarySkill : CharacterSkill
    {
        [Header("보조스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public float fireDelay = 0.5f;//발사 딜레이
        public float jumpPower = 300;//점프 파워
        public float jumpUpFDelay = 0.2f;//점프 시작 딜레이
        public float jumpUpTime = 0.5f;//점프 시간

        public override IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            //스킬 시작 시 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가

            //스킬 애니메이션 처리
            if (!animationTrigger.IsNullOrEmpty())
            {
                holder._caster.PlayerSkillAni(animationTrigger);
            }

            //보조 스킬 올라가기
            if (!holder.isReuse)
            {
                //스킬 재사용 가능 상태로 전환
                holder.isReuse = true;
                holder.NowChargeCnt = 1;

                //올라가기
                //벨로시티 0으로 초기화 후 대쉬
                

                Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
                casterRbody.linearVelocity = Vector2.zero;
                casterRbody.AddForceY(jumpPower, ForceMode2D.Impulse);
                yield return new WaitForSeconds(activeTime);
                casterRbody.linearVelocity = Vector2.zero;

                holder._caster.OnUseSubSkillEvent();//서브스킬 사용 이벤트 호출

                holder._caster._movementController.isUnableMove = false;//이동 가능

                //총알 발사
                float currentTimer = 0;
                while (!holder._caster._movementController.isGrounded)
                {
                    currentTimer+=Time.deltaTime;
                    if(currentTimer >= fireDelay)
                    {
                        currentTimer = 0;
                        ProjectileFactory.Instance.RequestProjectileFire(holder._caster, bulletPrefab, AttackType.SecondarySkill,
                            damageCoefficient, Vector2.zero, holder._caster.firePoint.position, holder._caster.firePoint.rotation, 0);
                    }
                    yield return null;
                }

            }
            //보조 스킬 내려가기
            else
            {
                // 착지 할 때까지 내려가기

            }

            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 시 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능

            //
            if (!holder.isReuse)
            {
                
            }
            else
            {
                
            }

            yield return base.End(holder);
        }
    }
}