using AshGreen.Character;
using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Debuff
{
    //
    public enum DebuffType
    {
        Burn = 0,// 화상
        Corruption,// 부패
        PartDestruction,// 부위파괴 
        Wound,// 상처
        breakdown// 붕괴
    }

    public enum DebuffDurationType
    {
        Timed, // 지속 시간에 따른 버프
        StackBased // 특정 행동 시 스택이 감소하는 버프
    }

    public abstract class DebuffData : ScriptableObject
    {
        public DebuffType debuffType;// 버프 타입
        public DebuffDurationType durationType;// 버프 지속 시간 타입
        public float duration; // 버프 지속 시간 (Timed 타입일 경우)

        // 버프 적용 메서드
        public abstract void ApplyDebuff(EnemyController enemy, Debuff debuff);
        // 버프 업데이트 메서드
        public abstract void UpdateDebuff(EnemyController enemy, Debuff debuff);
        // 버프 제거 메서드
        public abstract void RemoveDebuff(EnemyController enemy, Debuff debuff);
    }
}