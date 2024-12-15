using AshGreen.Character;
using AshGreen.Character.Player;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace AshGreen.Debuff
{
    //
    public enum DebuffType
    {
        Burn = 0,// 화상
        Corruption,// 부패
        PartDestruction,// 부위파괴 
        Wound,// 상처
        breakdown// 붕괴
    }

    public enum DebuffDurationType
    {
        Timed, // 지속 시간에 따른 디버프
        StackBased // 특정 행동 시 스택이 감소하는 디버프
    }

    public abstract class DebuffData : ScriptableObject
    {
        public DebuffType debuffType;// 디버프 타입
        public string debuffName;// 디버프 이름
        public Sprite debuffIcon;// 디버프 아이콘
        [SerializeField, TextArea]
        private string debuffDescription;// 디버프 설명
        //설명을 반환하는 메소드
        public string DescriptionTxt()
        {
            string returnDeco = debuffDescription;
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

        public DebuffDurationType durationType;// 디버프 지속 시간 타입
        public float duration; // 디버프 지속 시간 (Timed 타입일 경우)
        public float activationCycle = 0;// 디버프 활성화 주기
        public List<float> baseVal = new List<float>();//기본 효과 값
        public List<float> stackIncVal = new List<float>();//스택당 증가 효과 값

        // 디버프 적용 메서드
        public abstract void ApplyDebuff(EnemyController enemy, Debuff debuff);
        // 디버프 업데이트 메서드
        public abstract void UpdateDebuff(EnemyController enemy, Debuff debuff);
        // 디버프 재적용 메서드
        public abstract void ReapplyDebuff(EnemyController enemy, Debuff debuff);

        // 디버프 제거 메서드
        public abstract void RemoveDebuff(EnemyController enemy, Debuff debuff);

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