using AshGreen.Buff;
using AshGreen.DamageObj;
using AshGreen.Debuff;
using AshGreen.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character.Skill
{
    public enum SkillType
    {
        MainSkill = 0, SecondarySkill = 1, SpecialSkill = 2, PassiveSkill = 3
    }

    public abstract class CharacterSkill : ScriptableObject
    {
        public string skillName;   // 스킬 이름
        public SkillType skillType;//스킬 타입
        [TextArea(3, 10)]
        public string skillDescription; // 스킬 설명
        //설명을 반환하는 메소드
        public string DescriptionTxt()
        {
            string returnDeco = skillDescription;
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<string, string> fieldValues = new Dictionary<string, string>();

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);
                if (value is IList<float> list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        fieldValues[$"{field.Name}[{i}]"] = list[i].ToString();
                    }
                }
                else
                {
                    fieldValues[field.Name] = value?.ToString();
                }
            }

            // 연산식을 찾기 위한 정규식
            string operationPattern = @"{([^{}]+)}";
            Match operationMatch = Regex.Match(returnDeco, operationPattern);
            while (operationMatch.Success)
            {
                string expression = operationMatch.Groups[1].Value;
                string evaluatedExpression = EvaluateExpression(expression, fieldValues);
                returnDeco = returnDeco.Replace(operationMatch.Value, evaluatedExpression);
                operationMatch = operationMatch.NextMatch();
            }

            return returnDeco;
        }

        public float activeTime;   // 스킬 사용 시간
        public float cooldownTime; // 스킬 쿨타임
        public int maxChageCnt = 1;// 최대 충전 수

        //코스트 관련
        public int MaxHaveEnergy = 0;
        public int MinUseCoast = 0;

        public bool charging = false; //스킬 차징 여부
        public float chargingMoveSpeed = 0.75f; // 차징 시 이속

        //캔슬 여부
        public bool isNotCancellation = false; // 해당 스킬 캔슬 불가능 여불
        public bool skillCancel = false;       // 타 스킬 캔슬 여부
        public bool multipleUse = false;       // 다중 사용 가능 여부


        //공격 스킬 시 설정
        public float damageCoefficient = 1; // 스킬 데미지 배수

        //유틸 스킬 시 설정
        public float utillValue = 1;
        public float utillTime = 1;

        public Sprite skillIcon;   // 스킬 아이콘
        public string animationTrigger; // 스킬 발동 시 애니메이션 트리거
        public AudioClip skillSound;    // 스킬 발동 소리

        public BuffData buffData; // 버프 데이터
        public DebuffData debuffData; // 디버프 데이터

        //스킬 차징
        public virtual IEnumerator Charging(SkillHolder holder)
        {
            holder.state = SkillHolder.SkillState.charge;//차징 상태 전환
            float charginTime = 0;

            while (true)
            {
                charginTime += Time.deltaTime;
                if (holder.state == SkillHolder.SkillState.active)
                    break;
                yield return null;
            }

            holder.holderCorutine = holder._caster.StartCoroutine(Use(holder, charginTime));//차징 후 
        }

        //스킬 사용
        public virtual IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            holder.holderCorutine = holder._caster.StartCoroutine(End(holder));
            yield return null;
        }

        //스킬 종료 처리
        public virtual IEnumerator End(SkillHolder holder)
        {
            holder._caster.EndSkillAni();
            holder.state = SkillHolder.SkillState.Idle;

            yield return null;
        }

        private string EvaluateExpression(string expression, Dictionary<string, string> fieldValues)
        {
            foreach (var field in fieldValues)
            {
                expression = expression.Replace(field.Key, field.Value);
            }

            // DataTable을 사용하여 수식을 계산
            var dataTable = new System.Data.DataTable();
            var result = dataTable.Compute(expression, null);
            return result.ToString();
        }
    }
}

 