using AshGreen.Character.Player;
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
            itemName.text = m_itemData.name;
            itemPrice.text = m_itemData.price.ToString();
        }

        //아이템 생성
        public void CreateItem()
        {
            if (isCreate)
                return;

            m_playerController.itemManager.AddItemRpc(m_itemData.itemID);
        }

        // 클라이언트의 플레이어 컨트롤러 찾기
        private PlayerController FindLocalPlayer()
        {
            PlayerController playerController = null;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                NetworkObject networkObject = player?.GetComponent<NetworkObject>();
                if (networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
                    playerController = player?.GetComponent<PlayerController>();
            }
            return playerController;
        }
    }
}

