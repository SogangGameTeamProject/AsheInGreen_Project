using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class JumpState : CharacterStateBase
    {
        public MovementStateType onChangeType = MovementStateType.Idle;//점프 종료 시 전환할 상태타입

        private Rigidbody2D rBody = null;
        public override void Enter(CharacterController character)
        {
            base.Enter(character);

            if (rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            if (_character._movementController.isGrounded)
                _character._movementController.MovementStateTransition(onChangeType);
        }

        public override void Exit()
        {
            
        }
    }
}