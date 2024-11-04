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
        IDLE = 0, HIT, DESTROY, MOVE, APPEARED
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


        //HP 관련 설정값
        protected NetworkVariable<int> maxHp = new NetworkVariable<int>();
        protected NetworkVariable<int> nowHp = new NetworkVariable<int>();
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

        //--------상태 패턴------------
        [Rpc(SendTo.ClientsAndHost)]
        public void StateTransitionRpc(CombatStateType type)
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

        public void TakeDamage(float damage)
        {
            
        }
    }
}