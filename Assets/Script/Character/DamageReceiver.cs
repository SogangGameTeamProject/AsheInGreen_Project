using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace AshGreen.Character
{
    public class DamageReceiver : NetworkBehaviour, IDamageable
    {
        private CharacterController _character;

        public event Action<float> TakeDamageAction;//피격 시 호출되는 액션
        public event Action<CharacterController, float, AttackType, bool> DealDamageAction;//타격 시 호출되는 액션

        private void Start()
        {
            _character = GetComponent<CharacterController>();
        }

        //피격 처리 메서드
        public void TakeDamage(float damage)
        {

            if(_character.runningCombatStateType != CombatStateType.Death)
            {
                TakeDamageRpc(damage);
            }
        }

        [Rpc(SendTo.Owner)]
        public void TakeDamageRpc(float damage)
        {
            damage *= _character.TakenDamageCoefficient;
            TakeDamageAction?.Invoke(damage);
        }

        //타격 처리 메서드
        public void DealDamage(CharacterController target, float damageCoefficient, AttackType attackType)
        {

            Debug.Log("타격: " + this.gameObject.name);

            //데미지 계산
            float damage = 0;
            bool isCriticale = UnityEngine.Random.value <= _character.CriticalChance;
            Debug.Log("AttackPower: " + _character.AttackPower + " damageCoefficient: "+ damageCoefficient + " DealDamageCoefficient:" + _character.DealDamageCoefficient);
            damage = _character.AttackPower * damageCoefficient * _character.DealDamageCoefficient;


            DealDamageAction?.Invoke(target, damage, attackType, isCriticale);
        }
    }
}
