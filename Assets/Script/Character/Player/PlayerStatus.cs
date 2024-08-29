using UnityEngine;

namespace AshGreen.Player
{
    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus")]
    public class PlayerStatus : ScriptableObject
    {
        [Tooltip("최대체력")]
        public int MaxHP = 0;
        [Tooltip("공격력")]
        public float AttackPower = 0;
        [Tooltip("이동속도")]
        public float MoveSpeed = 0;
        [Tooltip("점프파워")]
        public float JumpPower = 0;
        [Tooltip("최대 점프 횟수")]
        public int JumMaxNum = 0;
        [Tooltip("스킬가속")]
        public float SkillAcceleration;
        [Tooltip("아이템 가속")]
        public float ItemAcceleration;
        [Tooltip("치명타 확률")]
        public float CriticalChance;
        [Tooltip("치명타 데미지")]
        public float CriticalDamage = 0;
    }
}