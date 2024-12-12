using AshGreen.Character;
using AshGreen.Character.Player;
using AshGreen.Debuff;
using AshGreen.UI;
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

        private GameObject buffTimer = null;

        // 버프 생성자
        public Buff(BuffData data, PlayerController targetPlayer, int stack)
        {
            buffData = data;
            _targetPlayer = targetPlayer;
            currentStacks = stack;
        }

        public void Apply()
        {
            remainingDuration = buffData.duration;
            currentTimer = 0;

            // 디버프 타이머 UI 생성
            buffTimer = GameObject.Instantiate(_targetPlayer.buffManager._buffIconPre,
                _targetPlayer.buffManager._buffUICanvas);
            buffTimer?.GetComponent<BuffTimer>().SetDebuff(this);

            if (_targetPlayer.IsOwner)
                buffData.ApplyBuff(_targetPlayer, this);
        }

        public void Remove()
        {
            GameObject.Destroy(buffTimer);// 디버프 타이머 UI 제거
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
