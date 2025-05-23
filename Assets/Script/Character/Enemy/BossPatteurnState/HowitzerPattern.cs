using AshGreen.Platform;
using AshGreen.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Character{
    public class HowitzerPattern : EnemyPatteurnStateInit
    {
        [SerializeField]
        private float moveSpeed = 100f;
        [SerializeField]
        private Vector2 oriPos;
        [SerializeField] 
        private Vector2 centerPos;
        [SerializeField]
        private GameObject fireHowitzerPre = null;
        [SerializeField]
        private Transform fireHowitzerPoint = null;
        [SerializeField]
        private GameObject howitzerPre = null;
        [SerializeField]
        private float howitzerSpeed = 50f;
        [SerializeField]
        private float howitzerLifeTime = 2f;
        [SerializeField]
        private float attackCofficient = 1; 
        [SerializeField]
        private float howitzerInterval = 2f;
        [SerializeField]
        private float howitzerY = 12f;
        [SerializeField]
        private int howitzerFireNum = 2;//곡사포 발사 횟수
        [SerializeField]
        private int howitzerCnt = 3;//곡사포 한번 발사시 소환할 곡사포 수
        [SerializeField]
        private float fireFirstDelay = 2f;
        [SerializeField]
        private float fireLastDelay = 0.35f;
        [SerializeField]
        private GameObject projectileWring = null;
        [SerializeField]
        private AudioClip attactSound = null;

        public override void Enter(EnemyController controller)
        {
            base.Enter(controller);
            oriPos = controller.transform.position;
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
            //보스 중앙으로 이동
            while (true)
            {
                float distance = Vector2.Distance(_enemy.transform.position, centerPos);
                if (distance <= 0.1f)
                    break;

                float t = Mathf.Clamp(1 / (distance + 1), 0.01f, 1f);

                _enemy.transform.position
                    = Vector2.Lerp(_enemy.transform.position, centerPos, t * Time.deltaTime * moveSpeed);

                yield return null;
            }

            //곡사포 발사
            for(int i = 0; i < howitzerFireNum; i++)
            {
                yield return new WaitForSeconds(fireFirstDelay);
                //랜덤 플레이어 위치와 가장 가까운 플랫폼 선택
                Vector2 randomP = GetPlayerPos(1);
                Vector2 targetP = _enemy.transform.position;
                float minDistance = 999;
                foreach (var platform in PlatformManager.Instance.platformList)
                {
                    Vector2 platformPos = platform.transform.position;
                    float distance = Vector2.Distance(platformPos, randomP);
                    if (distance < minDistance)
                    {
                        targetP = platformPos;
                        minDistance = distance;
                    }
                }

                //타겟 공격
                float startX = targetP.x - (howitzerCnt-1) * howitzerInterval/2;
                List<Vector2> firePoints = new List<Vector2>();
                for (int j = 0; j < howitzerCnt; j++)
                {
                    firePoints.Add(new Vector2(startX, howitzerY));
                    startX += howitzerInterval;
                }
                List<Vector2> getRandomFirePoints = new List<Vector2>();
                //경고 표시 발사
                for (int j = 0; j < howitzerCnt; j++)
                {

                    if (attactSound)
                        SoundManager.Instance.PlaySFXRpc(attactSound);
                    _enemy.SetTriggerAniParaRpc("IsFshoot");

                    //공격 포탄 발사
                    ProjectileFactory.Instance.RequestWaringTargetFire(fireHowitzerPre, Vector2.up * howitzerSpeed * 2,
                        fireHowitzerPoint.position, 2f);

                    //경고 표시 발사
                    int randomIndex = Random.Range(0, firePoints.Count - 1);
                    Vector2 firePoint = firePoints[randomIndex];
                    getRandomFirePoints.Add(firePoint);
                    firePoints.RemoveAt(randomIndex);
                    
                    ProjectileFactory.Instance.RequestWaringTargetFire(projectileWring, Vector2.down * howitzerSpeed*2,
                        firePoint, fireLastDelay*4);
                    yield return new WaitForSeconds(fireLastDelay);

                }

                //투사체 발사
                for (int j = 0; j < howitzerCnt; j++)
                {
                    
                    CharacterController character = _enemy.GetComponent<CharacterController>();
                    ProjectileFactory.Instance.RequestProjectileFire(character, howitzerPre, AttackType.Enemy, attackCofficient
                        , Vector2.down * howitzerSpeed, getRandomFirePoints[j], Quaternion.identity, howitzerLifeTime);
                    yield return new WaitForSeconds(fireLastDelay);
                }
            }

            //원위치로 이동
            while (true)
            {
                float distance = Vector2.Distance(_enemy.transform.position, oriPos);
                if (distance <= 0.1f)
                    break;

                float t = Mathf.Clamp(1 / (distance + 1), 0.01f, 1f);

                _enemy.transform.position
                    = Vector2.Lerp(_enemy.transform.position, oriPos, t * Time.deltaTime * moveSpeed);

                yield return null;
            }

            yield return base.ExePatteurn();
        }
    }
}
