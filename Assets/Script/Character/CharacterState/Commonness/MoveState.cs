using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class MoveState : CharacterStateBase
    {
        public CharacterStateType onChangeType = CharacterStateType.Idle;//이동 종료 시 전환할 상태타입
        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if(rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {

            if (rBody.linearVelocityX == 0)
            {
                _character.StateTransition(onChangeType);
                return;
            }
                
            
        }

        public override void Exit()
        {
            
        }
    }
}