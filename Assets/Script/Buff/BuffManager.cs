using UnityEngine;
using Unity.Netcode;
using AshGreen.Obsever;
using System.Collections.Generic;

namespace AshGreen.Buff
{
    public class BuffManager : NetworkBehaviour
    {
        [SerializeField]
        private List<BuffData> buffDatas = new List<BuffData>();// 버프 데이터를 저장하는 리스트
        private Dictionary<BuffType, Buff> activeBuffs =
            new Dictionary<BuffType, Buff>(); // 활성화된 버프를 저장하는 딕셔너리

        [ServerRpc]
        private void AddBuffServerRpc(BuffType buffType)
        {
            if (activeBuffs.ContainsKey(buffType))
            {
                if (activeBuffs[buffType].buffData.isStackable)
                {
                    // 중첩 가능한 버프의 경우 효과 강화 로직 추가
                    // 예: activeBuffs[buffType].Stack();
                }
                else
                {
                    // 중첩 불가능한 버프의 경우 지속 시간 갱신
                    activeBuffs[buffType].Refresh();
                }
            }
            else
            {
                
            }
        }

        
        private void UpdateBuffsC()
        {
            // 클라이언트에서 버프 상태를 업데이트하는 로직 추가
        }

        public void UpdateBuffs(float deltaTime)
        {
            if (IsServer)
            {
                List<BuffType> expiredBuffs = new List<BuffType>();

                foreach (var buff in activeBuffs.Values)
                {
                    buff.Update(deltaTime);
                    if (buff.IsExpired())
                    {
                        expiredBuffs.Add(buff.buffData.buffType);
                    }
                }

                foreach (var buffType in expiredBuffs)
                {
                    activeBuffs.Remove(buffType);
                }
            }
        }

        public void DecreaseBuffStack(BuffType buffType)
        {
            if (IsServer && activeBuffs.ContainsKey(buffType))
            {
                activeBuffs[buffType].DecreaseStack();
                if (activeBuffs[buffType].IsExpired())
                {
                    activeBuffs.Remove(buffType);
                }
            }
        }
    }
}
