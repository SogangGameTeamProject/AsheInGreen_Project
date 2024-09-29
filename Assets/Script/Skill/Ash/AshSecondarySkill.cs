using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSecondarySkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/보조 스킬")]
    public class AshSecondarySkill : CharacterSkill
    {

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("서브 스킬 사용");

            yield return base.End(holder);
        }
    }
}