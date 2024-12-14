using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using AshGreen.Sound;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSecondarySkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/보조 스킬")]
    public class AshSecondarySkill : CharacterSkill
    {
        public float dashPower = 100;
        public int energyIncrease = 1;
        public override IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            //스킬 애니메이션 처리
            if (!animationTrigger.IsNullOrEmpty())
            {
                holder._caster.PlayerSkillAni(animationTrigger);
            }

            //스킬 사운드 처리
            if (skillSound)
                SoundManager.Instance.PlaySFXRpc(skillSound);

            holder._caster._characterSkillManager.skillList[2].NowEnergy += energyIncrease;//특수스킬 에너지 충전
            //스킬 시작 시 처리
            holder._caster.SetDamageimmunity(true);//무적
            holder._caster._movementController.isUnableMove = true;//이동 불가


            //대쉬 방향 구하기
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.gravityScale = 0;//중력 설정
            float dashVecX = (new Vector2(casterRbody.linearVelocity.x, 0)).normalized.x;
            dashVecX = (dashVecX) == 0 ? (float)holder._caster.CharacterDirection : dashVecX;

            //벨로시티 0으로 초기화 후 대쉬
            casterRbody.linearVelocity = Vector2.zero;
            casterRbody.AddForceX(dashVecX*dashPower, ForceMode2D.Impulse);

            holder._caster.OnUseSubSkillEvent();//서브스킬 사용 이벤트 호출

            yield return new WaitForSeconds(activeTime);

            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 시 처리
            holder._caster.SetDamageimmunity(false);//무적
            holder._caster._movementController.isUnableMove = false;//이동 가능
            holder._caster.GetComponent<Rigidbody2D>().gravityScale = holder._caster.Gravity;
            holder._caster.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            yield return base.End(holder);
        }
    }
}