using UnityEngine;

namespace AshGreen.Character
{
    public class MovementIdleState : CharacterStateBase
    {
        public MovementStateType onJumpType = MovementStateType.Jump;//바닥에서 떨어질 시 전환할 상태
        public MovementStateType onMoveType = MovementStateType.Move;
        //바닥 체크를 위한 설정값
        public LayerMask groundLayer;
        public Vector2 groundChkOffset = Vector2.zero;
        public float groundChkRadius = 0.15f;

        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            //점프 체크
            if (!_character._movementController.isGrounded)
            {
                _character._movementController.MovementStateTransition(onJumpType);
                return;
            }


            //이동 체크
            if (rBody.linearVelocityX != 0)
                _character._movementController.MovementStateTransition(onMoveType);
        }

        public override void Exit()
        {
            
        }
    }
}