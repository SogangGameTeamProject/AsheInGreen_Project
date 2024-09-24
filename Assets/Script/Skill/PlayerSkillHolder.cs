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

            //값 초기화
            cooldownTime = skill.cooldownTime;
            activeTime = skill.activeTime;
            state = SkillState.ready;
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

        private CharacterSkill skill = null;
        private float cooldownTime = 0;
        private float activeTime = 0;
        public SkillState state { get; private set; }

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
                        ChageState(SkillState.cooldown);
                    break;
                //스킬 쿨다운 처리
                case SkillState.cooldown:
                    if (skill.cooldownTime > 0)
                        skill.cooldownTime -= Time.deltaTime;
                    else
                        ChageState(SkillState.ready);
                    break;
            }
        }

        //스킬 상태 전환 함수
        private void ChageState(SkillState newState)
        {
            //상태 종료 처리
            switch (state)
            {
                case SkillState.ready:

                    break;
                case SkillState.charge:

                    break;
                case SkillState.active:

                    break;
                case SkillState.cooldown:

                    break;
            }

            state = newState;

            //상태 시작 처리
            switch (state)
            {
                case SkillState.ready:

                    break;
                case SkillState.charge:

                    break;
                case SkillState.active:

                    break;
                case SkillState.cooldown:

                    break;
            }

            
        }

        /// <summary>
        /// 스킬 사용 메서드
        /// </summary>
        private void UseSkill()
        {
            ChageState(SkillState.active);
            //skill.Use(caster);
        }

        private void UseCharging()
        {

        }

        public void PresseSkill()
        {
            if (state != SkillState.ready)
                return;

            if (skill.charging)
            {

            }
            else
            {
                
            }
        }

        public void ReleaseSkill()
        {
            if (state == SkillState.charge)
                UseSkill();
        }
    }
}