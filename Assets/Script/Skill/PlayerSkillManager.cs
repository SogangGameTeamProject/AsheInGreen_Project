using UnityEngine;
using Unity.Netcode;

namespace AshGreen.Character.Skill
{
    public enum PlayerSkillType
    {
        mainSkill, SecondarySkill, SpecialSkill
    }
    public class PlayerSkillManager : NetworkBehaviour
    {
       private CharacterController _player = null;

        //스킬 관련
        public PlayerSkillHolder mainSkillHolder = null;
        public PlayerSkillHolder secondarySkillHolder = null;
        public PlayerSkillHolder specialSkillHolder = null;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _player = GetComponent<CharacterController>();

            if (IsOwner)
            {
                //스킬 초기화
                mainSkillHolder = new PlayerSkillHolder(_player, _player.baseConfig.skills[0]);
                secondarySkillHolder = new PlayerSkillHolder(_player, _player.baseConfig.skills[1]);
                specialSkillHolder = new PlayerSkillHolder(_player, _player.baseConfig.skills[2]);
            }
        }


    }
}
