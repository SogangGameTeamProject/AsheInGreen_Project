using AshGreen.Character.Player;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Item
{
    public class ItemSelectManager : MonoBehaviour
    {
        public ItemDataList itemDataList;//아이템 데이터 리스트
        [SerializeField]
        private List<ItemBtnController> m_btnControllerList
            = new List<ItemBtnController>();//아이템 버튼 컨트롤러 리스트
    }
}

