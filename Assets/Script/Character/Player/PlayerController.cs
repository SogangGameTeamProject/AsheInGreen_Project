using UnityEngine;
using AshGreen.Obsever;
using System;
using System.Collections.Generic;
namespace AshGreen.Player
{
    public class PlayerController : Subject
    {
        //------상태 패턴 관련 전역 변수 선언------
        public PlayerStateType runningStateType;
        private PlayerStateContext stateContext = null;
        //상태 정보 관리를 위한 클래스
        [System.Serializable]
        public class StateData
        {
            public PlayerStateType type;//트리거할 타입
            public PlayerStateBase state;//실행할 상태
        }
        public List<StateData> stateList//플레이어 상태 관리를 위한 리스트
            = new List<StateData>();

        public PlayerStateType startStateType = PlayerStateType.OnGround;

        //------스테이터스 관련 전역 변수 선언------
        [SerializeField]
        private PlayerStatus baseStatus = null;//기본능력치가 저장되는 변수
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
        private float baseJumMaxNum = 0;
        private float addJumMaxNum = 0;
        public float JumMaxNum
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
            stateContext = new PlayerStateContext(this);//콘텍스트 생성
            OnSetStatus();//스테이터스 값 초기화
            StateInit(startStateType);
        }

        //캐릭터 스테이터스값 초기 설정
        private void OnSetStatus()
        {
            if (baseStatus)
            {
                baseMaxHP = baseStatus.MaxHP;
                baseAttackPower = baseStatus.AttackPower;
                baseMoveSpeed = baseStatus.MoveSpeed;
                baseJumpPower = baseStatus.JumpPower;
                baseJumMaxNum = baseStatus.JumMaxNum;
                baseSkillAcceleration = baseStatus.SkillAcceleration;
                baseItemAcceleration = baseStatus.ItemAcceleration;
                baseCriticalChance = baseStatus.CriticalChance;
                baseCriticalDamage = baseStatus.CriticalDamage;
            }
        }

        //상태패턴 관련 함수들
        //상태 초기화 함수
        public void StateInit(PlayerStateType type)
        {
            PlayerState state = null;
            StateData findState = stateList.Find(state => state.type == type);
            if (findState != null)
            {
                state = findState.state.GetComponent<PlayerState>();
                runningStateType = findState.type;
                stateContext.Initialize(state);
            }
        }
        //상태 변환 함수
        public void StateTransition(PlayerStateType type)
        {
            PlayerState state = null;
            StateData findState = stateList.Find(state => state.type == type);
            if (findState != null)
            {
                state = findState.state.GetComponent<PlayerState>();
                runningStateType = findState.type;
                stateContext.TransitionTo(state);
            }
        }
    }
}
