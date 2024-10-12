using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSpecialSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/스페셜 스킬")]
    public class AshSpecialSkill : CharacterSkill
    {
        [Header("특수스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public float bulletSpeed = 200f;
        public float BulletDecayTime = 2;
        public float fireDelay = 0.05f;
        public float casterGrvity = 5;

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("특수스킬 사용");
            Debug.Log("소모 에너지: " + holder.NowEnergy);
            int nowEnergy = holder.NowEnergy;
            holder.NowEnergy = 0;//에너지 비우기

            //스킬 종료 처리
            holder._caster._movementController.isUnableMove = true;//이동 불가
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.linearVelocity = Vector2.zero;
            casterRbody.gravityScale = 0;//중력 설정

            //에너지 수치 만큼 총알 발싸
            for (int i = 0; i < nowEnergy; i++)
            {
                // 서버에 총알 생성을 요청하는 RPC 호출
                Vector3 firePointPosition = holder._caster.firePoint.position;
                Quaternion firePointRotation = holder._caster.firePoint.rotation;

                // 서버에서 총알 생성
                GameObject bullet = Instantiate(bulletPrefab, firePointPosition, firePointRotation);

                // 총알을 네트워크 오브젝트로 설정하고 스폰
                NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
                if (bulletNetworkObject != null)
                {
                    bulletNetworkObject.Spawn();
                }

                // 시간 경과 후 총알 파괴
                Destroy(bullet, BulletDecayTime);

                // 총알의 물리적 움직임 처리
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.linearVelocity = new Vector2((int)holder._caster.CharacterDirection * bulletSpeed, 0); // 발사 방향 설정

                yield return new WaitForSeconds(fireDelay);
            }

            yield return End(holder);
        }

        public override IEnumerator End(SkillHolder holder)
        {
            //스킬 종료 처리
            holder._caster._movementController.isUnableMove = false;//이동 가능
            Rigidbody2D casterRbody = holder._caster.GetComponent<Rigidbody2D>();
            casterRbody.gravityScale = casterGrvity;//중력 설정

            return base.End(holder);
        }
    }
}