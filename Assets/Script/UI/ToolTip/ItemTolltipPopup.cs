using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AshGreen.Item;
using AshGreen.Character.Player;
using AshGreen.Character;

namespace AshGreen.UI
{
    public class ItemTolltipPopup : MonoBehaviour
    {
        [HideInInspector]
        public ItemData itemData;//아이템 데이터

        //아이템 툴팁 팝업
        [SerializeField]
        private TextMeshProUGUI itemNameTxt;
        [SerializeField]
        private TextMeshProUGUI itemCoolTimeTxt;
        [SerializeField]
        private TextMeshProUGUI itemDecoTxt;

        //버프디버프 팝업
        [SerializeField]
        private GameObject effectPopup;
        [SerializeField]
        private TextMeshProUGUI effectNameTxt;
        [SerializeField]
        private Image effectIconImg;
        [SerializeField]
        private TextMeshProUGUI effectDurationTxt;
        [SerializeField]
        private TextMeshProUGUI effectDecoTxt;


        public void SetItemTooltip(ItemData data, int stacks)
        {
            itemData = data;
            itemNameTxt.text = itemData.itemName;
            itemCoolTimeTxt.text = itemData.cooldownTime > 0 ? itemData.cooldownTime.ToString() : "없음";
            itemDecoTxt.text = itemData.DescriptionTxt(stacks > 0 ? stacks : 1);

            if(itemData.buffData != null)
            {
                effectNameTxt.text = itemData.buffData.buffName;
                effectIconImg.sprite = itemData.buffData.buffIcon;
                effectDurationTxt.text = itemData.buffData.duration > 0 ?
                    itemData.buffData.duration.ToString() : "없음";
                effectDecoTxt.text = itemData.buffData.buffDescription;
            }
            else if(itemData.debuffData != null)
            {
                effectNameTxt.text = itemData.debuffData.debuffName;
                effectIconImg.sprite = itemData.debuffData.debuffIcon;
                effectDurationTxt.text = itemData.debuffData.duration > 0 ?
                    itemData.debuffData.duration.ToString() : "없음";
                effectDecoTxt.text = itemData.debuffData.debuffDescription;
            }
            else
            {
                effectPopup.SetActive(false);
            }
        }
    }
}