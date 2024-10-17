using UnityEngine;
using AshGreen.Character.Skill;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
namespace AshGreen.Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Scriptable Objects/Character/CharacterConfig")]
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

        [Header("Data")]
        public Sprite characterSprite;          // CharacterSprite for the character selection scene
        public Sprite characterShipSprite;      // The character ship sprite for the character selection scene
        public Sprite iconSprite;               // Sprite use on the player UI on gameplay scene
        public Sprite iconDeathSprite;          // Sprite use on the player UI on gameplay scene for his death
        public string characterName;            // Character name
        public GameObject spaceshipPrefab;      // Prefab of the spaceship to use on gameplay scene
        public GameObject spaceshipScorePrefab; // Sprite for the ship on the endgame scene UI    
        public Color Color;
        public Color darkColor;

        [Header("스킬 설정")]
        public List<CharacterSkill> skills;
        [Header("투사체 설정")]
        public List<GameObject> projectileObjects = new List<GameObject>();
        [Header("ClientId")]
        public ulong clientId = 0;
        public int playerId = 0;
        public bool isSelected;
    }
}