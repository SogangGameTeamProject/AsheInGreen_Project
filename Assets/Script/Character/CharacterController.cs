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

    [System.Serializable]
    public class CharacterController : Subject
    {
        //외부 컨트롤러들
        public MovementController _movementController = null;
        public DamageReceiver _damageReceiver = null;
        public StatusEffectManager _statusEffectManager = null;

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
                characterDirection = value;
                float localScaleX = transform.localScale.x;
                if((localScaleX < 0 && value == CharacterDirection.Right) || (localScaleX > 0 && value == CharacterDirection.Left))
                    OnFlipServerRpc();
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
                Debug.Log("현재체력: " + nowHp.Value);
            }
        }

        /// <summary>
        /// 플레이어의 체력을 조정하는 메서드
        /// </summary>
        /// <param name="addNowHp">증감 체력값</param>
        /// <param name="addMaxHp">증감할 최대체력 값</param>
        [ServerRpc(RequireOwnership = false)]
        public void SetHpServerRpc(int addNowHp, int addMaxHp = 0)
        {
            this.addMaxHp.Value += addMaxHp;
            NowHP += addNowHp;
        }

        //공격력 관련 전역변수
        private NetworkVariable<float> baseAttackPower = new NetworkVariable<float>(0);
        private NetworkVariable<float> addAttackPower = new NetworkVariable<float>(0);
        private NetworkVariable<float> addAttackPerPower = new NetworkVariable<float>(1);
        public float AttackPower
        {
            get
            {
                float attackPower = (baseAttackPower.Value + addAttackPower.Value) * addAttackPerPower.Value;
                return attackPower > 0 ? attackPower : 1;
            }
        }
        /// <summary>
        /// 플레이어 공격력을 조정하는 메서드
        /// </summary>
        /// <param name="addAttackPower">증감할 공격력(고정)</param>
        /// <param name="addAttackPowerPer">증감할 공격력(%)</param>
        [ServerRpc(RequireOwnership = false)]
        public void SetAttackpowerServerRpc(float addAttackPower, float addAttackPerPower = 0)
        {
            this.addAttackPower.Value += addAttackPower;
            this.addAttackPerPower.Value += addAttackPerPower;
        }

        //이동속도 관련 변수
        private NetworkVariable<float> baseMoveSpeed = new NetworkVariable<float>(0);
        private NetworkVariable<float> addMoveSpeed = new NetworkVariable<float>(0);
        private NetworkVariable<float> addMovePerSpeed = new NetworkVariable<float>(1);
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
        [ServerRpc(RequireOwnership = false)]
        public void SetMovespeedServerRpc(float addMoveSpeed, float addMovePerSpeed = 0)
        {
            this.addMoveSpeed.Value += addMoveSpeed;
            this.addMovePerSpeed.Value += addMovePerSpeed;
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
        /// <summary>
        /// 점프관련 스탯 조정 메서드
        /// </summary>
        /// <param name="addJumMaxNum"></param>
        /// <param name="addJumpPower"></param>
        [ServerRpc(RequireOwnership = false)]
        public void SetJumpServerRpc(int addJumMaxNum, float addJumpPower = 0)
        {
            this.addJumMaxNum.Value += addJumMaxNum;
            this.addJumpPower.Value += addJumpPower;
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
        /// <summary>
        /// [스킬, 아이템] 가속 스탯 증감 조정 메서드
        /// </summary>
        /// <param name="addSkillAcceleration">증감할 스킬 가속</param>
        /// <param name="addItemAcceleration">증감할 아이템 가속</param>
        [ServerRpc(RequireOwnership = false)]
        public void SetAccelerationServerRpc(float addSkillAcceleration, float addItemAcceleration = 0)
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
        /// <summary>
        /// 치명타 관련 스탯 증감 조정 메서드
        /// </summary>
        /// <param name="addCriticalChance">증감 치명타 확률</param>
        /// <param name="addCriticalDamage">증감 치명타 데미지</param>
        [ServerRpc(RequireOwnership = false)]
        public void SetCriticalServerRpc(float addCriticalChance, float addCriticalDamage = 0)
        {
            this.addCriticalChance.Value += addCriticalChance;
            this.addCriticalDamage.Value += addCriticalDamage;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            combatStateContext = new CharacterStateContext(this);//콘텍스트 생성
            OnSetStatusServerRpc();//스테이터스 값 초기화
            CombatStateInitServerRpc(CombatStateType.Idle);

            //피격 타격 액션 설정
            _damageReceiver.TakeDamageAction += TakeDamage;
            _damageReceiver.DealDamageAction += DealDamage;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            //피격 타격 액션 제거
            _damageReceiver.TakeDamageAction -= TakeDamage;
            _damageReceiver.DealDamageAction -= DealDamage;
        }

        private void FixedUpdate()
        {
            combatStateContext.StateUpdate();
        }

        //캐릭터 스테이터스값 초기 설정
        [ServerRpc(RequireOwnership = false)]
        private void OnSetStatusServerRpc()
        {
            if (baseConfig)
            {
                baseMaxHP.Value = baseConfig.MaxHP;
                Debug.Log(baseMaxHP.Value);
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
        [ServerRpc(RequireOwnership = false)]
        private void OnFlipServerRpc()
        {
            Debug.Log("방향전환 요청");
            OnFlipClientRpc();
        }
        [ClientRpc]
        private void OnFlipClientRpc()
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
        }

        /// <summary>
        /// 피격 타격 처리 메서드
        /// </summary>
        /// 
        public void TakeDamage(float damage)
        {
            Debug.Log("플레이어 피격 처리");
            SetHpServerRpc(-(int)damage);
            CombatStateTransitionServerRpc(CombatStateType.Hit);
        }

        public void DealDamage(CharacterController target, float damage, AttackType attackType, bool isCritical = false)
        {
            if(target.runningCombatStateType != CombatStateType.Death)
            {
                target._damageReceiver.TakeDamage(damage);
            }
        }

        //----------상태패턴 관련 함수들---------

        //-----전투 상태 과련 함수----
        //전투 상태 초기화 함수
        [ServerRpc(RequireOwnership = false)]
        public void CombatStateInitServerRpc(CombatStateType type)
        {
            CombatStateInitClientRpc(type);
        }

        [ClientRpc]
        public void CombatStateInitClientRpc(CombatStateType type)
        {
            CombatStateInit(type);
        }

        private void CombatStateInit(CombatStateType type)
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
        [ServerRpc(RequireOwnership = false)]
        public void CombatStateTransitionServerRpc(CombatStateType type)
        {
            CombatStateTransitionClientRpc(type);
        }

        [ClientRpc]
        public void CombatStateTransitionClientRpc(CombatStateType type)
        {
            CombatStateInit(type);
        }
        private void CombatStateTransition(CombatStateType type)
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
