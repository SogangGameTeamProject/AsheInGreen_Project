using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using AshGreen.DamageObj;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshSpecialSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/스페셜 스킬")]
    public class AshSpecialSkill : CharacterSkill
    {
        [Header("특수스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public int bulletPreIndex = 0;
        public float bulletSpeed = 200f;
        public float bulletDestroyTime = 2;
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
                Vector3 firePointPosition = holder._caster.firePoint.position;//투사체 발사 위치 조정
                Quaternion firePointRotation = holder._caster.firePoint.rotation;//투사체 회전 조정
                NetworkObject owner = holder._caster.GetComponent<NetworkObject>();//공격자 설정
                float damage = damageCoefficient;//데미지 설정
                Vector2 fireDir = new Vector2((int)holder._caster.CharacterDirection, 0) * bulletSpeed;//발사 방향 조정

                holder._caster._characterProjectileFactory.RequestProjectileFireServerRpc(
                   owner, bulletPreIndex, AttackType.MainSkill, damage, fireDir,
                   firePointPosition, firePointRotation, bulletDestroyTime
                   );

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