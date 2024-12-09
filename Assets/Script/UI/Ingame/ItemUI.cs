using AshGreen.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AshGreen.UI
{
    public class ItemUI : MonoBehaviour
    {
        public ItemEffectInit _item;//아이템 객체
        [SerializeField]
        private Image _itemImage;//아이템 이미지
        [SerializeField]
        private TextMeshProUGUI _stackTxt;//아이템 스택 텍스트

        public void SetItemUI(ItemEffectInit item)
        {
            _item = item;
            _itemImage.sprite = item.itemData.icon;
            _stackTxt.text = item._stacks.ToString();
        }

        public void UpdateStack()
        {
            _stackTxt.text = _item._stacks.ToString();
        }

        public void RemoveItemUI()
        {
            Destroy(gameObject);
        }
    }
}
