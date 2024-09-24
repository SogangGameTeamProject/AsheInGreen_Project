using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Character.Skill
{
    public abstract class CharacterSkill : ScriptableObject
    {
        public string skillName;   // 스킬 이름
        public float activeTime;   // 스킬 사용 시간
        public float cooldownTime; // 스킬 쿨타임
        public bool charging = false; //스킬 차징 여부
        public float chargingMoveSpeed = 0.75f; // 차징 시 이속
        public int maxChageCnt = 1;//최대 충전 수
        public float damageMultiplier = 1; // 스킬 데미지 배수
        public Sprite skillIcon;   // 스킬 아이콘
        public string animationTrigger; // 스킬 발동 시 애니메이션 트리거
        public AudioClip skillSound;    // 스킬 발동 소리

        public abstract void Initialize(CharacterController caster);

        // 스킬의 발동 로직
        public abstract void Use(float chageTime = 0);
    }
}

 