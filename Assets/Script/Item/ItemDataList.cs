using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Item
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Scriptable Objects/Item/ItemDataList")]
    public class ItemDataList : ScriptableObject
    {
        public List<ItemData> dataList = new List<ItemData>();
    }
}
