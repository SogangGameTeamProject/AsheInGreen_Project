using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Item
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Scriptable Objects/Item/ItemDataList")]
    public class ItemDataList : ScriptableObject
    {
        public List<ItemData> commonItemList = new List<ItemData>();
        public float commonItemRate = 0.5f;
        public List<ItemData> rareItemList = new List<ItemData>();
        public float rareItemRate = 0.35f;
        public List<ItemData> epicItemList = new List<ItemData>();
        public float epicItemRate = 0.1f;
        public List<ItemData> legendaryList = new List<ItemData>();
        public float legendaryItemRate = 0.05f;
        public List<ItemData> otherList = new List<ItemData>();

        // 아이템 데이터 리스트
        public List<ItemData> DataList
        {
            get
            {
                var dataList = new List<ItemData>();
                dataList.AddRange(commonItemList);
                dataList.AddRange(rareItemList);
                dataList.AddRange(epicItemList);
                dataList.AddRange(legendaryList);
                dataList.AddRange(otherList);
                return dataList;
            }
        }

        // 랜덤 아이템 반환
        public List<ItemData> GetRandomItems(int returnNum)
        {
            var randomItems = new List<ItemData>();
            var usedItems = new HashSet<ItemData>();
            System.Random random = new System.Random();

            for (int i = 0; i < returnNum; i++)
            {
                int rarityRoll = random.Next(100); // 0부터 99까지의 랜덤 숫자 생성

                List<ItemData> selectedList;

                if (rarityRoll < commonItemRate * 100) // commonItemList 확률
                {
                    selectedList = commonItemList;
                }
                else if (rarityRoll < (commonItemRate + rareItemRate) * 100) // rareItemList 확률
                {
                    selectedList = rareItemList;
                }
                else if (rarityRoll < (commonItemRate + rareItemRate + epicItemRate) * 100) // epicItemList 확률
                {
                    selectedList = epicItemList;
                }
                else // legendaryList 확률
                {
                    selectedList = legendaryList;
                }

                // 선택된 리스트에서 랜덤한 아이템 하나를 추가
                if (selectedList.Count > 0)
                {
                    ItemData selectedItem;
                    do
                    {
                        int itemIndex = random.Next(selectedList.Count);
                        selectedItem = selectedList[itemIndex];
                    }
                    while (usedItems.Contains(selectedItem) && usedItems.Count < DataList.Count);

                    if (usedItems.Contains(selectedItem))
                    {
                        i--;
                        continue;
                    }

                    randomItems.Add(selectedItem);
                    usedItems.Add(selectedItem);
                }
                else
                {
                    i--;
                }
            }

            return randomItems;
        }
    }
}
