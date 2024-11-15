using AshGreen.Character.Player;
using AshGreen.Platform;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class MoveState : CharacterStateBase
    {
        private PlayerController _player = null;
        private MovementController _movement = null;
        public MovementStateType onChangeType = MovementStateType.Idle;//이동 종료 시 전환할 상태타입
        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if (_player == null)
                _player = (PlayerController)_character;
            if (_movement == null)
                _movement = _player._movementController;
            if (rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
            _animator.SetBool("IsMove", true);
        }

        public override void StateUpdate()
        {
            if (!IsOwner)
                return;

            //이동 방향에 따른 방향 전환
            if (rBody.linearVelocityX > 0.1f && _character.CharacterDirection == CharacterDirection.Left)
            {
                _character.CharacterDirection = CharacterDirection.Right;
            }
            else if (rBody.linearVelocityX < -0.1f && _character.CharacterDirection == CharacterDirection.Right)
            {
                _character.CharacterDirection = CharacterDirection.Left;
            }

            //플랫폼 정보 가져오기
            Collider2D collisionPlatform =
                Physics2D.OverlapBox(_movement.groundChecker.bounds.center, _movement.groundChecker.bounds.size, 0,
                _movement.platformLayer);

            float platformVecX = 0;
            if (collisionPlatform != null)
            {
                platformVecX = collisionPlatform.gameObject.GetComponent<PlatformController>().syncVelocity.Value.x;
            }
            float playerVecX = rBody.linearVelocityX - platformVecX;

            //이동 상태 종료 체크
            if ((playerVecX <= 0.5f && playerVecX >= -0.5f)
                && _movement.isGrounded)
            {
                _movement.MovementStateTransitionRpc(onChangeType);
                return;
            }
        }

        public override void Exit()
        {
            _animator.SetBool("IsMove", false);
        }
    }
}