using AshGreen.Buff;
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
        public int currentDamage; //
        public float[] baseVal;
        public float[] stackVal;

        // 버프 생성자
        public Debuff(DebuffData data, EnemyController targetEnemy, int stack, float[] baseVal, float[] stackVal)
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
            currentDamage = 0;
            debuffData.ApplyDebuff(_targetEnemy, this);
        }

        public void Remove()
        {
            debuffData.RemoveDebuff(_targetEnemy, this);
        }

        // 버프 재적용
        public void Reapply(int stack)
        {
            if (currentStacks >= stack)
            {
                currentStacks = stack;

                if (_targetEnemy.IsOwner)
                {
                    debuffData.RemoveDebuff(_targetEnemy, this);
                    debuffData.ApplyDebuff(_targetEnemy, this);
                }
            }
            else
            {
                remainingDuration = debuffData.duration;
            }
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

            if (_targetEnemy.IsOwner)
                debuffData.UpdateDebuff(_targetEnemy, this);
        }

        public void saveDamage(float damage)
        {
            currentDamage += (int)damage;
        }
    }
}
