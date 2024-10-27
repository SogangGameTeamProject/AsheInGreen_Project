using AshGreen.Character.Player;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace AshGreen.Character.Skill
{
    [System.Serializable]
    public class SkillHolder
    {
        //홀더 생성자
        public SkillHolder(PlayerController caster, CharacterSkill skill)
        {
            _caster = caster;
            this.skill = skill;
            MaxChargeCnt = skill.maxChageCnt;
            nowChargeCnt = skill.maxChageCnt;
            coolTime = skill.cooldownTime;
            state = SkillState.Idle;
            maxHaveEnergy = skill.MaxHaveEnergy;
            minUseCoast = skill.MinUseCoast;
        }

        public PlayerController _caster = null;

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
        public int maxHaveEnergy = 0;//최대 충전 에너지
        public int minUseCoast = 0;//사용시 최소 소모 코스트
        private int nowEnergy = 0;//현재 에너지 량
        public int NowEnergy
        {
            get
            {
                return nowEnergy;
            }
            set
            {
                nowEnergy = Mathf.Clamp(value, 0, maxHaveEnergy);
            }
        }
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
            float realCooltime = coolTime * (100 / (100 + _caster.SkillAcceleration));
            //쿨타임 적용
            if (nowChargeCnt < skill.maxChageCnt)
            {
                if (currentCoolTime < realCooltime)
                {
                    currentCoolTime += Time.deltaTime;
                }
                else
                    NowChargeCnt++;
            }
            _caster.SkillInfoUpdateHUDRPC(skill.skillType, NowChargeCnt > 0 ? 0 : realCooltime - currentCoolTime, skill.MinUseCoast, nowEnergy);
        }

        //스킬 차징 메서드
        public void Charging()
        {
            holderCorutine = _caster.StartCoroutine(skill.Charging(this));
        }

        //스킬 사용 메서드
        public void Use()
        {
            Debug.Log("스킬 쿨타임"+coolTime * (100 / (100 + _caster.SkillAcceleration)));
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