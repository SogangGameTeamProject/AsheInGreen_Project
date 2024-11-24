using AshGreen.Character.Player;
using NUnit.Framework.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Item
{
    public abstract class ItemEffectInit : MonoBehaviour
    {
        public ItemData itemData;
        protected PlayerController _playerController;
        [HideInInspector]
        public int _stacks = 0;

        //아이템 효과 적용
        public virtual void ApplyEffect(PlayerController player)
        {
            if(_playerController == null)
                _playerController = player;
        }

        // 아이템 효과 추가
        public virtual void AddEffect()
        {
            _stacks++;
        }

        // 아이템 효과 제거
        public virtual void RemoveEffect()
        {
            _stacks--;
        }
    }
}
