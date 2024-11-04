using AshGreen.Sound;
using AshGreen.State;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public abstract class EnemyPatteurnStateInit : NetworkBehaviour, IState<EnemyController>
    {
        protected EnemyController _enemy = null;
        protected SoundManager _soundManager = null;
        protected Coroutine runningCoroutine = null;
        [SerializeField]
        private int nextPatteurnIndex = 0;

        public virtual void Enter(EnemyController controller)
        {
            //플레이어 초기화
            if (_enemy == null)
                _enemy = controller;
            //사운드 매니저 초기화
            if (_soundManager == null)
                _soundManager = SoundManager.Instance;
            
            runningCoroutine = StartCoroutine(ExePatteurn());
        }

        public abstract void StateUpdate();

        public virtual void Exit()
        {
            _enemy.
        }

        //보스 패턴을 실질적으로 구현할 추상 코루틴
        protected abstract IEnumerator ExePatteurn();

        /// <summary>
        /// 플레이어의 위치를 구하는 메서드
        /// </summary>
        /// <param name="findType">플레이어 찾는 방식 0: 가까운거 1: 랜덤</param>
        /// <returns>플레이어 위치</returns>
        protected Vector2 GetPlayerPos(int findType)
        {
            Vector2 playerPos = Vector2.zero;

            return playerPos;
        }
    }
}

