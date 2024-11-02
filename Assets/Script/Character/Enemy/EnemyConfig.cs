using UnityEngine;
using System;
using AshGreen.Character.Skill;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;
using static UnityEngine.Rendering.DebugUI;
using Newtonsoft.Json.Linq;

namespace AshGreen.Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Scriptable Objects/Character/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Data")]
        public string characterName;            // 캐릭터명
        public Sprite iconSprite;          // 캐릭터 icon
        [Header("초기 스테이터스")]
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
    }
}