using System.Collections;
using UnityEngine;

namespace AshGreen.Character
{
    public class InvincibilityWhenHitState : HitState
    {
        public string stateAni = "IsHeart";
        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            _animator.SetTrigger(stateAni);

            if (IsOwner)
                _character.SetDamageimmunityRpc(true);
        }

        public override void Exit()
        {
            base.Exit();
            if (IsOwner)
                _character.SetDamageimmunityRpc(false);
        }
    }
}