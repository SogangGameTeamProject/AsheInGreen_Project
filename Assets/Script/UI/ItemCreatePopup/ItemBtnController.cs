using AshGreen.Character.Player;
using AshGreen.Sound;
using AshGreen.UI;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace AshGreen.Item
{
    public class ItemBtnController : MonoBehaviour
    {
        public PlayerController m_playerController;//로컬 플레이어 컨트롤러
        private ItemData m_itemData;//아이템 데이터
        private bool isCreate = false;//아이템 생성 여부
        [SerializeField]
        private Sprite soldOutImg = null;//아이템 판매 완료 이미지
        
        [SerializeField]
        private AudioClip createSound = null;

        //아이템 UI
        [SerializeField]
        private Image itemImg;// 아이템 이미지
        [SerializeField]
        private TextMeshProUGUI itemName;//아이템 이름
        [SerializeField]
        private TextMeshProUGUI itemPrice;//아이템 가격

        [SerializeField]
        private OpenItemTooltipPopup _openItemTooltipPopup;

        //아이템 데이터 설정
        public void SetItemData(ItemData itemData, PlayerController player)
        {
            m_playerController = player;
            m_itemData = itemData;
            itemImg.sprite = m_itemData.icon;
            itemImg.gameObject.SetActive(true);
            itemName.text = m_itemData.itemName;
            itemPrice.text = m_itemData.price.ToString();

            _openItemTooltipPopup.itemData = m_itemData;//아이템 툴팁 데이터 설정

            //해당 아이템 스텍 구하기
            int itemStack = 0;
            if (player.itemManager.itemInventory.ContainsKey(itemData.itemID))
            {
                itemStack = player.itemManager.itemInventory[itemData.itemID]._stacks;
            }
            _openItemTooltipPopup.itemStack = itemStack;//아이템 스텍 설정
        }

        //아이템 제작
        public void CreateItem()
        {
            //아이템 제작 가능여부 제크
            if (isCreate || m_playerController.Money < m_itemData.price)
                return;
            if(createSound)
                SoundManager.Instance.PlaySFX(createSound);//아이템 제작 사운드
            m_playerController.AddMoneyServerRpc(-m_itemData.price);//돈 차감
            itemImg.sprite = soldOutImg;//아이템 판매 완료 이미지로 변경
            isCreate = true;//아이템 생성 완료
            m_playerController.itemManager.AddItemRpc(m_itemData.itemID);//아이템 추가
        }
    }
}