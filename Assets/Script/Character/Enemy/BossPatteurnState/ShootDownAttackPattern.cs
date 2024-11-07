using AshGreen.Platform;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character{
    public class ShootDownAttackPattern : EnemyPatteurnStateInit
    {
        //흡수공격
        [SerializeField]
        private GameObject damageArea;

        //갈메기 소환
        [SerializeField]
        private GameObject seagullPre = null;
        [SerializeField]
        private List<Vector2> seagullSpawnPoint;
        [SerializeField]
        private float seagullSpawnDelay = 1.6f;
        [SerializeField]
        private float seagullSpawnNum = 10;

        //갈메기 공격
        [SerializeField]
        private GameObject bulletPre = null;
        [SerializeField]
        private Transform firePoint = null;
        [SerializeField]
        private float attackCofficient = 1;
        [SerializeField]
        private float seagullFirePatternDealy = 5f;
        [SerializeField]
        private float fireDelay = 1.6f;
        [SerializeField]
        private float lastDeay = 2f;

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
            //흡수공격 처리 오브젝트 활성화
            SetDamageAreaRpc(true);

            //플랫폼 이동
            PlatformManager platformManager = PlatformManager.Instance;
            foreach(var platform in platformManager.platformList)
            {
                platform.StateTransitionRpc(PlatformStateType.MOVE);
            }


            //갈매기 소환 코루틴 호출
            StartCoroutine(SpawnSeagull());

            yield return new WaitForSeconds(seagullFirePatternDealy);

            //갈메기 플렛폼 공격
            while (true)
            {
                //공격 할 플렛폼 탐색
                bool isFind = false;
                Vector2 targetP = Vector2.zero;
                Vector2 playerP = GetPlayerPos(1);
                float minDistance = 999;
                foreach(var platform in platformManager.platformList)
                {
                    float distance = Vector2.Distance(playerP, platform.transform.position);
                    if (distance < minDistance)
                    {
                        isFind = true;
                        minDistance = distance;
                        targetP = platform.transform.position;
                    }
                }

                //타겟을 못찾으면 브레이크
                if (!isFind)
                    break;

                //타겟 공격
                ProjectileFactory.Instance.RequestProjectileTargetFire(_enemy, bulletPre, AttackType.Enemy, attackCofficient,
                    targetP, firePoint.position, Quaternion.identity);

                yield return new WaitForSeconds(fireDelay);
            }

            //흡수공격 처리 오브젝트 비활성화
            SetDamageAreaRpc(false);

            yield return new WaitForSeconds(lastDeay);

            yield return base.ExePatteurn();
        }

        //갈메기 플렛폼 소환 코루틴
        private IEnumerator SpawnSeagull()
        {
            int seagullIndex = 0;
            int addIndex = 1;
            int spawnCnt = 0;
            while (true)
            {
                spawnCnt++;

                ProjectileFactory.Instance.RequestPlatformSpawn(seagullPre, seagullSpawnPoint[seagullIndex]);

                if (spawnCnt >= seagullSpawnNum)
                    break;

                seagullIndex += addIndex;
                if (!(seagullIndex > 0 && seagullIndex < seagullSpawnPoint.Count-1))
                    addIndex *= -1;
                
                yield return new WaitForSeconds(seagullSpawnDelay);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetDamageAreaRpc(bool value)
        {
            damageArea.SetActive(value);
        }
    }
}
