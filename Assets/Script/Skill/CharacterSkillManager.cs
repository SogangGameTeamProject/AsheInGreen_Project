using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

namespace AshGreen.Character.Skill
{
    
    public class CharacterSkillManager : NetworkBehaviour
    {
        private CharacterController _character = null;

        public enum CharacterSkillStatetype
        {
            Idle, Charge, Use
        }

        //스킬 리스트
        public List<SkillHolder> skillList = new List<SkillHolder>();


        private void Start()
        {
            _character = GetComponent<CharacterController>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _character = GetComponent<CharacterController>();

            if (IsOwner)
            {
                //스킬 초기화
                foreach(CharacterSkill skill in _character.baseConfig.skills)
                {
                    SkillHolder skillHolder = new SkillHolder(_character, skill);
                    skillList.Add(skillHolder);
                }
            }
        }
    }
}
