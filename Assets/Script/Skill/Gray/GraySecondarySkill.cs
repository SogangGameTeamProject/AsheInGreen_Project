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
        public float bulletSpeed = 80f;
        public float jumpPower = 150;//점프 파워
        public float jumpUpFDelay = 0.2f;//점프 시작 딜레이
        public float addGravity = 2f;//중력값

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
                holder.state = SkillHolder.SkillState.Idle;
                holder.isReuse = true;
                holder.NowChargeCnt = 1;

                //중력값 조정
                holder._caster.AddGravityServerRpc(-addGravity);
                holder._caster.AddJumpRpc(0, -0.5f);
                holder._caster.jumCnt = 0;

                //올라가기

                yield return new WaitForSeconds(jumpUpFDelay);
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
                        //보스 타겟
                        Vector2 fireDir = Vector2.zero;//발사 방향 조정
                        Vector2 targetPos;
                        EnemyController target = GameObject.FindAnyObjectByType<EnemyController>();
                        if (target)
                        {
                            Vector2 casterPos = (Vector2)holder._caster.gameObject.transform.position;
                            targetPos = (Vector2)target.gameObject.transform.position;
                            fireDir = targetPos - casterPos;
                            if (fireDir.x > 0)
                                holder._caster.CharacterDirection = CharacterDirection.Right;
                            else
                                holder._caster.CharacterDirection = CharacterDirection.Left;
                        }
                        else
                        {
                            Vector2 casterPos = (Vector2)holder._caster.gameObject.transform.position;
                            targetPos = new Vector2(casterPos.x + ((int)holder._caster.CharacterDirection * 20), casterPos.y);
                        }


                        ProjectileFactory.Instance.RequestProjectileTargetFire
                            (holder._caster, bulletPrefab, AttackType.SecondarySkill, damageCoefficient,
                        targetPos, holder._caster.firePoint.position, holder._caster.firePoint.rotation, bulletSpeed);
                    }
                    yield return null;
                }

            }
            //보조 스킬 내려가기
            else
            {
                holder.isReuse = false;
                //중력값 조정
                holder._caster.AddGravityServerRpc(addGravity);
                holder._caster.AddJumpRpc(0, 0.5f);

                // 착지 할 때까지 내려가기
                //내려가기
                //벨로시티 0으로 초기화 후 대쉬
                Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
                casterRbody.linearVelocity = Vector2.zero;
                casterRbody.AddForceY(-jumpPower, ForceMode2D.Impulse);
                
                while(!holder._caster._movementController.isGrounded)
                {
                    yield return null;
                }
                casterRbody.linearVelocity = Vector2.zero;
            }

            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 시 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능

            //
            if (holder.isReuse)
            {
                holder.isReuse = false;
                //중력값 조정
                holder._caster.AddGravityServerRpc(addGravity);
                holder._caster.AddJumpRpc(0, 0.5f);

                //쿨타임 조정
                holder.currentCoolTime = 0;
                holder.NowChargeCnt = 0;
            }
            else
            {

            }

            yield return base.End(holder);
        }
    }
}