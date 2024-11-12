using AshGreen.Character.Player;
using UnityEngine;

namespace AshGreen.Character
{
    public class MovementIdleState : CharacterStateBase
    {
        public MovementStateType onJumpType = MovementStateType.Jump;//바닥에서 떨어질 시 전환할 상태
        public MovementStateType onMoveType = MovementStateType.Move;

        private PlayerController _player = null;
        private MovementController _movement = null;
        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if (_player == null)
                _player = (PlayerController)_character;
            if (_movement == null)
                _movement = _player._movementController;
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

            //플랫폼 정보 가져오기
            Collider2D collisionPlatform =
                Physics2D.OverlapBox(_movement.groundChecker.bounds.center, _movement.groundChecker.bounds.size, 0,
                _movement.platformLayer);
            float platformVecX = 0;
            if (collisionPlatform != null)
                platformVecX = collisionPlatform.gameObject.GetComponent<Rigidbody2D>().linearVelocityX;
            float playerVecX = rBody.linearVelocityX - platformVecX;
            Debug.Log("정지 상태");
            //이동 체크
            if (Mathf.Round(playerVecX) != 0)
            {
                _movement.MovementStateTransitionRpc(onMoveType);
            }
        }

        public override void Exit()
        {
            
        }
    }
}