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
            Debug.Log("아이들 상태");
            rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            //레이케스트로 바닥 위인지 체크 아니면 OnAir상태로 전환
            Vector2 playerPos = _character.transform.position;
            RaycastHit2D groundHit =
                Physics2D.CircleCast(playerPos + groundChkOffset, groundChkRadius, Vector2.up, groundChkRadius, groundLayer);
            if (groundHit.collider == null)
            {
                _character.MovementStateTransition(onJumpType);
                return;
            }


            //이동 체크
            if (rBody.linearVelocityX != 0)
                _character.MovementStateTransition(onMoveType);

            Debug.Log("OnGround");
        }

        public override void Exit()
        {
            
        }
    }
}