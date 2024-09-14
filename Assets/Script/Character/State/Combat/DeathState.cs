using UnityEngine;

namespace AshGreen.Character
{
    public class DeathState : CharacterStateBase
    {
        public string stateAni = "IsDeath";
        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            _animator.SetTrigger(stateAni);
        }

        public override void StateUpdate()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}