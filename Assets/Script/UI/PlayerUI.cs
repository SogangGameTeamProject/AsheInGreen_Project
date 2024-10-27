using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Netcode;
using AshGreen.Obsever;
using AshGreen.Character.Player;
using AshGreen.Character.Skill;


public class PlayerUI : MonoBehaviour
{
    [Serializable]
    public struct HUD
    {
        public Image playerIcon;
        public GameObject p1;
        public GameObject p2;
        public GameObject hpPanel;
        public GameObject hpPre;
        public Image leftLvImg;
        public Image rightLvImg;
        public Image mainSkillIcon;
        public GameObject mainSkillEnergy;
        public TextMeshProUGUI mainSkillEnergyTxt;
        public GameObject mainSkillTimer;
        public TextMeshProUGUI mainSkillTime;
        public Image secondarySkillIcon;
        public GameObject secondarySkillEnergy;
        public TextMeshProUGUI secondarySkillEnergyTxt;
        public GameObject secondarySkillTimer;
        public TextMeshProUGUI secondarySkillTime;
        public Image specialSkillIcon;
        public GameObject specialEnergy;
        public TextMeshProUGUI specialSkillEnergyTxt;
        public GameObject specialSkillTimer;
        public TextMeshProUGUI specialSkillTimeTxt;
    }
    
    public HUD playerHud;
    [HideInInspector]
    public PlayerController player;

    private void Start()
    {
        UpdateHp(player.MaxHP, player.NowHP);
    }

    //hp바 업데이트
    public void UpdateHp(int maxHp, int nowHp)
    {
        GridLayoutGroup hpGrid = playerHud.hpPanel.GetComponent<GridLayoutGroup>();
        float width = 450;
        float cellSizeX = width/maxHp;
        hpGrid.cellSize = new Vector2(cellSizeX, hpGrid.cellSize.y);

        // 하위 오브젝트를 전부 삭제
        foreach (Transform child in playerHud.hpPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < nowHp; i++)
        {
            Instantiate(playerHud.hpPre, playerHud.hpPanel.transform);
        }
    }

    public void UpdateLv(int lavel)
    {

    }

    public void UpdateSkill(SkillType skillType , float coolTime, int minUseCoast, int nowEnergy)
    {
        GameObject timer = null;
        TextMeshProUGUI timeTxt = null;

        GameObject skillEnergy = null;
        TextMeshProUGUI skillEnergyTxt = null;
        switch (skillType)
        {
            case SkillType.MainSkill:
                timer = playerHud.mainSkillTimer;
                timeTxt = playerHud.mainSkillTime;
                skillEnergy = playerHud.mainSkillEnergy;
                skillEnergyTxt = playerHud.mainSkillEnergyTxt;
                break;
            case SkillType.SecondarySkill:
                timer = playerHud.secondarySkillTimer;
                timeTxt = playerHud.secondarySkillTime;
                skillEnergy = playerHud.secondarySkillEnergy;
                skillEnergyTxt = playerHud.secondarySkillEnergyTxt;
                break;
            case SkillType.SpecialSkill:
                timer = playerHud.specialSkillTimer;
                timeTxt = playerHud.specialSkillTimeTxt;
                skillEnergy = playerHud.specialEnergy;
                skillEnergyTxt = playerHud.specialSkillEnergyTxt;
                break;
        }

        //쿨타임 적용
        if (coolTime > 0f)
        {
            timer.SetActive(true);
            timeTxt.text = Mathf.Ceil(coolTime).ToString();
        }
        else
        {
            timer.SetActive(false);
        }

        //에너지 적용
        if (minUseCoast > 0)
        {
            skillEnergy.SetActive(true);
            skillEnergyTxt.text = nowEnergy.ToString();
        }
        else
        {
            skillEnergy.SetActive(false);
        }
    }
}