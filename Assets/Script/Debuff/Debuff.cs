using AshGreen.Character;
using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Debuff
{
    public class Debuff
    {
        public EnemyController _targetEnemy;// 버프 대상 플레이어
        public DebuffData debuffData;// 버프 데이터
        public float remainingDuration;// 남은 지속 시간
        public float currentTimer;// 현재 타이머
        public int currentStacks;// 현재 중첩 수
        public float baseVal = 0;
        public float stackVal = 0;

        // 버프 생성자
        public Debuff(DebuffData data, EnemyController targetEnemy, int stack, float baseVal = 0, float stackVal = 0)
        {
            debuffData = data;
            _targetEnemy = targetEnemy;
            currentStacks = stack;
            this.baseVal = baseVal;
            this.stackVal = stackVal;
        }

        public void Apply()
        {
            remainingDuration = debuffData.duration;
            currentTimer = 0;
            debuffData.ApplyDebuff(_targetEnemy, this);
        }

        public void Remove()
        {
            debuffData.RemoveDebuff(_targetEnemy, this);
        }

        public void Update(float deltaTime)
        {
            if (debuffData.durationType == DebuffDurationType.Timed)
            {
                remainingDuration -= deltaTime;
                currentTimer += deltaTime;
                if (remainingDuration <= 0)
                    _targetEnemy.debuffManager.RemoveDebuffRpc(debuffData.debuffType);
            }
        }
    }
}
