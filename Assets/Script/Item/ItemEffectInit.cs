using AshGreen.Character.Player;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace AshGreen.Item
{
    public abstract class ItemEffectInit : MonoBehaviour
    {
        public ItemData itemData;
        private PlayerController _playerController;
        public int _stacks = 0;

        // 아이템 효과 추가
        public abstract void AddEffect(PlayerController player);

        // 아이템 효과 제거
        public abstract void RemoveEffect();
    }
}
