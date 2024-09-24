using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Character.Skill
{
    public class PlayerSkillHolder : NetworkBehaviour
    {
        public PlayerSkillHolder(CharacterController caster, CharacterSkill skill)
        {
            _caster = caster;
            this.skill = skill;
            skill.Initialize(_caster);
        }

        CharacterController _caster = null;

        //스킬 상태
        public enum SkillState
        {
            ready,//준비됨
            charge,//차징 중
            active,//사용 중
            cooldown//쿨타임
        }

        public CharacterSkill skill = null;
        public float cooldownTime = 0;
        public float activeTime = 0;
        public SkillState state = SkillState.ready;

        private void Update()
        {
            switch (state)
            {
                //차징 시 처리
                case SkillState.charge:
                    
                    break;
                //스킬 사용 중 처리
                case SkillState.active:
                    if (skill.activeTime > 0)
                        skill.activeTime -= Time.deltaTime;
                    else
                        state = SkillState.cooldown;
                    break;
                //스킬 쿨다운 처리
                case SkillState.cooldown:
                    if (skill.cooldownTime > 0)
                        skill.cooldownTime -= Time.deltaTime;
                    else
                        state = SkillState.ready;
                    break;
            }
        }

        /// <summary>
        /// 스킬 사용 메서드
        /// </summary>
        public void UseSkill()
        {
            if((state == SkillState.ready && !skill.charging) || state == SkillState.charge)
            {
                state = SkillState.active;
                skill.Use();
            }
            else if(state == SkillState.ready && skill.charging)
            {
                state = SkillState.charge;
            }
        }
    }
}