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
        private int baseMaxHP = 0;
        private int addMaxHp = 0;
        public int MaxHP
        {
            get
            {
                int maxHp = baseMaxHP + addMaxHp;
                return maxHp > 0 ? maxHp : 1;
            }
            set
            {
                addMaxHp += value;
            }
        }
        //공격력 관련 전역변수
        private float baseAttackPower = 0;
        private float addAttackPower = 0;
        public float AttackPower
        {
            get
            {
                float attackPower = baseAttackPower + addAttackPower;
                return attackPower > 0 ? attackPower : 1;
            }
            set
            {
                addAttackPower += value;
            }
        }
        //이동속도 관련 변수
        private float baseMoveSpeed = 0;
        private float addMoveSpeed = 0;
        public float MoveSpeed
        {
            get
            {
                return baseMoveSpeed + addMoveSpeed;
            }
            set { 
                addMoveSpeed += value;
            }
        }
        //점프파워 관련 변수
        private float baseJumpPower = 0;
        private float addJumpPower = 0;
        public float JumpPower
        {
            get { 
                return baseJumpPower + addJumpPower; 
            }
            set { 
                addJumpPower += value;
            }
        }
        //점프 횟수 관련 변수
        private int baseJumMaxNum = 0;
        private int addJumMaxNum = 0;
        public int JumMaxNum
        {
            get
            {
                return baseJumMaxNum + addJumMaxNum;
            }
            set
            {
                addJumMaxNum += value;
            }
        }
        public int jumCnt { get; set; }
        //스킬가속 관련 변수
        private float baseSkillAcceleration = 0;
        private float addSkillAcceleration = 0;
        public float SkillAcceleration {
            get
            {
                return baseSkillAcceleration + addSkillAcceleration;
            }
            set
            {
                addSkillAcceleration += value;
            }
        }
        //아이템가속 관련 변수
        private float baseItemAcceleration = 0;
        private float addItemAcceleration = 0;
        public float ItemAcceleration
        {
            get
            {
                return baseItemAcceleration + addItemAcceleration;
            }
            set
            {
                addItemAcceleration += value;
            }
        }
        //치명타 확률
        private float baseCriticalChance = 0;
        private float addCriticalChance = 0;
        public float CriticalChance
        {
            get
            {
                return baseCriticalChance + addCriticalChance;
            }
            set
            {
                addCriticalChance += value;
            }
        }
        //치명타 데미지
        private float baseCriticalDamage = 0;
        private float addCriticalDamage = 0;
        public float CriticalDamage
        {
            get
            {
                return baseCriticalDamage + addCriticalDamage;
            }
            set
            {
                addCriticalDamage += value;
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
                baseMaxHP = baseConfig.MaxHP;
                baseAttackPower = baseConfig.AttackPower;
                baseMoveSpeed = baseConfig.MoveSpeed;
                baseJumpPower = baseConfig.JumpPower;
                baseJumMaxNum = baseConfig.JumMaxNum;
                baseSkillAcceleration = baseConfig.SkillAcceleration;
                baseItemAcceleration = baseConfig.ItemAcceleration;
                baseCriticalChance = baseConfig.CriticalChance;
                baseCriticalDamage = baseConfig.CriticalDamage;
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
