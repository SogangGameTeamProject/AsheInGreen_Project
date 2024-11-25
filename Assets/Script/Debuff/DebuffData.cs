using UnityEngine;

namespace AshGreen.Debuff
{
    //
    public enum DebuffType
    {
        Burn = 0,// 화상
        Corruption,// 부패
        PartDestruction,// 부위파괴 
        Wound,// 상처
        breakdown// 붕괴
    }

    [CreateAssetMenu(fileName = "DebuffData", menuName = "Scriptable Objects/DebuffData")]
    public class DebuffData : ScriptableObject
    {
        public DebuffType debuffType;
    }
}