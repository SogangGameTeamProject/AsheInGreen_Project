using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

namespace AshGreen.Character.Skill
{
    [CreateAssetMenu(fileName = "AshMainSkill", menuName = "Scriptable Objects/스킬/플레이어/애쉬/메인 스킬")]
    public class AshMainSkill : CharacterSkill
    {
        [Header("메인스킬 옵션")]
        public GameObject bulletPrefab;//투사체 프리펩
        public float bulletSpeed = 200f;
        public float BulletDecayTime = 2;
        public float chargingTime = 0.3f;//차징 단계별 시간
        public int maxChargingCnt = 3;//최대 차징 횟수
        public int energyIncrease = 1; //단계별 충전 량

        public override IEnumerator Charging(SkillHolder holder)
        {
            Debug.Log("스킬 차징");
            
            return base.Charging(holder);
        }

        public override IEnumerator Use(SkillHolder holder, float chageTime = 0)
        {
            Debug.Log("메인스킬 사용");

            int chargeCnt = 
                Mathf.Clamp((int)(chageTime / chargingTime), 0, maxChargingCnt);//차징 단계 구하기
            Debug.Log("충전 량: " + chargeCnt);
            //차징 정도에 따른 에너지 충전
            holder._caster._characterSkillManager.skillList[2].NowEnergy
                += energyIncrease*chargeCnt;//특수스킬 에너지 충전

            //총알 발사 구현
            //총알 데미지 적용

            // 총알을 소환할 위치와 회전
            GameObject bullet = 
                Instantiate(bulletPrefab, holder._caster.firePoint.position, holder._caster.firePoint.rotation);
            
            // 총알을 NetworkObject로 인식하도록 설정
            NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();

            if (bulletNetworkObject != null)
            {
                bulletNetworkObject.Spawn(); // 총알을 네트워크 오브젝트로 생성 및 모든 클라이언트에 동기화
            }

            // 총알의 물리적 움직임 처리
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocityX = (int)holder._caster.CharacterDirection * bulletSpeed; // 발사 방향 설정

            // 시간 경과 후 총알 파괴
            Destroy(bullet, BulletDecayTime);

            return base.Use(holder);
        }
    }
}