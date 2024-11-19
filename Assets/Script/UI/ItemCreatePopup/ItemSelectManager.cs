using AshGreen.Character.Player;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AshGreen.Item
{
    public class ItemSelectManager : MonoBehaviour
    {
        public ItemDataList itemDataList;//아이템 데이터 리스트
        [SerializeField]
        private List<ItemBtnController> m_btnControllerList
            = new List<ItemBtnController>();//아이템 버튼 컨트롤러 리스트

        //활성화 시 아이템리스트 설정
        private void OnEnable()
        {
            // m_btnControllerList의 갯수만큼 랜덤으로 중복되지 않게 요소를 뽑아서 새로운 리스트 생성
            List<ItemData> randomItemDataList = itemDataList.dataList
                .OrderBy(x => Random.value)
                .Take(m_btnControllerList.Count)
                .ToList();

            // 새로운 리스트를 사용하여 추가 작업 수행
            // 예: 버튼 컨트롤러에 아이템 데이터 설정
            for (int i = 0; i < m_btnControllerList.Count; i++)
            {
                m_btnControllerList[i].SetItemData(randomItemDataList[i]);
            }
        }
    }
}

