using AshGreen.Character.Player;
using AshGreen.Platform;
using UnityEngine;

namespace AshGreen.Character
{
    public class MovementIdleState : CharacterStateBase
    {
        public MovementStateType onJumpType = MovementStateType.Jump; // 바닥에서 떨어질 시 전환할 상태
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

            // 점프 체크
            if (!_movement.isGrounded)
            {
                _movement.MovementStateTransitionRpc(onJumpType);
                return;
            }

            // 플랫폼 정보 가져오기
            Collider2D collisionPlatform = Physics2D.OverlapBox(
                _movement.groundChecker.bounds.center,
                _movement.groundChecker.bounds.size,
                0,
                _movement.platformLayer
            );

            float platformVecX = 0;
            if (collisionPlatform != null)
            {
                Rigidbody2D platformRigidbody = collisionPlatform.gameObject.GetComponent<Rigidbody2D>();
                if (collisionPlatform != null)
                {
                    platformVecX = collisionPlatform.gameObject.GetComponent<PlatformController>().syncVelocity.Value.x;
                }
            }

            float playerVecX = rBody.linearVelocity.x - platformVecX;

            // 이동 체크
            if ((playerVecX > 0.5f || playerVecX < -0.5f)
                && _movement.isGrounded)
            {
                _movement.MovementStateTransitionRpc(onMoveType);
            }
        }

        public override void Exit()
        {
            // 상태 종료 시 필요한 로직이 있다면 여기에 추가
        }
    }
}