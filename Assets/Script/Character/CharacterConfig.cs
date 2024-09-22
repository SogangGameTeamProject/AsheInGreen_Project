using UnityEngine;

namespace AshGreen.Character
{
    [CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character/CharacterStatus")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("초기 스테이터스")]
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

        //[Header("액션 설정")]

    }
}