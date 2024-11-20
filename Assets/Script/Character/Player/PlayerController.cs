using AshGreen.Character;
using AshGreen.Character.Skill;
using AshGreen.Item;
using System;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public class PlayerController : CharacterController
    {
        [HideInInspector]
        public GameplayManager gameplayManager;//게임플레이 메니저
        public PlayerUI playerUI;//플레이어 UI
        public ulong clientID;

        public ItemManager itemManager;//아이템 매니저
        public MovementController _movementController = null;
        public CharacterSkillManager _characterSkillManager = null;
        public Transform firePoint = null;

        public CharacterConfig characterConfig = null;//기본능력치가 저장되는 변수

        //돈관련
        public NetworkVariable<int> m_money = new NetworkVariable<int>(0);
        public int Money
        {
            get => m_money.Value;
            set
            {
                SetMoneyServerRpc(Mathf.Clamp(value, 0, 999999));
            }
        }
        
        //현재 돈 값을 수정하는 원격프로토콜 함수
        [ServerRpc]
        private void SetMoneyServerRpc(int value)
        {
            m_money.Value = value;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정

            level.OnValueChanged += LevelUpdateHUDRPC;
            addMaxHp.OnValueChanged += MaxHpUpdateHUDRPC;
            nowHp.OnValueChanged += NowHpUpdateHUDRPC;

            MaxHpUpdateHUDRPC(MaxHP, MaxHP);

            if (IsOwner)
            {
                OnSetStatusRpc();//스테이터스 값 초기화
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            level.OnValueChanged -= LevelUpdateHUDRPC;
            addMaxHp.OnValueChanged -= MaxHpUpdateHUDRPC;
            nowHp.OnValueChanged -= NowHpUpdateHUDRPC;
        }

        //캐릭터 스테이터스값 초기 설정
        [Rpc(SendTo.Server)]
        private void OnSetStatusRpc()
        {
            if (characterConfig)
            {
                Debug.Log("데이터 초기화");
                m_money.Value = characterConfig.startMoney;
                Debug.Log($"m_money: {m_money.Value}");
                LevelUpEx.Value = characterConfig.LevelUpEx;
                baseMaxHP.Value = characterConfig.MaxHP;
                nowHp.Value = baseMaxHP.Value;
                GrowthAttackPower.Value = characterConfig.GrowthAttackPower;
                GrowthPerAttackPower.Value = characterConfig.GrowthPerAttackPower;
                baseAttackPower.Value = characterConfig.AttackPower;
                baseMoveSpeed.Value = characterConfig.MoveSpeed;
                baseJumpPower.Value = characterConfig.JumpPower;
                baseJumMaxNum.Value = characterConfig.JumMaxNum;
            }
        }

        //UI 업데이트 함수들
        //레벨
        [Rpc(SendTo.ClientsAndHost)]
        private void LevelUpdateHUDRPC(int previousValue, int newValue)
        {
            if (playerUI == null) return;
            playerUI.UpdateLv(newValue);
            playerUI.UpdateHp(MaxHP, NowHP);
        }
        //최대 체력
        [Rpc(SendTo.ClientsAndHost)]
        private void MaxHpUpdateHUDRPC(int previousValue, int newValue)
        {
            if (playerUI == null) return;
            playerUI.UpdateHp(MaxHP, NowHP);
        }
        //현재 체력
        [Rpc(SendTo.ClientsAndHost)]
        private void NowHpUpdateHUDRPC(int previousValue, int newValue)
        {
            if (playerUI == null) return;
            playerUI.UpdateHp(MaxHP, NowHP);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SkillInfoUpdateHUDRPC(SkillType skillType, float coolTime, int minUseCoast, int nowEnergy)
        {
            playerUI.UpdateSkill(skillType, coolTime, minUseCoast, nowEnergy);
        }

        //애니메이션 동기화 함수
        public void PlayerSkillAni(string paraName)
        {
            _animator.SetLayerWeight(0, 0);
            _animator.SetLayerWeight(1, 1);
            _animator.SetTrigger(paraName);

            PlayerSkillAniRpc(paraName);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayerSkillAniRpc(string paraName)
        {
            if (IsOwner) return;

            _animator.SetLayerWeight(0, 0);
            _animator.SetLayerWeight(1, 1);
            _animator.SetTrigger(paraName);
        }

        public void EndSkillAni()
        {
            _animator.SetLayerWeight(0, 1);
            _animator.SetLayerWeight(1, 0);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void EndSkillAniRpc()
        {
            if (IsOwner) return;

            _animator.SetLayerWeight(0, 1);
            _animator.SetLayerWeight(1, 0);
        }
    }
}
