using UnityEngine;

namespace AshGreen.Buff
{
    // BuffType 열거형 선언
    public enum BuffType
    {
        Adrenaline = 0,// 아드레날린
        UtteranceZ,// 발화 Z
        Concentration,// 집중
        OverflowingPower,// 넘치는 힘
        Shield,// 실드
    }

    public enum BuffDurationType
    {
        Timed, // 지속 시간에 따른 버프
        StackBased // 특정 행동 시 스택이 감소하는 버프
    }

    [CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
    public class BuffData : ScriptableObject
    {
        public BuffType buffType;// 버프 타입
        public BuffDurationType durationType;// 버프 지속 시간 타입
        public float duration; // 버프 지속 시간 (Timed 타입일 경우)
        public int maxStacks; // 최대 스택 수 (StackBased 타입일 경우)
        public bool isStackable; // 중첩 가능 여부
    }

}