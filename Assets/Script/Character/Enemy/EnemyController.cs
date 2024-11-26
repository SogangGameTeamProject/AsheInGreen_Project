using AshGreen.Debuff;
using AshGreen.State;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public class EnemyController : CharacterController
    {
        [SerializeField]
        private EnemyConfig enemyConfig;

        public DebuffManager debuffManager = null;

        private StateContext<EnemyController> patteurnStateContext = null;

        public List<EnemyPatteurnStateInit> patteurnStateList//상태 관리를 위한 리스트
            = new List<EnemyPatteurnStateInit>();
        private int runningPatteurnStateIndex = -1;
        [SerializeField]
        private int startPatteurnIndex = 0;//시작할 패턴
        [SerializeField]
        private float startDelayTime = 1;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            patteurnStateContext = new StateContext<EnemyController>(this);//콘텍스트 생성

            if (IsServer)
            {
                Invoke("PatteurnStateInitRpc", startDelayTime);
                OnSetStatusRpc();//스테이터스 값 초기화
            }

            nowHp.OnValueChanged += UpdateHpHUDRPC;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            nowHp.OnValueChanged -= UpdateHpHUDRPC;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsSpawned)
                patteurnStateContext.StateUpdate();
        }

        //캐릭터 스테이터스값 초기 설정
        [Rpc(SendTo.Server)]
        private void OnSetStatusRpc()
        {
            if (enemyConfig)
            {
                baseMaxHP.Value = enemyConfig.MaxHP;
                nowHp.Value = baseMaxHP.Value;
                GrowthAttackPower.Value = enemyConfig.GrowthAttackPower;
                GrowthPerAttackPower.Value = enemyConfig.GrowthPerAttackPower;
                baseAttackPower.Value = enemyConfig.AttackPower;
                SetEnemyNameRpc(enemyConfig.characterName);
            }
        }

        //패턴 상태 초기화 함수
        [Rpc(SendTo.ClientsAndHost)]
        public void PatteurnStateInitRpc()
        {
            IState<EnemyController> state = null;
            state = patteurnStateList[startPatteurnIndex].GetComponent<IState<EnemyController>>();
            runningPatteurnStateIndex = startPatteurnIndex;
            patteurnStateContext.TransitionTo(state);
        }
        //패턴 상태 변환 함수
        [Rpc(SendTo.Server)]
        public void PatteurnStateTransitionRpc(int index)
        {
            Debug.Log(index + "패턴 전환");
            IState<EnemyController> state = null;
            state = patteurnStateList[index].GetComponent<IState<EnemyController>>();
            runningPatteurnStateIndex = index;
            patteurnStateContext.TransitionTo(state);
        }

        //---------서버에서의 애니메이션 파라미터 설정을 하는 메서드들-------------

        //Float타입의 파라미터를 변경하는 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void SetFloatAniParaRpc(string paraName, float setValue)
        {
            _animator.SetFloat(paraName, setValue);
        }

        //Int타입의 파라미터를 변경하는 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void SetIntAniParaRpc(string paraName, int setValue)
        {
            _animator.SetInteger(paraName, setValue);
        }

        //Bool타입의 파라미터를 변경하는 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void SetBoolAniParaRpc(string paraName, bool setValue)
        {
            _animator.SetBool(paraName, setValue);
        }

        //Trigger타입의 파라미터를 변경하는 메서드
        [Rpc(SendTo.ClientsAndHost)]
        public void SetTriggerAniParaRpc(string paraName)
        {
            _animator.SetTrigger(paraName);
        }

        //보스 이름 설정 메서드
        [Rpc(SendTo.ClientsAndHost)]
        private void SetEnemyNameRpc(string name)
        {
            EnemyUIController.Instance.enemyHud.Name.text = name;
        }

        //보스 체력 바 업데이트 메서드
        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateHpHUDRPC(int previousValue, int newValue)
        {
            EnemyUIController.Instance.HpUpdate(MaxHP, newValue);
        }
    }
}
