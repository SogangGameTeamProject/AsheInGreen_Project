using UnityEngine;
namespace AshGreen.Character.Skill
{
    public class SkillHolder : MonoBehaviour
    {
        public CharacterSkill skill = null;
        float cooldownTime;
        float activeTime;

        enum SkillState
        {
            ready,
            active,
            cooldown
        }

        private SkillState state = SkillState.ready;

        private void Update()
        {
            switch (state)
            {
                case SkillState.ready:

                    break;
                case SkillState.active:

                    break;
                case SkillState.cooldown:

                    break;
            }
        }
    }
}