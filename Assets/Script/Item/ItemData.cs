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
        //설명을 반환하는 프로퍼티
        public string Description
        {
            get
            {
                string returnDeco = description;
                FieldInfo[] fields = 
                    this.GetType().GetFields
                    (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    string placeholder = "{" + field.Name + "}";
                    if (returnDeco.Contains(placeholder))
                    {
                        object value = field.GetValue(this);
                        returnDeco = returnDeco.Replace(placeholder, value?.ToString());
                    }

                    // 배열 인덱스 패턴을 찾기 위한 정규식
                    string arrayPattern = @"{" + field.Name + @"\[(\d+)\]}";
                    Match match = Regex.Match(returnDeco, arrayPattern);
                    while (match.Success)
                    {
                        int index = int.Parse(match.Groups[1].Value);
                        if (field.GetValue(this) is IList<float> list && index < list.Count)
                        {
                            returnDeco = returnDeco.Replace(match.Value, list[index].ToString());
                        }
                        match = match.NextMatch();
                    }
                }
                return returnDeco;
            }
        }
        public int price = 15;//가격
        public Sprite icon;// 아이콘
        public bool hasCooldown;//쿨다운이 있는지
        public float cooldownTime;//쿨다운 시간
        public int activeNum = 0;//발동 조건
        public List<float> baseVal = new List<float>();//기본 효과 값
        public List<float> stackIncVal = new List<float>();//스택당 증가 효과 값
    }
}
