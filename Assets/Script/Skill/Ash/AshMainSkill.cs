using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshMainSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/메인 스킬")]
    public class AshMainSkill : CharacterSkill
    {
        public override IEnumerator Charging(SkillHolder holder)
        {
            Debug.Log("스킬 차징");
            return base.Charging(holder);
        }

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("메인스킬 사용");

            yield return base.End(holder);
        }
    }
}