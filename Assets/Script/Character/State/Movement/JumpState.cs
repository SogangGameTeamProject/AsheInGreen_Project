using AshGreen.Character.Player;
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
            //이동 방향에 따른 방향 전환
            if (rBody.linearVelocityX > 0.1f && _character.CharacterDirection == CharacterDirection.Left)
            {
                _character.CharacterDirection = CharacterDirection.Right;
            }
            else if (rBody.linearVelocityX < -0.1f && _character.CharacterDirection == CharacterDirection.Right)
            {
                _character.CharacterDirection = CharacterDirection.Left;
            }

            if (!IsOwner) 
                return;

            if (((PlayerController)_character)._movementController.isGrounded)
                ((PlayerController)_character)._movementController.MovementStateTransitionRpc(onChangeType);
        }

        public override void Exit()
        {
            
        }
    }
}