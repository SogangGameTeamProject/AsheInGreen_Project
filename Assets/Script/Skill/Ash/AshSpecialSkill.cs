using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSpecialSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/스페셜 스킬")]
    public class AshSpecialSkill : CharacterSkill
    {
        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("특수스킬 사용");

            yield return End(holder);
        }
    }
}