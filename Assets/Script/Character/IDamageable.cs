using System;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public enum AttackType
    {
        None = 0,
        //플레이어 공격
        MainSkill = 101, SecondarySkill, SpecialSkill,
        //아이템
        Item = 201,
        //디버프
        Debuff = 301,
        //적 공격
        Enemy = 401,
    }

    public interface IDamageable
    {
        //피격 처리 메서드
        public void TakeDamageRpc(float damage);

        //타격 처리 메서드
        public void DealDamageRpc(NetworkObjectReference target, float damageCoefficient, AttackType attackType);
    }
}
