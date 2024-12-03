using AshGreen.Platform;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace AshGreen.Character{
    public class ClawStrikePattern : EnemyPatteurnStateInit
    {
        [SerializeField]
        private float attackNum = 3;//공격 횟 수
        [SerializeField]
        private float attackCofficient = 1;
        [SerializeField]
        private float attackSpeed = 50f;//공격 스피드
        [SerializeField]
        private float minTargetingRange = 5f;//타겟과의 최소 거리
        [SerializeField]
        private Vector2 oriPos;
        [SerializeField]
        private float firstDealay = 0.5f;//공격 선딜
        [SerializeField]
        private float lastDealay = 0.5f;//공격 후딜

        //근접공격
        [SerializeField]
        private GameObject damageArea;
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
            base.Exit();
        }

        protected override IEnumerator ExePatteurn()
        {
            oriPos = _enemy.transform.position;
            //플렛폼 공격
            for (int i = 0; i<attackNum; i++)
            {
                //랜덤 플레이어 위치와 가장 가까운 플랫폼 선택
                Vector2 randomP = GetPlayerPos(1);
                Vector2 targetP = _enemy.transform.position;
                float minDistance = 999;
                foreach(var platform in PlatformManager.Instance.platformList)
                {
                    Vector2 platformPos = platform.transform.position;
                    float distance = Vector2.Distance(platformPos, randomP);
                    if(distance < minDistance)
                    {
                        targetP = platformPos;
                        minDistance = distance;
                    }
                }

                //타겟 위치로 이동
                while (true)
                {
                    float distance = Mathf.Abs(_enemy.transform.position.x - (targetP.x + minTargetingRange));
                    if (distance <= 0.1f)
                        break;

                    float t = Mathf.Clamp(1 / (distance + 1), 0.01f, 1f);

                    _enemy.transform.position  = Vector2.Lerp(_enemy.transform.position, 
                        new Vector2(targetP.x + minTargetingRange, targetP.y), t * Time.deltaTime * attackSpeed);

                    yield return null;
                }

                //타겟 공격
                yield return new WaitForSeconds(firstDealay);
                _enemy.SetTriggerAniParaRpc("IsSmash");
                yield return new WaitForSeconds(lastDealay);
            }

            //원래 위치로 이동
            while (true)
            {
                float distance = Mathf.Abs(_enemy.transform.position.x - oriPos.x);
                if (distance <= 0.05f)
                    break;

                float t = Mathf.Clamp(1 / (distance + 1), 0.01f, 1f);

                _enemy.transform.position
                    = Vector2.Lerp(_enemy.transform.position, oriPos, t * Time.deltaTime * attackSpeed);

                yield return null;
            }

            yield return base.ExePatteurn();
        }
    }
}
