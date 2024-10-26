using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Netcode;
using AshGreen.Obsever;
using AshGreen.Character.Player;


public class PlayerUI : Observer
{
    [Serializable]
    public struct HUD
    {
        public GameObject hpPanel;
        public GameObject hpPre;
        public Image leftLvImg;
        public Image rightLvImg;
        public Image mainSkillIcon;
        public Image secondarySkillIcon;
        public Image specialSkillIcon;
    }

    public HUD playerHud;

    public override void Notify(Subject subject)
    {
        if (!IsOwner) return;

        PlayerController controller = subject.GetComponent<PlayerController>();

        UpdateHpRpc(controller.MaxHP, controller.nowHp.Value);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateHpRpc(int maxHp, int nowHp)
    {
        
    }
}