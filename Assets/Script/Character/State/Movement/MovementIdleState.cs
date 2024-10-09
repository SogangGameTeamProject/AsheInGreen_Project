using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Character
{
    public class MovementIdleState : CharacterStateBase
    {
        public MovementStateType onJumpType = MovementStateType.Jump;//바닥에서 떨어질 시 전환할 상태
        public MovementStateType onMoveType = MovementStateType.Move;

        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {

            if (!IsOwner)
                return;

            //점프 체크
            if (!((PlayerController)_character)._movementController.isGrounded)
            {
                ((PlayerController)_character)._movementController.MovementStateTransitionRpc(onJumpType);
                return;
            }


            //이동 체크
            if (Mathf.Clamp(rBody.linearVelocityX, -0.2f, 0.2f) != 0)
                ((PlayerController)_character)._movementController.MovementStateTransitionRpc(onMoveType);
        }

        public override void Exit()
        {
            
        }
    }
}