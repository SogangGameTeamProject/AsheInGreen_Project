using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AshGreen.Character.Player;
using AshGreen.Character;
using AshGreen.Character.Skill;

namespace AshGreen.UI
{
    public class SkillToolltipPopup : MonoBehaviour
    {
        [HideInInspector]
        public CharacterSkill skill;//스킬 데이터

        //아이템 툴팁 팝업
        [SerializeField]
        private TextMeshProUGUI skillNameTxt;
        [SerializeField]
        private TextMeshProUGUI skillTypeTxt;
        [SerializeField]
        private TextMeshProUGUI skillCoolTimeTxt;
        [SerializeField]
        private TextMeshProUGUI skillDecoTxt;

        //버프디버프 팝업
        [SerializeField]
        private GameObject effectPopup;
        [SerializeField]
        private TextMeshProUGUI effectNameTxt;
        [SerializeField]
        private Image effectIconImg;
        [SerializeField]
        private TextMeshProUGUI effectDurationTxt;
        [SerializeField]
        private TextMeshProUGUI effectDecoTxt;


        public void SetSkillTooltip(CharacterSkill data)
        {
            skill = data;
            switch (skill.skillType)
            {
                case SkillType.MainSkill:
                    skillTypeTxt.text = "메인 스킬";
                    break;
                case SkillType.SecondarySkill:
                    skillTypeTxt.text = "보조 스킬";
                    break;
                case SkillType.SpecialSkill:
                    skillTypeTxt.text = "특수 스킬";
                    break;
            }

            skillNameTxt.text = skill.skillName;
            skillCoolTimeTxt.text = skill.cooldownTime > 0 ? skill.cooldownTime.ToString() : "없음";
            skillDecoTxt.text = skill.DescriptionTxt();

            if(skill.buffData != null)
            {
                effectNameTxt.text = skill.buffData.buffName;
                effectIconImg.sprite = skill.buffData.buffIcon;
                effectDurationTxt.text = skill.buffData.duration > 0 ?
                    skill.buffData.duration.ToString() : "없음";
                effectDecoTxt.text = skill.buffData.DescriptionTxt();
            }
            else if(skill.debuffData != null)
            {
                effectNameTxt.text = skill.debuffData.debuffName;
                effectIconImg.sprite = skill.debuffData.debuffIcon;
                effectDurationTxt.text = skill.debuffData.duration > 0 ?
                    skill.debuffData.duration.ToString() : "없음";
                effectDecoTxt.text = skill.debuffData.DescriptionTxt();
            }
            else
            {
                effectPopup.SetActive(false);
            }
        }
    }
}