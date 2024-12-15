using AshGreen.Character.Player;
using AshGreen.Item;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AshGreen.UI
{
    public class OpenItemTooltipPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //아이템 툴팁 팝업
        [SerializeField]
        private GameObject _itemTooltipPopup;
        //아이템 툴팁 위치
        [SerializeField]
        private Vector2 _offset;
        //아이템 데이터
        [HideInInspector]
        public ItemData itemData;
        [HideInInspector]
        public int itemStack;
        //아이템 툴팁
        private GameObject tooltip = null;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenItemTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseItemTooltip();
        }

        public void OpenItemTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip);
                tooltip = null;
            }

            tooltip = Instantiate(_itemTooltipPopup, transform);
            tooltip.transform.localPosition = _offset;
            tooltip.transform.parent = transform.parent.parent.parent.parent.parent;

            //아이템 툴팁 설정
            tooltip.GetComponent<ItemTolltipPopup>().SetItemTooltip(itemData, itemStack);
        }

        public void CloseItemTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip);
                tooltip = null;
            }
        }
    }
}