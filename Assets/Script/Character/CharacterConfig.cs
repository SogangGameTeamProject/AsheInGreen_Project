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
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Scriptable Objects/Character/PlayerConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Data")]
        public string characterName;            // 캐릭터명
        public Sprite nameImg;
        public Sprite profileImg;
        public RuntimeAnimatorController propfileAni;
        public Sprite ingameImg;
        public Sprite iconSprite;          // 캐릭터 icon
        public GameObject playerPre; //플레이어 프리펩
        [Header("초기 스테이터스")]
        [Tooltip("초기 자금")]
        public int startMoney = 1000;
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

        [Header("아디")]
        //클라이언트 아이디
        public Dictionary<ulong, int> selectClientIds = new Dictionary<ulong, int>();

        public void EmptyData()
        {
            selectClientIds.Clear();
        }

        public ulong GetClientId(ulong clientId)
        {
            ulong id = 9999;

            ulong localId = clientId;
            if (selectClientIds.ContainsKey(localId))
                id = localId;

            return id;
        }

        public void SetClientId(ulong clientId, bool OUL = false)
        {
            if (OUL)
            {
                selectClientIds.Remove(clientId);
                foreach (KeyValuePair<ulong, int> kvp in selectClientIds)
                {
                    Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<ulong, int> kvp in selectClientIds)
                {
                    Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
                }
                selectClientIds.Add(clientId, -1);
            }
        }

        public int GetPlayerId(ulong clientId)
        {
            int id = -1;
            selectClientIds.TryGetValue(clientId, out id);
            return id;
        }
        public bool isPlayerId(int playerId)
        {
            bool value = false;
            value = selectClientIds.ContainsValue(playerId);
            return value;
        }
        public void SetPlayerId(ulong clientId, int playerId)
        {
            selectClientIds[clientId] = playerId;
        }
        public bool isSelectedForClientId(ulong clientId)
        {
            return selectClientIds.ContainsKey(clientId);
        }
        public bool isSelectedForPlayerId(int playerId)
        {
            return selectClientIds.ContainsValue(playerId);
        }
    }
}