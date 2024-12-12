using UnityEngine;
using Unity.Netcode;
using AshGreen.Character.Player;
using System.Collections.Generic;
using AshGreen.Character;
using System.Linq;

namespace AshGreen.Debuff
{
    public class DebuffManager : NetworkBehaviour
    {
        public Transform _debuffUICanvas;// 디버프 캔버스
        public GameObject _debuffIconPre;// 디버프 아이콘 프리팹
        private EnemyController enemy;// 플레이어 컨트롤러
        [SerializeField]
        private List<DebuffData> debuffDatas = new List<DebuffData>();// 버프 데이터를 저장하는 리스트
        public Dictionary<DebuffType, Debuff> activeDebuffs =
            new Dictionary<DebuffType, Debuff>(); // 활성화된 버프를 저장하는 딕셔너리

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            enemy = transform.parent?.GetComponent<EnemyController>();// 플레이어 컨트롤러 컴포넌트를 가져옴
        }

        // 디버프 추가 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void AddDebuffRpc(DebuffType debuffType, int stack)
        {
            // 버프 딕셔너리에 해당 버프가 없다면 추가
            if (!activeDebuffs.ContainsKey(debuffType))
            {
                DebuffData debuff = debuffDatas.FirstOrDefault(x => x.debuffType == debuffType);
                activeDebuffs[debuffType] = new Debuff(debuff, enemy, stack);
                // 버프 적용
                activeDebuffs[debuffType].Apply();
            }
            else
                activeDebuffs[debuffType].Reapply(stack);
        }

        // 버프 제거 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void RemoveDebuffRpc(DebuffType debuffType)
        {
            if (activeDebuffs.ContainsKey(debuffType))
            {
                activeDebuffs[debuffType].Remove();
                activeDebuffs.Remove(debuffType);
            }
        }

        private void Update()
        {
            // activeBuffs.Values를 복사하여 새로운 리스트를 만듭니다.
            var debuffsToUpdate = activeDebuffs.Values.ToList();

            foreach (var debuff in debuffsToUpdate)
            {
                debuff.Update(Time.deltaTime);
            }
        }
    }
}