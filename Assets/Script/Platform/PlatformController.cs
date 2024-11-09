using AshGreen.Character;
using AshGreen.State;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static AshGreen.Character.CharacterController;

namespace AshGreen.Platform
{
    public enum PlatformStateType
    {
        IDLE = 0, HIT, DESTROY, MOVE
    }

    public class PlatformController : NetworkBehaviour, IDamageable
    {
        //-----상태 패턴 관련 전역 변수------
        public PlatformStateType runningStateType;
        private StateContext<PlatformController> stateContext = null;
        [System.Serializable]
        public class StateData
        {
            public PlatformStateType type;
            public PlatformStateInit state;
        }
        public List<StateData> stateList//상태 관리를 위한 리스트
            = new List<StateData>();
        [SerializeField]
        private PlatformStateType startState = PlatformStateType.IDLE;


        //HP 관련 설정값
        [SerializeField]
        protected NetworkVariable<int> maxHp = new NetworkVariable<int>(2);
        [SerializeField]
        protected NetworkVariable<int> nowHp = new NetworkVariable<int>(2);
        protected int NowHp
        {
            get
            {
                return nowHp.Value;
            }

            private set
            {
                nowHp.Value = Mathf.Clamp(value, 0, maxHp.Value);
            }
        }

        [Rpc(SendTo.Server)]

        protected void SetNowHpRpc(int value)
        {
            NowHp = value;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            PlatformManager.Instance.platformList.Add(this);

            stateContext = new StateContext<PlatformController>(this);
            //상태 초기화
            if (IsServer)
                StateTransitionRpc(startState);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            PlatformManager.Instance.platformList.Remove(this);
        }

        private void Update()
        {
            stateContext.StateUpdate();
        }

        //--------상태 패턴------------
        [Rpc(SendTo.ClientsAndHost)]
        public void StateTransitionRpc(PlatformStateType type)
        {
            IState<PlatformController> state = null;
            StateData findState = stateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<IState<PlatformController>>();
                runningStateType = findState.type;
                stateContext.TransitionTo(state);
            }
        }

        //------------피격 처리-----------
        //일단 안씀
        public void DealDamage(Character.CharacterController target, float damageCoefficient, AttackType attackType)
        {
            
        }

        //플렛폼 피격 쳐리
        public void TakeDamage(float damage)
        {
            if (NowHp - damage > 0)
                StateTransitionRpc(PlatformStateType.HIT);
            else
                StateTransitionRpc(PlatformStateType.DESTROY);
            SetNowHpRpc(NowHp - (int)damage);
        }
    }
}