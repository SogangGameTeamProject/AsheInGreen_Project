using UnityEngine;

namespace AshGreen.Item
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;//아이템 명
        public string description;//설명
        public Sprite icon;// 아이콘
        public bool hasCooldown;//쿨다운이 있는지
        public float cooldownTime;//쿨다운 시간
        public float baseEffectValue;//기본 효과 값
        public float effectIncrementPerStack;//스택당 증가 효과 값
    }
}
