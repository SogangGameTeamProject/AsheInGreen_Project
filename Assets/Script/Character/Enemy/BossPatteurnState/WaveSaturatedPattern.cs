using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character{
    public class WaveSaturatedPattern : EnemyPatteurnStateInit
    {
        //이동 관련
        [SerializeField]
        private float moveSpeed = 100f;
        [SerializeField]
        private Vector2 centerPos = Vector2.zero;
        [SerializeField]
        private Vector2 oriPos = Vector2.zero;

        //공격 관련
        [SerializeField]
        private GameObject waveAttackArea = null;//파도 공격 에리어
        [SerializeField]
        private float waveAttactLastDelay = 1.5f;
        [SerializeField]
        private float fireFirstDelay = 2f;
        [SerializeField]
        private float fireDelay = 0.3f;
        [SerializeField]
        private float lsatDelay = 2f;
        [SerializeField]
        private GameObject bulletPre = null;
        [SerializeField]
        private float attackCofficient = 1.0f;
        [SerializeField]
        private Transform leftFirePoint= null;
        [SerializeField]
        private Transform rightFirePoint= null;

        //플렛폼 소환 관련
        [SerializeField]
        private List<Vector2> seagullSpawnPoints;
        [SerializeField]
        private GameObject seagullPre = null;
        [SerializeField]
        private List<Vector2> platformSpawnPoints;
        [SerializeField]
        private GameObject platformPre = null;


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
            //가운데로 위치 이동
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

            //파도소환
            SetDamageAreaRpc(true);
            yield return new WaitForSeconds(waveAttactLastDelay);
            SetDamageAreaRpc(false);

            //갈메기 소환
            foreach(var seagull in seagullSpawnPoints)
            {
                ProjectileFactory.Instance.RequestObjectSpawn(seagullPre, seagull);
            }

            yield return new WaitForSeconds(fireFirstDelay);

            //갈메기 격추
            for(int i = 0; i < seagullSpawnPoints.Count/2; i++)
            {
                _enemy.SetTriggerAniParaRpc("IsFshoot");
                //왼쪽 타겟 공격
                ProjectileFactory.Instance.RequestProjectileTargetFire(_enemy, bulletPre, AttackType.Enemy, attackCofficient,
                    seagullSpawnPoints[i], leftFirePoint.position, Quaternion.identity);
                //오른쪽 타겟 공격
                ProjectileFactory.Instance.RequestProjectileTargetFire(_enemy, bulletPre, AttackType.Enemy, attackCofficient,
                    seagullSpawnPoints[seagullSpawnPoints.Count-i-1], rightFirePoint.position, Quaternion.identity);

                yield return new WaitForSeconds(fireDelay);
            }

            //플렛폼 소환

            foreach (var platform in platformSpawnPoints)
            {
                ProjectileFactory.Instance.RequestObjectSpawn(platformPre, platform);
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

            yield return new WaitForSeconds(lsatDelay);

            yield return base.ExePatteurn();
        }

        //파도 데미지 영역 활성화 메서드
        [Rpc(SendTo.ClientsAndHost)]
        private void SetDamageAreaRpc(bool value)
        {
            waveAttackArea.SetActive(value);
        }
    }
}
