using UnityEngine;
using AshGreen.Character.Skill;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
namespace AshGreen.Character
{
    [CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character/CharacterStatus")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("애니메이션 설정")]
        public RuntimeAnimatorController animator;
        [Header("초기 스테이터스")]
        [Tooltip("레벨업에 필요한 경험치")]
        public int LevelUpEx = 200;
        [Tooltip("최대체력")]
        public int MaxHP = 0;
        [Tooltip("성장 체력")]
        public int GrowthMaxHP = 1;
        [Tooltip("성장 체력(%)")]
        public float GrowthPerMaxHP = 10;
        [Tooltip("공격력")]
        public int AttackPower = 10;
        [Tooltip("성장 공격력")]
        public int GrowthAttackPower = 8;
        [Tooltip("성장 공격력(%)")]
        public float GrowthPerAttackPower = 0;
        [Tooltip("이동속도")]
        public float MoveSpeed = 0;
        [Tooltip("점프파워")]
        public float JumpPower = 0;
        [Tooltip("최대 점프 횟수")]
        public int JumMaxNum = 0;

        [Header("스킬 설정")]
        public List<CharacterSkill> skills;
    }
}