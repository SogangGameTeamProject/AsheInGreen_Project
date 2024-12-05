using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;
using WebSocketSharp;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "GrayPassiveSkill", menuName = "Scriptable Objects/스킬/플레이어/그래이/패시브 스킬")]
    public class GrayPassiveSkill : CharacterSkill
    {
        [Header("패시브 스킬 옵션")]
        public SkillType[] skillType;

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            
            yield return base.Use(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            
            return base.End(holder);
        }
    }
}