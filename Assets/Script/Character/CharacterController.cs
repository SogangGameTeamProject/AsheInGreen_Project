using UnityEngine;
using AshGreen.Obsever;
using System;
using System.Collections.Generic;
using AshGreen.Character;
using Unity.Netcode;
using AshGreen.State;
using Unity.Collections.LowLevel.Unsafe;

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
        //외부 컨트롤러들
        public DamageReceiver _damageReceiver = null;
        public StatusEffectManager _statusEffectManager = null;
        public Animator _animator = null;

        private Rigidbody2D rBody = null;

        //------상태 패턴 관련 전역 변수 선언------
        
        //---------전투 상태--------
        public CombatStateType runningCombatStateType;
        private StateContext<CharacterController> combatStateContext = null;
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
                OnFlip(value);
                RequsetOnFlipRpc(value);
                characterDirection = value;
            }
        }

        //서버에 방향 전환 요청 메서드
        [Rpc(SendTo.ClientsAndHost)]
        private void RequsetOnFlipRpc(CharacterDirection newValue)
        {
            if (IsOwner)
                return;
            OnFlip(newValue);
        }
        //방향 전환 매서드
        private void OnFlip(CharacterDirection newValue)
        {
            if (runningCombatStateType == CombatStateType.Death)
                return;

            Vector3 flipScale = transform.localScale;
            if (newValue == CharacterDirection.Left)
                flipScale.x = Mathf.Abs(flipScale.x) * -1;
            else
                flipScale.x = Mathf.Abs(flipScale.x);
            transform.localScale = flipScale;
        }

        //------스테이터스 관련 전역 변수 선언------
        //레벨 관련
        protected NetworkVariable<int> LevelUpEx = new NetworkVariable<int>(0);
        protected NetworkVariable<int> level = new NetworkVariable<int>(1);
        protected NetworkVariable<int> experience = new NetworkVariable<int>(0);

        public int Experience
        {
            get
            {
                return experience.Value;
            }
            private set
            {
                int ex = experience.Value + value;
                if(ex >= LevelUpEx.Value)
                {
                    ex -= LevelUpEx.Value;
                    level.Value++;
                }
                experience.Value = ex;
            }
        }

        //최대체력 관련 전역변수
        protected NetworkVariable<int> baseMaxHP = new NetworkVariable<int>(0);
        protected NetworkVariable<int> addMaxHp = new NetworkVariable<int>(0);
        protected NetworkVariable<int> GrowthMaxHP = new NetworkVariable<int>(0);
        protected NetworkVariable<float> GrowthPerMaxHP = new NetworkVariable<float>(0);
        public int MaxHP
        {
            get
            {
                int maxHp =
                    (int)(baseMaxHP.Value + addMaxHp.Value + (level.Value * GrowthMaxHP.Value));
                maxHp += (int)(maxHp * (level.Value * GrowthPerMaxHP.Value));
                return maxHp > 0 ? maxHp : 1;
            }
        }
        //현재 체력 관련 전역변수
        public NetworkVariable<int> nowHp = new NetworkVariable<int>(0);

        public int NowHP
        {
            get
            {
                return nowHp.Value;
            }
            private set
            {
                nowHp.Value = Mathf.Clamp(value, 0, MaxHP);
            }
        }

        /// <summary>
        /// 플레이어의 체력을 조정하는 메서드
        /// </summary>
        /// <param name="addNowHp">증감 체력값</param>
        /// <param name="addMaxHp">증감할 최대체력 값</param>
        [Rpc(SendTo.Server)]
        public void SetHpRpc(int addNowHp, int addMaxHp = 0)
        {
            this.addMaxHp.Value += addMaxHp;
            NowHP += addNowHp;
        }

        //공격력 관련 전역변수
        protected NetworkVariable<int> baseAttackPower = new NetworkVariable<int>(0);
        protected NetworkVariable<int> GrowthAttackPower = new NetworkVariable<int>(0);
        protected NetworkVariable<float> GrowthPerAttackPower = new NetworkVariable<float>(0);
        protected NetworkVariable<int> addAttackPower = new NetworkVariable<int>(0);
        protected NetworkVariable<float> addAttackPerPower = new NetworkVariable<float>(1);

        public int AttackPower
        {
            get
            {   
                float attackPower = baseAttackPower.Value + (level.Value * GrowthAttackPower.Value) + addAttackPower.Value;
                if(GrowthPerAttackPower.Value > 0)
                    attackPower += attackPower * (level.Value * GrowthPerAttackPower.Value);
                if(addAttackPerPower.Value > 0)
                    attackPower *= addAttackPerPower.Value;

                return attackPower > 0 ? (int)attackPower : 1;
            }
        }

        /// <summary>
        /// 플레이어 공격력을 조정하는 메서드
        /// </summary>
        /// <param name="addAttackPower">증감할 공격력(고정)</param>
        /// <param name="addAttackPowerPer">증감할 공격력(%)</param>
        [Rpc(SendTo.Server)]
        public void SetAttackpowerRpc(int addAttackPower, float addAttackPerPower = 0)
        {
            this.addAttackPower.Value += addAttackPower;
            this.addAttackPerPower.Value += addAttackPerPower;
        }

        //이동속도 관련 변수
        protected NetworkVariable<float> baseMoveSpeed = new NetworkVariable<float>(0);
        protected NetworkVariable<float> addMoveSpeed = new NetworkVariable<float>(0);
        protected NetworkVariable<float> addMovePerSpeed = new NetworkVariable<float>(1);
        public float MoveSpeed
        {
            get
            {
                return (baseMoveSpeed.Value + addMoveSpeed.Value) * addMovePerSpeed.Value;
            }
        }

        /// <summary>
        /// 이동속도 능력치 조정 메서드
        /// </summary>
        /// <param name="addMoveSpeed">증감할 이동속도(고정)</param>
        /// <param name="addMovePerSpeed">증감할 이동속도(%)</param>
        [Rpc(SendTo.Server)]
        public void SetMovespeedRpc(float addMoveSpeed, float addMovePerSpeed = 0)
        {
            this.addMoveSpeed.Value += addMoveSpeed;
            this.addMovePerSpeed.Value += addMovePerSpeed;
        }

        //점프파워 관련 변수
        protected NetworkVariable<float> baseJumpPower = new NetworkVariable<float>(0);
        protected NetworkVariable<float> addJumpPower = new NetworkVariable<float>(0);
        public float JumpPower
        {
            get { 
                return baseJumpPower.Value + addJumpPower.Value; 
            }
        }
        //점프 횟수 관련 변수
        protected NetworkVariable<int> baseJumMaxNum = new NetworkVariable<int>(0);
        protected NetworkVariable<int> addJumMaxNum =  new NetworkVariable<int>(0);
        public int JumMaxNum
        {
            get
            {
                return baseJumMaxNum.Value + addJumMaxNum.Value;
            }
        }
        /// <summary>
        /// 점프관련 스탯 조정 메서드
        /// </summary>
        /// <param name="addJumMaxNum"></param>
        /// <param name="addJumpPower"></param>
        [Rpc(SendTo.Server)]
        public void SetJumpRpc(int addJumMaxNum, float addJumpPower = 0)
        {
            this.addJumMaxNum.Value += addJumMaxNum;
            this.addJumpPower.Value += addJumpPower;
        }

        public int jumCnt { get; set; }
        //스킬가속 관련 변수
        private NetworkVariable<float> addSkillAcceleration =  new NetworkVariable<float>(50);
        public float SkillAcceleration {
            get
            {
                return addSkillAcceleration.Value;
            }
        }
        //아이템가속 관련 변수
        private NetworkVariable<float> addItemAcceleration =  new NetworkVariable<float>(0);
        public float ItemAcceleration
        {
            get
            {
                return addItemAcceleration.Value;
            }
        }
        /// <summary>
        /// [스킬, 아이템] 가속 스탯 증감 조정 메서드
        /// </summary>
        /// <param name="addSkillAcceleration">증감할 스킬 가속</param>
        /// <param name="addItemAcceleration">증감할 아이템 가속</param>
        [Rpc(SendTo.Server)]
        public void SetAccelerationRpc(float addSkillAcceleration, float addItemAcceleration = 0)
        {
            this.addSkillAcceleration.Value += addSkillAcceleration;
            this.addItemAcceleration.Value += addItemAcceleration;
        }
        //치명타 확률
        private NetworkVariable<float> baseCriticalChance = new NetworkVariable<float>(0);
        private NetworkVariable<float> addCriticalChance =  new NetworkVariable<float>(0);
        public float CriticalChance
        {
            get
            {
                float chance = baseCriticalChance.Value + addCriticalChance.Value;
                chance = Mathf.Clamp(chance, 0, 1);
                return chance;
            }
        }
        //치명타 데미지
        private NetworkVariable<float> baseCriticalDamage = new NetworkVariable<float>(1.2f);
        private NetworkVariable<float> addCriticalDamage  = new NetworkVariable<float>(0);
        public float CriticalDamage
        {
            get
            {
                float damage = baseCriticalDamage.Value + addCriticalDamage.Value;
                damage = Mathf.Clamp(damage, 1, 99);
                return damage;
            }
        }
        /// <summary>
        /// 치명타 관련 스탯 증감 조정 메서드
        /// </summary>
        /// <param name="addCriticalChance">증감 치명타 확률</param>
        /// <param name="addCriticalDamage">증감 치명타 데미지</param>
        [Rpc(SendTo.Server)]
        public void SetCriticalRpc(float addCriticalChance, float addCriticalDamage = 0)
        {
            this.addCriticalChance.Value += addCriticalChance;
            this.addCriticalDamage.Value += addCriticalDamage;
        }

        //받는 피해 증가 관련 스탯
        public NetworkVariable<float> takenDamageCoefficient = new NetworkVariable<float>(1);
        public float TakenDamageCoefficient
        {
            get
            {
                return takenDamageCoefficient.Value;
            }
        }
        [Rpc(SendTo.Server)]
        public void SetTakenDamageCoefficientRpc(float value)
        {
            takenDamageCoefficient.Value = Mathf.Clamp(takenDamageCoefficient.Value + value, 0, 10);
        }

        //가하는 데미지 증가 
        private NetworkVariable<float> dealDamageCoefficient = new NetworkVariable<float>(1);
        public float DealDamageCoefficient
        {
            get
            {
                return dealDamageCoefficient.Value;
            }
        }
        [Rpc(SendTo.Server)]
        public void SetDealDamageCoefficientRpc(float value)
        {
            dealDamageCoefficient.Value = Mathf.Clamp(dealDamageCoefficient.Value + value, 0, 10);
        }

        //피해 면역 관련 
        public NetworkVariable<bool> isDamageImmunity = new NetworkVariable<bool>(false);// 피해 면역
        public LayerMask projectilesLayer;

        public void SetDamageimmunity(bool value)
        {
            ConflictSettings(rBody, projectilesLayer, value);
            SetDamageimmunityRpc(value);
        }

        [Rpc(SendTo.Server)]
        private void SetDamageimmunityRpc(bool damageImmunity)
        {
            this.isDamageImmunity.Value = damageImmunity;
        }

        //피해면역 처리
        [Rpc(SendTo.ClientsAndHost)]
        public void DamageImmunityRpc(bool preValue, bool newValue)
        {
            if (preValue == newValue) return;
            if (IsOwner) return;

            ConflictSettings(rBody, projectilesLayer, newValue);
        }

        

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            rBody = GetComponent<Rigidbody2D>();

            combatStateContext = new StateContext<CharacterController>(this);//콘텍스트 생성

            if(IsOwner)
                CombatStateTransitionRpc(CombatStateType.Idle);

            //피해 면역 처리
            isDamageImmunity.OnValueChanged += DamageImmunityRpc;

            //피격 타격 액션 설정
            _damageReceiver.TakeDamageAction += TakeDamage;
            _damageReceiver.DealDamageAction += DealDamage;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            //델리게이터 제거
            isDamageImmunity.OnValueChanged -= DamageImmunityRpc;
            _damageReceiver.TakeDamageAction -= TakeDamage;
            _damageReceiver.DealDamageAction -= DealDamage;

        }

        protected virtual void FixedUpdate()
        {
            if (IsSpawned)
                combatStateContext.StateUpdate();
        }

        

        /// <summary>
        /// 피격 타격 처리 메서드
        /// </summary>
        /// 
        public void TakeDamage(float damage)
        {
            if (runningCombatStateType != CombatStateType.Death)
            {

                if (nowHp.Value-damage > 0)
                    CombatStateTransitionRpc(CombatStateType.Hit);
                else
                    CombatStateTransitionRpc(CombatStateType.Death);

                SetHpRpc(-(int)damage);
            }
        }

        /// <summary>
        /// 피격 타격 처리 메서드
        /// </summary>
        /// 
        public void DealDamage(CharacterController target, float damage, AttackType attackType, bool isCritical = false)
        {
            target._damageReceiver.TakeDamage(damage);
        }

        /// <summary>
        /// 특정 레이어와의 충돌을 끄고 키는 함수
        /// </summary>
        /// <param name="rBody">충돌 설정할 리지드 바디</param>
        /// <param name="layerMask">설정할 레이어</param>
        /// <param name="onOffVlaue">끌지 킬지 여부</param>
        public void ConflictSettings(Rigidbody2D rBody, LayerMask layerMask, bool onOffVlaue)
        {
            if (onOffVlaue)
            {
                rBody.excludeLayers |= layerMask;
            }
            else
            {
                rBody.excludeLayers &= ~layerMask;
            }
        }

        //----------상태패턴 관련 함수들---------

        //-----전투 상태 과련 함수----
        //전투 상태 변환 함수
        [Rpc(SendTo.ClientsAndHost)]
        public void CombatStateTransitionRpc(CombatStateType type)
        {
            IState<CharacterController> state = null;
            CombatStateData findState = combatStateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<IState<CharacterController>>();
                runningCombatStateType = findState.type;
                combatStateContext.TransitionTo(state);
            }
        }

        
    }
}
