using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "GraySecondarySkill", menuName = "Scriptable Objects/스킬/플레이어/그래이/보조 스킬")]
    public class GraySecondarySkill : CharacterSkill
    {
        [Header("보조스킬 옵션")]
        public float fireDelay = 0.5f;//발사 딜레이
        public float jumpPower = 300;//점프 파워
        public float jumpUpFDelay = 0.2f;//점프 시작 딜레이
        public float jumpUpTime = 0.5f;//점프 시간

        public override IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            //스킬 시작 시 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가

            //보조 스킬 올라가기
            if (!holder.isReuse)
            {

            }
            //보조 스킬 내려가기
            else
            {

            }

            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 시 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능
            yield return base.End(holder);
        }
    }
}