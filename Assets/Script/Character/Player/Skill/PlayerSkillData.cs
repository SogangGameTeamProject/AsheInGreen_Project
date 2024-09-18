using UnityEngine;

namespace AshGreen.Character.Player.Skill
{
    [CreateAssetMenu(fileName = "PlayerSkillData", menuName = "Scriptable Objects/PlayerSkillData")]
    public class PlayerSkillData : ScriptableObject
    {
        public string skillName;   // 스킬 이름
        public float cooldownTime; // 스킬 쿨타임
        public float damageMultiplier; // 스킬 데미지 배수
        public Sprite skillIcon;   // 스킬 아이콘
        public string animationTrigger; // 스킬 발동 시 애니메이션 트리거
        public AudioClip skillSound;    // 스킬 발동 소리

        // 스킬의 발동 로직
        public virtual void Activate(GameObject player)
        {
            // 기본 스킬 발동 로직 구현
        }
    }
}

