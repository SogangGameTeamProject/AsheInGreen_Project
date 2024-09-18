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

            Debug.Log("아이들 체크");

            //점프 체크
            if (!_character._movementController.isGrounded)
            {
                _character._movementController.MovementStateTransitionServerRpc(onJumpType);
                return;
            }


            //이동 체크
            if (Mathf.Round(rBody.linearVelocityX) != 0)
                _character._movementController.MovementStateTransitionServerRpc(onMoveType);
        }

        public override void Exit()
        {
            
        }
    }
}