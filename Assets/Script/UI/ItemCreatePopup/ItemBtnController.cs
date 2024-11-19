using AshGreen.Character.Player;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace AshGreen.Item
{
    public class ItemBtnController : MonoBehaviour
    {
        private PlayerController m_playerController;//로컬 플레이어 컨트롤러
        private ItemData m_itemData;//아이템 데이터
        private bool isCreate = false;//아이템 생성 여부
        [SerializeField]
        private Sprite soldOutImg = null;//아이템 판매 완료 이미지

        //아이템 UI
        [SerializeField]
        private Image itemImg;// 아이템 이미지
        [SerializeField]
        private TextMeshProUGUI itemName;//아이템 이름
        [SerializeField]
        private TextMeshProUGUI itemPrice;//아이템 가격

        //아이템 데이터 설정
        public void SetItemData(ItemData itemData)
        {
            m_playerController = FindLocalPlayer();
            m_itemData = itemData;
            itemImg.sprite = m_itemData.icon;
            itemImg.gameObject.SetActive(true);
            itemName.text = m_itemData.name;
            itemPrice.text = m_itemData.price.ToString();
        }

        //아이템 제작
        public void CreateItem()
        {
            //아이템 제작 가능여부 제크
            if (isCreate && m_playerController.Money >= m_itemData.price)
                return;
            m_playerController.Money -= m_itemData.price;//돈 차감
            itemImg.sprite = soldOutImg;//아이템 판매 완료 이미지로 변경
            isCreate = true;//아이템 생성 완료
            m_playerController.itemManager.AddItemRpc(m_itemData.itemID);//아이템 추가
        }

        // 클라이언트의 플레이어 컨트롤러 찾기
        private PlayerController FindLocalPlayer()
        {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")
            .Select(p => p.GetComponent<NetworkObject>())
            .FirstOrDefault(n => n != null &&
            n.OwnerClientId == NetworkManager.Singleton.LocalClientId)?.gameObject;

            return player?.GetComponent<PlayerController>();
        }
    }
}