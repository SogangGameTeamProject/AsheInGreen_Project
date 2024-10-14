using AshGreen.Character.Player;
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
            {
                //스킬 캔슬
                ((PlayerController)_character)._characterSkillManager.AllStopRpc();
                _character.SetDamageimmunity(true);
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (IsOwner)
                _character.SetDamageimmunity(false);
        }
    }
}