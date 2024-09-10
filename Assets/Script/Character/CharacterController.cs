using UnityEngine;
using AshGreen.Obsever;
using System;
using System.Collections.Generic;
using AshGreen.Character;
using Unity.Netcode;
namespace AshGreen.Character
{
    //캐릭터 방향 타입
    public enum CharacterDirection
    {
        Left = -1, Right = 1
    }

    //전투 상태 타입
    public enum CombatStateType
    {
        Null = -1, Idle = 0, Hit = 1, Death = 2, 
    }

    public class CharacterController : Subject
    {
        //
        public MovementController _movementController = null;

        //------상태 패턴 관련 전역 변수 선언------
        
        //---------전투 상태--------
        public CombatStateType runningCombatStateType;
        private CharacterStateContext combatStateContext = null;
        //상태 정보 관리를 위한 클래스
        [System.Serializable]
        public class CombatStateData
        {
            public CombatStateType type;//트리거할 타입
            public CharacterStateBase state;//실행할 상태
        }
        public List<CombatStateData> combatStateList//상태 관리를 위한 리스트
            = new List<CombatStateData>();

        //캐릭터 방향 관련
        private CharacterDirection characterDirection = CharacterDirection.Right;
        public CharacterDirection CharacterDirection
        {
            get
            {
                return characterDirection;
            }

            set
            {
                if(characterDirection != value)
                {
                    characterDirection = value;
                    OnFlipServerRpc();
                }
                
            }
        }

        //------스테이터스 관련 전역 변수 선언------
        [SerializeField]
        private CharacterConfig baseConfig = null;//기본능력치가 저장되는 변수
        //최대체력 관련 전역변수
        private NetworkVariable<int> baseMaxHP = new NetworkVariable<int>(0);
        private NetworkVariable<int> addMaxHp = new NetworkVariable<int>(0);
        public int MaxHP
        {
            get
            {
                int maxHp = baseMaxHP.Value + addMaxHp.Value;
                return maxHp > 0 ? maxHp : 1;
            }
        }

        //현재 체력 관련 전역변수
        public NetworkVariable<int> nowHp {  get; private set; }
        [ServerRpc(RequireOwnership = false)]
        public void SetNowHPServerRpc(int value)
        {
            nowHp.Value += value;
        }

        //공격력 관련 전역변수
        private NetworkVariable<float> baseAttackPower = new NetworkVariable<float>(0);
        private NetworkVariable<float> addAttackPower = new NetworkVariable<float>(0);
        private NetworkVariable<float> attackPerPower = new NetworkVariable<float>(0);
        public float AttackPower
        {
            get
            {
                float attackPower = (baseAttackPower.Value + addAttackPower.Value) * attackPerPower.Value;
                return attackPower > 0 ? attackPower : 1;
            }
        }
        //이동속도 관련 변수
        private NetworkVariable<float> baseMoveSpeed = new NetworkVariable<float>(0);
        private NetworkVariable<float> addMoveSpeed = new NetworkVariable<float>(0);
        private NetworkVariable<float> addMovePerSpeed = new NetworkVariable<float>(0);
        public float MoveSpeed
        {
            get
            {
                return (baseMoveSpeed.Value + addMoveSpeed.Value) * addMovePerSpeed.Value;
            }
        }
        //점프파워 관련 변수
        private NetworkVariable<float> baseJumpPower = new NetworkVariable<float>(0);
        private NetworkVariable<float> addJumpPower = new NetworkVariable<float>(0);
        public float JumpPower
        {
            get { 
                return baseJumpPower.Value + addJumpPower.Value; 
            }
        }
        //점프 횟수 관련 변수
        private NetworkVariable<int> baseJumMaxNum = new NetworkVariable<int>(0);
        private NetworkVariable<int> addJumMaxNum =  new NetworkVariable<int>(0);
        public int JumMaxNum
        {
            get
            {
                return baseJumMaxNum.Value + addJumMaxNum.Value;
            }
        }
        public int jumCnt { get; set; }
        //스킬가속 관련 변수
        private NetworkVariable<float> baseSkillAcceleration = new NetworkVariable<float>(0);
        private NetworkVariable<float> addSkillAcceleration =  new NetworkVariable<float>(0);
        public float SkillAcceleration {
            get
            {
                return baseSkillAcceleration.Value + addSkillAcceleration.Value;
            }
        }
        //아이템가속 관련 변수
        private NetworkVariable<float> baseItemAcceleration = new NetworkVariable<float>(0);
        private NetworkVariable<float> addItemAcceleration =  new NetworkVariable<float>(0);
        public float ItemAcceleration
        {
            get
            {
                return baseItemAcceleration.Value + addItemAcceleration.Value;
            }
        }
        //치명타 확률
        private NetworkVariable<float> baseCriticalChance = new NetworkVariable<float>(0);
        private NetworkVariable<float> addCriticalChance =  new NetworkVariable<float>(0);
        public float CriticalChance
        {
            get
            {
                return baseCriticalChance.Value + addCriticalChance.Value;
            }
        }
        //치명타 데미지
        private NetworkVariable<float> baseCriticalDamage = new NetworkVariable<float>(0);
        private NetworkVariable<float> addCriticalDamage  = new NetworkVariable<float>(0);
        public float CriticalDamage
        {
            get
            {
                return baseCriticalDamage.Value + addCriticalDamage.Value;
            }
        }

        private void Start()
        {
            combatStateContext = new CharacterStateContext(this);//콘텍스트 생성
            OnSetStatus();//스테이터스 값 초기화
            CombatStateInit(CombatStateType.Idle);
        }

        private void FixedUpdate()
        {
            combatStateContext.StateUpdate();
        }

        //캐릭터 스테이터스값 초기 설정
        private void OnSetStatus()
        {
            if (baseConfig)
            {
                baseMaxHP.Value = baseConfig.MaxHP;
                nowHp.Value = baseMaxHP.Value;
                baseAttackPower.Value = baseConfig.AttackPower;
                baseMoveSpeed.Value = baseConfig.MoveSpeed;
                baseJumpPower.Value = baseConfig.JumpPower;
                baseJumMaxNum.Value = baseConfig.JumMaxNum;
                baseSkillAcceleration.Value = baseConfig.SkillAcceleration;
                baseItemAcceleration.Value = baseConfig.ItemAcceleration;
                baseCriticalChance.Value = baseConfig.CriticalChance;
                baseCriticalDamage.Value = baseConfig.CriticalDamage;
            }
        }

        //Flip구현 함수
        [ServerRpc]
        private void OnFlipServerRpc()
        {
            OnFlipClientRpc();
        }
        [ClientRpc]
        private void OnFlipClientRpc()
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
        }

        //----------상태패턴 관련 함수들---------

        //-----전투 상태 과련 함수----
        //전투 상태 초기화 함수
        public void CombatStateInit(CombatStateType type)
        {
            CharacterState state = null;
            CombatStateData findState = combatStateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<CharacterState>();
                runningCombatStateType = findState.type;
                combatStateContext.Initialize(state);
            }
        }
        //전투 상태 변환 함수
        public void CombatStateTransition(CombatStateType type)
        {
            CharacterState state = null;
            CombatStateData findState = combatStateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<CharacterState>();
                runningCombatStateType = findState.type;
                combatStateContext.TransitionTo(state);
            }
        }
    }
}
