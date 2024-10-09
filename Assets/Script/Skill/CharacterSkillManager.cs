using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;
using static AshGreen.Character.Skill.SkillHolder;
using UnityEditor.Experimental.GraphView;
using AshGreen.Character.Player;

namespace AshGreen.Character.Skill
{
    
    public class CharacterSkillManager : NetworkBehaviour
    {
        public PlayerController _player = null;

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
                //스킬 초기화
                foreach(CharacterSkill skill in _player.baseConfig.skills)
                {
                    SkillHolder skillHolder = new SkillHolder(_player, skill);
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
            //스킬 입력 처리
            if (skillList[index].NowChargeCnt > 0 && skillList[index].state == SkillState.Idle)
            {
                //스킬 캔슬 여부 체크 및 처리
                bool skillCancel = skillList[index].skill.skillCancel;
                bool multipleUse = skillList[index].skill.multipleUse;

                int cnt = -1;
                foreach (SkillHolder holder in skillList)
                {
                    cnt++;

                    //자기거 체크 X
                    if (cnt == index)
                        continue;

                    SkillState state = holder.state;
                    bool isNotCancellation = holder.skill.isNotCancellation;

                    if(state != SkillState.Idle)
                    {
                        if (isNotCancellation || (!skillCancel && !multipleUse))
                            return;
                        else if(skillCancel)
                            holder.Stop();
                    }
                }

                //코스트 체크 후 사용 처리
                if(skillList[index].NowEnergy >= skillList[index].minUseCoast)
                {
                    if (skillList[index].skill.charging)
                        skillList[index].Charging();
                    else
                        skillList[index].Use();
                }
            }
        }

        public void ReleaseSkill(int index)
        {
            //스킬 입력처리
            if (skillList[index].skill.charging && skillList[index].state == SkillState.charge)
                skillList[index].Use();
        }

        //모든 스킬 캔슬
        public void AllStop()
        {
            foreach (SkillHolder holder in skillList)
            {
                if(holder.state != SkillState.Idle)
                    holder.Stop();
            }
        }
    }
}
