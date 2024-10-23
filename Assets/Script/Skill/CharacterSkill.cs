using AshGreen.DamageObj;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AshGreen.Character.Skill
{
    public enum SkillType
    {
        Attack, Utill
    }

    public abstract class CharacterSkill : ScriptableObject
    {
        public string skillName;   // 스킬 이름
        public float activeTime;   // 스킬 사용 시간
        public float cooldownTime; // 스킬 쿨타임
        public int maxChageCnt = 1;// 최대 충전 수

        //코스트 관련
        public int MaxHaveEnergy = 0;
        public int MinUseCoast = 0;

        public bool charging = false; //스킬 차징 여부
        public float chargingMoveSpeed = 0.75f; // 차징 시 이속

        //캔슬 여부
        public bool isNotCancellation = false; // 해당 스킬 캔슬 불가능 여불
        public bool skillCancel = false;       // 타 스킬 캔슬 여부
        public bool multipleUse = false;       // 다중 사용 가능 여부

        public SkillType skillType;//스킬 타입

        //공격 스킬 시 설정
        public float damageCoefficient = 1; // 스킬 데미지 배수

        //유틸 스킬 시 설정
        public float utillValue = 1;
        public float utillTime = 1;

        public Sprite skillIcon;   // 스킬 아이콘
        public string animationTrigger; // 스킬 발동 시 애니메이션 트리거
        public AudioClip skillSound;    // 스킬 발동 소리

        //스킬 차징
        public virtual IEnumerator Charging(SkillHolder holder)
        {
            holder.state = SkillHolder.SkillState.charge;//차징 상태 전환
            float charginTime = 0;

            while (true)
            {
                charginTime += Time.deltaTime;
                if (holder.state == SkillHolder.SkillState.active)
                    break;
                yield return null;
            }

            holder.holderCorutine = holder._caster.StartCoroutine(Use(holder, charginTime));//차징 후 
        }

        //스킬 사용
        public virtual IEnumerator Use(SkillHolder holder, float chargeTime = 0)
        {
            
            holder.holderCorutine = holder._caster.StartCoroutine(End(holder));
            yield return null;
        }

        //스킬 종료 처리
        public virtual IEnumerator End(SkillHolder holder)
        {
            Debug.Log("종료");
            holder.state = SkillHolder.SkillState.Idle;

            yield return null;
        }


        public void Fire(CharacterController caster, GameObject bulletPre, AttackType attackType, float damage, Vector2 fireDir,
            Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            GameObject bullet = 
                        NetworkObjectSpawner.SpawnNewNetworkObjectAsPlayerObject(
                            bulletPre,
                            firePos,
                            NetworkManager.Singleton.LocalClientId,
                            true);

            // 시간 경과 후 총알 파괴
            if (destroyTime > 0)
                Destroy(bullet, destroyTime);

            //투사체 설정
            DamageObjBase damageObj = bullet.GetComponent<DamageObjBase>();

            damageObj.caster = caster;
            damageObj.dealType = attackType;
            damageObj.damage = damage;

            // 총알의 물리적 움직임 처리
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = fireDir; // 발사 방향 설정
        }
    }
}

 