using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace AshGreen.UI
{
    public class BuffTimer : MonoBehaviour
    {
        private Buff.Buff buff;//표시할 디버프 정보
        [SerializeField]
        private Image buffIcon;//디버프 아이콘
        [SerializeField]
        private Image buffTimerIcon;//디버프 타이머 아이콘

        public void SetDebuff(Buff.Buff buff)
        {
            this.buff = buff;
            buffIcon.sprite = buff.buffData.buffIcon;
            buffTimerIcon.sprite = buff.buffData.buffIcon;
        }

        private void Update()
        {
            UpdateTimer();
        }

        public void UpdateTimer()
        {
            //남은 지속시간에 따라 디버프 아이콘 fillAmount 조정
            buffTimerIcon.fillAmount = buff.remainingDuration / buff.buffData.duration;
        }
    }
}
