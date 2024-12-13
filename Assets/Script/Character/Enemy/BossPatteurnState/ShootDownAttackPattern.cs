using AshGreen.Platform;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character{
    public class ShootDownAttackPattern : EnemyPatteurnStateInit
    {
        //플랫폼 이동 경고 표시
        [SerializeField]
        private GameObject movePlatformWringPre = null;
        [SerializeField]
        private Vector2 movePlatformWingPoint = Vector2.zero;

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
        private int attackNum = 6;
        [SerializeField]
        private float attackCofficient = 1;
        [SerializeField]
        private float patternStartDealy = 2f;
        [SerializeField]
        private float seagullFirePatternDealy = 5f;
        [SerializeField]
        private float fireFirstDelay = 1f;
        [SerializeField]
        private float fireDelay = 1.6f;
        [SerializeField]
        private float lastDeay = 2f;
        [SerializeField]
        private GameObject projectileWring = null;

        private Coroutine seagullCoroutine = null;

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
            if(seagullCoroutine != null)
            {
                StopCoroutine(seagullCoroutine);
                seagullCoroutine = null;
            }
        }

        protected override IEnumerator ExePatteurn()
        {
            ProjectileFactory.Instance.RequestObjectSpawn(movePlatformWringPre, movePlatformWingPoint, patternStartDealy);
            //갈매기 소환 코루틴 호출
            seagullCoroutine = StartCoroutine(SpawnSeagull());
            yield return new WaitForSeconds(patternStartDealy);

            //흡수공격 처리 오브젝트 활성화
            _enemy.SetBoolAniParaRpc("IsAbsorb", true);

            //플랫폼 이동
            PlatformManager platformManager = PlatformManager.Instance;
            foreach(var platform in platformManager.platformList)
            {
                platform.StateTransitionRpc(PlatformStateType.MOVE);
            }

            yield return new WaitForSeconds(seagullFirePatternDealy);


            //갈메기 플렛폼 공격
            _enemy.SetBoolAniParaRpc("IsAbsorb", false);
            int fireNum = 0;
            while (fireNum < attackNum)
            {
                fireNum++;
                //공격 할 플렛폼 탐색
                bool isFind = false;
                Transform target = null;
                Vector2 playerP = GetPlayerPos(1);
                float minDistance = 999;
                foreach(var platform in platformManager.platformList)
                {
                    float distance = Vector2.Distance(playerP, platform.transform.position);
                    if (distance < minDistance)
                    {
                        isFind = true;
                        minDistance = distance;
                        target = platform.transform;
                    }
                }

                //타겟을 못찾으면 브레이크
                if (!isFind)
                    break;
                //경고 표시 설정
                ProjectileFactory.Instance.RequestObjectSpawn(projectileWring, target.position, 0, target);
                yield return new WaitForSeconds(fireFirstDelay);

                //타겟 공격
                _enemy.SetTriggerAniParaRpc("IsShooting");
                if(target != null)
                    ProjectileFactory.Instance.RequestProjectileTargetFire(_enemy, bulletPre, AttackType.Enemy, attackCofficient,
                    target.position, firePoint.position, Quaternion.identity);

                yield return new WaitForSeconds(fireDelay);
            }

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

                ProjectileFactory.Instance.RequestObjectSpawn(seagullPre, seagullSpawnPoint[seagullIndex]);

                if (spawnCnt >= seagullSpawnNum)
                    break;

                seagullIndex += addIndex;
                if (!(seagullIndex > 0 && seagullIndex < seagullSpawnPoint.Count-1))
                    addIndex *= -1;
                
                yield return new WaitForSeconds(seagullSpawnDelay);
            }
        }
    }
}
