using AshGreen.Character.Player;
using AshGreen.Character.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AshGreen.UI
{
    public class OpenSkillTooltipPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //스킬 툴팁 팝업
        [SerializeField]
        private GameObject _skillTooltipPopup;
        //스킬 툴팁 위치
        [SerializeField]
        private Vector2 _offset;
        //스킬 데이터
        [HideInInspector]
        public CharacterSkill skil;
        //스킬 툴팁
        private GameObject tooltip = null;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenItemTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseItemTooltip();
        }

        public void OpenItemTooltip()
        {
            if(skil == null) return;
            if (tooltip)
            {
                Destroy(tooltip);
                tooltip = null;
            }

            tooltip = Instantiate(_skillTooltipPopup, transform.position + (Vector3)_offset, Quaternion.identity, transform.parent.parent.parent.parent);
            
            //아이템 툴팁 설정
            tooltip.GetComponent<SkillToolltipPopup>().SetSkillTooltip(skil);
        }

        public void CloseItemTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip);
                tooltip = null;
            }
        }
    }
}