using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Character.Skill
{
    public class SkillHolder : NetworkBehaviour
    {
        public SkillHolder(CharacterController caster, CharacterSkill skill)
        {
            _caster = caster;
            this.skill = skill;
            nowChargeCnt = skill.maxChageCnt;
            state = SkillState.ready;
        }

        public CharacterController _caster = null;

        //스킬 상태
        public enum SkillState
        {
            ready,//준비됨
            charge,//차징 중
            active//사용 중
        }

        private CharacterSkill skill = null;
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
        public SkillState state { get; set; }
        
        public Coroutine holderCorutine = null;

        private void Update()
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
            state = SkillState.charge;
            holderCorutine = StartCoroutine(skill.Chargin(this));
        }

        //스킬 사용 메서드
        public void Use()
        {
            state = SkillState.active;
            holderCorutine = StartCoroutine(skill.Use(this));
        }

        //스킬 정지
        public void Stop()
        {
            if(state == SkillState.active)
            {
                state = SkillState.ready;
                StopCoroutine(holderCorutine);
                holderCorutine = null;
            }
        }

        public void PresseSkill()
        {
            if (state != SkillState.ready || nowChargeCnt > 0)
                return;
            nowChargeCnt--;//사용 횟수 차감
            if (skill.charging)
            {
                Charging();
            }
            else
            {
                Use();
            }
        }

        public void ReleaseSkill()
        {
            if (state == SkillState.charge)
                state = SkillState.active;
        }
    }
}