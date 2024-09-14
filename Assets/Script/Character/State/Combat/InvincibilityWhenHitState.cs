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

            _character.SetDamageimmunityServerRpc(true);
        }

        public override void Exit()
        {
            base.Exit();
            _character.SetDamageimmunityServerRpc(false);
        }
    }
}