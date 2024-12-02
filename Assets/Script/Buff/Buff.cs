using AshGreen.Character;
using AshGreen.Character.Player;
using AshGreen.Debuff;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Buff
{
    public class Buff
    {
        public PlayerController _targetPlayer;// 버프 대상 플레이어
        public BuffData buffData;// 버프 데이터
        public float remainingDuration;// 남은 지속 시간
        public float currentTimer;// 현재 타이머
        public int currentStacks;// 현재 중첩 수
        public float[] baseVal;
        public float[] stackVal;

        // 버프 생성자
        public Buff(BuffData data, PlayerController targetPlayer, int stack, float[] baseVal, float[] stackVal)
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
            currentTimer = 0;
            if(_targetPlayer.IsOwner)
                buffData.ApplyBuff(_targetPlayer, this);
        }

        public void Remove()
        {
            if (_targetPlayer.IsOwner)
                buffData.RemoveBuff(_targetPlayer, this);
        }

        // 버프 재적용
        public void Reapply(int stack)
        {
            if(currentStacks < stack)
            {
                currentStacks = stack;

                if (_targetPlayer.IsOwner)
                {
                    buffData.RemoveBuff(_targetPlayer, this);
                    buffData.ApplyBuff(_targetPlayer, this);
                }
            }
            else
            {
                remainingDuration = buffData.duration;
            }
        }

        public void Update(float deltaTime)
        {
            if (buffData.durationType == BuffDurationType.Timed)
            {
                remainingDuration -= deltaTime;
                currentTimer += deltaTime;
                if (remainingDuration <= 0 && _targetPlayer.IsOwner)
                    _targetPlayer.buffManager.RemoveBuffRpc(buffData.buffType);
            }

            if(_targetPlayer.IsOwner)
                buffData.UpdateBuff(_targetPlayer, this);
        }
    }
}
