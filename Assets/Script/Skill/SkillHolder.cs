using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Character.Skill
{
    [System.Serializable]
    public class SkillHolder
    {
        public SkillHolder(CharacterController caster, CharacterSkill skill)
        {
            _caster = caster;
            this.skill = skill;
            nowChargeCnt = skill.maxChageCnt;
            state = SkillState.Idle;
        }

        public CharacterController _caster = null;

        //스킬 상태
        public enum SkillState
        {
            Idle,//준비됨
            charge,//차징 중
            active//사용 중
        }

        public CharacterSkill skill = null;
        private float nowChargeCnt = 0;//현재 스킬 충전 횟수
        public float NowChargeCnt
        {
            get { return nowChargeCnt; }
            set
            {
                if (value <= skill.maxChageCnt)
                {
                    nowChargeCnt = value;
                    currentCoolTime = 0;
                }
            }
        }
        public float currentCoolTime { get; private set; }//지속 쿨타임 
        public SkillState state { get; set; }//스킬 상태
        
        public Coroutine holderCorutine = null;

        public void Update()
        {
            //쿨타임 적용
            if(nowChargeCnt < skill.maxChageCnt)
            {
                if (currentCoolTime < skill.cooldownTime)
                    currentCoolTime += Time.deltaTime;
                else
                    NowChargeCnt++;
            }
        }

        //스킬 차징 메서드
        public void Charging()
        {
            holderCorutine = _caster.StartCoroutine(skill.Charging(this));
        }

        //스킬 사용 메서드
        public void Use()
        {
            if (state == SkillState.Idle)
                holderCorutine = _caster.StartCoroutine(skill.Use(this));
            else
                state = SkillState.active;
        }

        //스킬 정지 메서드
        public void Stop()
        {
            if(state == SkillState.active)
            {
                state = SkillState.Idle;
                _caster.StopCoroutine(holderCorutine);
                holderCorutine = null;
            }
        }
    }
}