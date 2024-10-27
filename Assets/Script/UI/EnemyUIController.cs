using AshGreen.Character.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : Singleton<EnemyUIController>
{
    [Serializable]
    public struct HUD
    {
        public Image HpBar;
        public TextMeshProUGUI Name;
    }

    public HUD enemyHud;
    [HideInInspector]
    public PlayerController player;

    public void HpUpdate(int maxHp, int nowHp)
    {
        float hp = (float)nowHp / (float)maxHp;
        enemyHud.HpBar.fillAmount = hp;
    }
}
