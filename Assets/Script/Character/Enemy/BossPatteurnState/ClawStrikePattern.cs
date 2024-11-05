using AshGreen.Platform;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace AshGreen.Character{
    public class ClawStrikePattern : EnemyPatteurnStateInit
    {
        [SerializeField]
        private float attackNum = 3;//공격 횟 수
        [SerializeField]
        private float attackSpeed = 50f;//공격 스피드
        [SerializeField]
        private float minTargetingRange = 5f;//타겟과의 최소 거리
        [SerializeField]
        private Vector2 oriPos;
        [SerializeField]
        private float fistDealay = 2f;//공격 선딜
        [SerializeField]
        private Transform attackPoint = null;
        [SerializeField]
        private GameObject attackPre = null;
        public override void Enter(EnemyController controller)
        {
            base.Enter(controller);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
        }

        public override void Exit()
        {
            
        }

        protected override IEnumerator ExePatteurn()
        {
            oriPos = _enemy.transform.position;
            //플렛폼 공격
            for (int i = 0; i<attackNum; i++)
            {
                //공격 선 딜레이
                yield return new WaitForSeconds(fistDealay);

                //랜덤 플레이어 위치와 가장 가까운 플랫폼 선택
                Vector2 randomP = GetPlayerPos(1);
                Vector2 targetP = _enemy.transform.position;
                float minDistance = 999;
                foreach(var platform in PlatformManager.Instance.platformList)
                {
                    Vector2 platformPos = platform.transform.position;
                    float distance = Vector2.Distance(platformPos, _enemy.transform.position);
                    if(minDistance > distance)
                    {
                        targetP = platformPos;
                        minDistance = distance;
                    }
                }
                Debug.Log($"targetP: {targetP} minDistance: {minDistance}");
                //타겟 위치로 이동
                float targetDistance = minDistance;
                while (targetDistance <= minTargetingRange)
                {
                    Debug.Log("이동 중");
                    //타겟과의 거리 갱신
                    Vector2 target = new Vector2(targetP.x, 0);
                    Vector2 enemy = new Vector2(_enemy.transform.position.x, 0);
                    targetDistance = Vector2.Distance(target, enemy);

                    //이동
                    Vector2 moveVec = (target - enemy).normalized;
                    Debug.Log("이동방향: " + moveVec);
                    _rbody.linearVelocity = moveVec * attackSpeed;

                    yield return null;
                }

                //타겟 공격


                //원래 위치로 이동
                while (true)
                {
                    float distance = Vector2.Distance(oriPos, _enemy.transform.position);

                    Vector2 moveVec = oriPos - (Vector2)_enemy.transform.position;
                    moveVec.Normalize();
                    _rbody.linearVelocityX = moveVec.x * attackSpeed;

                    if (distance <= 0.5f)
                        break;
                    yield return null;
                }

            }

            yield return base.ExePatteurn();
        }
    }
}
