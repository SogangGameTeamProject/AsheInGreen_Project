using UnityEngine;
using Unity.Netcode;
using AshGreen.Obsever;
using System.Collections.Generic;
using AshGreen.Character.Player;
using System.Linq;
using System;

namespace AshGreen.Buff
{
    public class BuffManager : NetworkBehaviour
    {
        private PlayerController playerController;// 플레이어 컨트롤러
        [SerializeField]
        private List<BuffData> buffDatas = new List<BuffData>();// 버프 데이터를 저장하는 리스트
        public Dictionary<BuffType, Buff> activeBuffs =
            new Dictionary<BuffType, Buff>(); // 활성화된 버프를 저장하는 딕셔너리

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            playerController = transform.parent?.GetComponent<PlayerController>();// 플레이어 컨트롤러 컴포넌트를 가져옴
        }

        // 버프 추가 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void AddBuffRpc(BuffType buffType, int stack, float[] baseVal, float[] stackVal)
        {
            // 버프 딕셔너리에 해당 버프가 없다면 추가
            if (!activeBuffs.ContainsKey(buffType))
            {
                BuffData buff = buffDatas.FirstOrDefault(x => x.buffType == buffType);
                activeBuffs[buffType] = new Buff(buff, playerController, stack, baseVal, stackVal);
            }
            else
                activeBuffs[buffType].Remove();
            // 버프 적용
            activeBuffs[buffType].Apply();
        }

        // 버프 제거 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void RemoveBuffRpc(BuffType buffType)
        {
            activeBuffs[buffType].Remove();
            activeBuffs.Remove(buffType);
        }

        private void Update()
        {
            // activeBuffs.Values를 복사하여 새로운 리스트를 만듭니다.
            var buffsToUpdate = activeBuffs.Values.ToList();

            foreach (var buff in buffsToUpdate)
            {
                buff.Update(Time.deltaTime);
            }
        }
    }
}
