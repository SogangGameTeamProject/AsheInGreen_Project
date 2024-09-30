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
            MaxChargeCnt = skill.maxChageCnt;
            nowChargeCnt = skill.maxChageCnt;
            coolTime = skill.cooldownTime;
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
        public int MaxChargeCnt = 0;// 최대 스킬 충전 횟수
        private int nowChargeCnt = 0;//현재 스킬 충전 횟수
        public int NowChargeCnt
        {
            get { return nowChargeCnt; }
            set
            {
                nowChargeCnt = Mathf.Clamp(value, 0, skill.maxChageCnt);
                currentCoolTime = 0;
            }
        }
        public float coolTime = 0;
        public float currentCoolTime { get; private set; }//지속 쿨타임 
        public SkillState state;//스킬 상태
        
        public Coroutine holderCorutine = null;

        public void Update()
        {
            //쿨타임 적용
            if(nowChargeCnt < skill.maxChageCnt)
            {
                if (currentCoolTime < coolTime)
                    currentCoolTime += Time.deltaTime;
                else
                    NowChargeCnt++;
            }

            ////피격 시 스킬 캔슬
            //if (state != SkillState.Idle && _caster.runningCombatStateType != CombatStateType.Idle)
            //{
            //    Stop();
            //}
        }

        //스킬 차징 메서드
        public void Charging()
        {
            holderCorutine = _caster.StartCoroutine(skill.Charging(this));
        }

        //스킬 사용 메서드
        public void Use()
        {
            NowChargeCnt--;
            if (state == SkillState.Idle)
                holderCorutine = _caster.StartCoroutine(skill.Use(this));
            else
                state = SkillState.active;
        }

        //스킬 정지 메서드
        public void Stop()
        {
             _caster.StopCoroutine(holderCorutine);
             holderCorutine = _caster.StartCoroutine(skill.End(this));
        }
    }
}