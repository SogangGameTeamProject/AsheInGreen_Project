using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSecondarySkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/보조 스킬")]
    public class AshSecondarySkill : CharacterSkill
    {
        [Header("서브스킬 옵션")]
        public float dashPower = 100;
        public int energyIncrease = 1;
        public float casterGrvity = 5;
        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("서브 스킬");
            holder._caster._characterSkillManager.skillList[2].NowEnergy += energyIncrease;//특수스킬 에너지 충전
            //스킬 시작 시 처리
            holder._caster.SetDamageimmunityRpc(true);//무적
            holder._caster._movementController.isUnableMove = true;//이동 불가

            //무적 처리 대기


            //대쉬 방향 구하기
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterGrvity = casterRbody.gravityScale;
            casterRbody.gravityScale = 0;//중력 설정
            float dashVecX = (new Vector2(casterRbody.linearVelocity.x, 0)).normalized.x;
            Debug.Log(dashVecX);
            dashVecX = (dashVecX) == 0 ? (float)holder._caster.CharacterDirection : dashVecX;
            Debug.Log(dashVecX);
            //벨로시티 0으로 초기화 후 대쉬
            casterRbody.linearVelocity = Vector2.zero;
            casterRbody.AddForceX(dashVecX*dashPower, ForceMode2D.Impulse);
            Debug.Log(dashVecX * dashPower);

            yield return new WaitForSeconds(activeTime);

            yield return End(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            Debug.Log("종료 처리");
            //스킬 종료 시 처리
            holder._caster.SetDamageimmunityRpc(false);//무적
            holder._caster._movementController.isUnableMove = false;//이동 가능
            holder._caster.GetComponent<Rigidbody2D>().gravityScale = casterGrvity;
            holder._caster.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            return base.End(holder);
        }
    }
}