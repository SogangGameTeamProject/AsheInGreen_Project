using UnityEngine;
using UnityEngine.UI;
using AshGreen.Debuff;
using Unity.VisualScripting;

namespace AshGreen.UI
{
    public class DebuffTimer : MonoBehaviour
    {
        private Debuff.Debuff debuff;//표시할 디버프 정보
        [SerializeField]
        private Image debuffIcon;//디버프 아이콘
        [SerializeField]
        private Image debuffTimerIcon;//디버프 타이머 아이콘

        public void SetDebuff(Debuff.Debuff debuff)
        {
            this.debuff = debuff;
            debuffIcon.sprite = debuff.debuffData.debuffIcon;
            debuffTimerIcon.sprite = debuff.debuffData.debuffIcon;
        }

        private void Update()
        {
            UpdateTimer();
        }

        public void UpdateTimer()
        {
            //남은 지속시간에 따라 디버프 아이콘 fillAmount 조정
            debuffTimerIcon.fillAmount = debuff.remainingDuration / debuff.debuffData.duration;
        }
    }
}
