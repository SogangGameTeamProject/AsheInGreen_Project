using AshGreen.Character.Player;
using AshGreen.Sound;
using AshGreen.State;
using System.Collections;
using System.Collections.Generic;
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
        private int nextPatteurnIndex = 0;//다음 패턴 인덱스 번호

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

        public virtual void StateUpdate()
        {
            //사망시 패턴 강제 종료
            if(_enemy.runningCombatStateType == CombatStateType.Death)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
        }

        public abstract void Exit();

        //보스 패턴을 실질적으로 구현할 추상 코루틴
        protected virtual IEnumerator ExePatteurn()
        {
            //다음 패턴 전환
            runningCoroutine = null;
            _enemy.PatteurnStateTransitionRpc(nextPatteurnIndex);
            yield return null;
        }

        /// <summary>
        /// 플레이어의 위치를 구하는 메서드
        /// </summary>
        /// <param name="findType">플레이어 찾는 방식 0: 가까운거 1: 랜덤</param>
        /// <returns>플레이어 위치</returns>
        protected Vector2 GetPlayerPos(int findType)
        {
            Vector2 returnPos = Vector2.zero;
            Vector2 thisPos = _enemy.gameObject.transform.position;
            //모든 플레이어 컨트롤러 리스트로 저장
            List<PlayerController> players = new List<PlayerController>(FindObjectsOfType<PlayerController>());

            //죽은 플레이어 리스트에서 삭제
            foreach (var player in players)
            {
                if(player.runningCombatStateType == CombatStateType.Death)
                    players.Remove(player);
            }
            //가까운 플레이어 찾기
            if(findType == 0)
            {
                float distance = 99999;

                foreach (var player in players)
                {
                    Vector2 pPos = (Vector2)player.transform.position;
                    float dis = Vector2.Distance(pPos, thisPos);
                    if (dis < distance)
                    {
                        distance = dis;
                        returnPos = pPos;
                    }
                }
            }
            //랜덤 플레이어 찾기
            else
            {
                // 랜덤 인덱스를 사용하여 리스트에서 랜덤 값 추출
                int randomIndex = Random.Range(0, players.Count-1);

                returnPos = players[randomIndex].gameObject.transform.position;
            }

            return returnPos;
        }

    }
}

