using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AshGreen.Item
{
    //아이템 타입
    public enum ItemType: byte
    {
        Common,//일반
        Rare,//희귀
        Epic,//에픽
        Legendary,//전설
        Other//기타
    }

    [CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Item/ItemData")]
    public class ItemData : ScriptableObject
    {
        public int itemID;//아이템 ID
        public ItemType type;//아이템 타입
        public GameObject itemObj;//아이템 오브젝트
        public string itemName;//아이템 명
        [SerializeField, TextArea]
        private string description;//설명
        //설명을 반환하는 메소드
        public string DescriptionTxt()
        {
            string returnDeco = description;
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

        public Buff.BuffData buffData;//버프 데이터
        public Debuff.DebuffData debuffData;//디버프 데이터

        public int price = 15;//가격
        public Sprite icon;// 아이콘
        public bool hasCooldown;//쿨다운이 있는지
        public float cooldownTime;//쿨다운 시간
        public int activeNum = 0;//발동 조건
        public List<float> baseVal = new List<float>();//기본 효과 값
        public List<float> stackIncVal = new List<float>();//스택당 증가 효과 값

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
