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
            if (IsServer)
            {
                Debug.Log("피격: "+this.gameObject.name);
                TakeDamageAction?.Invoke(damage);
            }
                
        }

        //타격 처리 메서드
        public void DealDamage(CharacterController target, float damage, AttackType attackType, bool isCritical = false)
        {
            if (IsServer)
            {
                Debug.Log("타격: " + this.gameObject.name);
                DealDamageAction?.Invoke(target, damage, attackType, isCritical);
                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
                
        }
    }
}
