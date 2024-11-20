using AshGreen.Character.Player;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.Netcode;

namespace AshGreen.Item
{
    public class ItemSelectManager : MonoBehaviour
    {
        private PlayerController m_playerController = null;//로컬 플레이어 컨트롤러

        public ItemDataList itemDataList;//아이템 데이터 리스트
        [SerializeField]
        private List<ItemBtnController> m_btnControllerList
            = new List<ItemBtnController>();//아이템 버튼 컨트롤러 리스트

        [SerializeField]
        private TextMeshProUGUI m_haveMoney;//소지금 텍스트

        //활성화 시 아이템리스트 설정
        private void OnEnable()
        {
            m_playerController = FindLocalPlayer();
            // m_btnControllerList의 갯수만큼 랜덤으로 중복되지 않게 요소를 뽑아서 새로운 리스트 생성
            List<ItemData> randomItemDataList = itemDataList.dataList
                .OrderBy(x => Random.value)
                .Take(m_btnControllerList.Count)
                .ToList();

            // 새로운 리스트를 사용하여 추가 작업 수행
            // 예: 버튼 컨트롤러에 아이템 데이터 설정
            for (int i = 0; i < m_btnControllerList.Count; i++)
            {
                if (randomItemDataList.Count <= i)
                    break;
                m_btnControllerList[i].SetItemData(randomItemDataList[i], m_playerController);
            }
        }

        private void Update()
        {
            if (m_playerController != null)
                m_haveMoney.text = m_playerController.Money.ToString();
        }

        // 클라이언트의 플레이어 컨트롤러 찾기
        private PlayerController FindLocalPlayer()
        {
            //오너 캐릭터가 이미 있는지 체크
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            GameObject player = players
                .Select(p => p.GetComponent<NetworkObject>())
                .FirstOrDefault(n => n != null &&
                n.OwnerClientId == NetworkManager.Singleton.LocalClientId)?.gameObject;

            return player?.GetComponent<PlayerController>();
        }
    }
}

