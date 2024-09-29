using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;
using static AshGreen.Character.Skill.SkillHolder;
using UnityEditor.Experimental.GraphView;

namespace AshGreen.Character.Skill
{
    
    public class CharacterSkillManager : NetworkBehaviour
    {
        public CharacterController _character = null;

        public enum CharacterSkillStatetype
        {
            Idle, Charge, Use
        }

        //스킬 리스트
        public List<SkillHolder> skillList = new List<SkillHolder>();


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                Debug.Log(_character.baseConfig);
                Debug.Log(_character.baseConfig.skills);
                //스킬 초기화
                foreach(CharacterSkill skill in _character.baseConfig.skills)
                {
                    SkillHolder skillHolder = new SkillHolder(_character, skill);
                    skillList.Add(skillHolder);
                }
            }
        }

        public void Update()
        {
            //홀더 업데이트
            if (IsOwner)
            {
                //스킬 초기화
                foreach (SkillHolder holder in skillList)
                {
                    holder.Update();
                }
            }
        }

        //스킬 입력 처리
        public void PresseSkill(int index)
        {
            if (skillList[index].skill.charging)
                skillList[index].Charging();
            else
                skillList[index].Use();
        }

        public void ReleaseSkill(int index)
        {
            if (skillList[index].skill.charging && skillList[index].state == SkillState.charge)
                skillList[index].Use();
        }
    }
}
