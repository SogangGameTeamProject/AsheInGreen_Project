using AshGreen.Character;
using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Buff
{
    public class Buff
    {
        public PlayerController _targetPlayer;// 버프 대상 플레이어
        public BuffData buffData;// 버프 데이터
        public float remainingDuration;// 남은 지속 시간
        public int currentStacks;// 현재 중첩 수
        public float baseVal = 0;
        public float stackVal = 0;

        // 버프 생성자
        public Buff(BuffData data, PlayerController targetPlayer, int stack, float baseVal = 0, float stackVal = 0)
        {
            buffData = data;
            _targetPlayer = targetPlayer;
            currentStacks = stack;
            this.baseVal = baseVal;
            this.stackVal = stackVal;
        }

        public void Apply()
        {
            remainingDuration = buffData.duration;
            buffData.ApplyBuff(_targetPlayer, this);
        }

        public void Remove()
        {
            buffData.RemoveBuff(_targetPlayer, this);
        }

        public void Update(float deltaTime)
        {
            if (buffData.durationType == BuffDurationType.Timed)
            {
                remainingDuration -= deltaTime;
                if (remainingDuration <= 0)
                    _targetPlayer.buffManager.RemoveBuffRpc(buffData.buffType);
            }
        }
    }
}
