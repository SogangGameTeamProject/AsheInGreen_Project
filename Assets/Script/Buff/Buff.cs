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

        public Buff(BuffData data, PlayerController targetPlayer)
        {
            buffData = data;
            if (data.durationType == BuffDurationType.Timed)
            {
                remainingDuration = data.duration;
            }
            else if (data.durationType == BuffDurationType.StackBased)
            {
                currentStacks = data.maxStacks;
            }

            _targetPlayer = targetPlayer;
        }

        public void Refresh()
        {
            if (buffData.durationType == BuffDurationType.Timed)
            {
                remainingDuration = buffData.duration;
            }
            else if (buffData.durationType == BuffDurationType.StackBased)
            {
                currentStacks = buffData.maxStacks;
            }
        }

        public void Update(float deltaTime)
        {
            if (buffData.durationType == BuffDurationType.Timed)
            {
                remainingDuration -= deltaTime;
            }
        }

        public bool IsExpired()
        {
            if (buffData.durationType == BuffDurationType.Timed)
            {
                return remainingDuration <= 0;
            }
            else if (buffData.durationType == BuffDurationType.StackBased)
            {
                return currentStacks <= 0;
            }
            return false;
        }

        public void DecreaseStack()
        {
            if (buffData.durationType == BuffDurationType.StackBased)
            {
                currentStacks--;
            }
        }
    }
}
