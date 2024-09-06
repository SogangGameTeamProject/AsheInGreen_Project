using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class JumpState : CharacterStateBase
    {
        public MovementStateType onChangeType = MovementStateType.Idle;//점프 종료 시 전환할 상태타입
        //바닥 체크를 위한 설정값
        public LayerMask groundLayer;
        public Vector2 groundChkOffset = Vector2.zero;
        public float groundChkRadius = 0.15f;


        private Rigidbody2D rBody = null;
        public override void Enter(CharacterController character)
        {
            base.Enter(character);

            Debug.Log("점프 상태");
            if (rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            if(rBody.linearVelocityY <= 0)
            {
                //레이케스트로 바닥 위인지 체크 맞으면 OnAir상태로 전환
                Vector2 playerPos = _character.transform.position;
                RaycastHit2D groundHit =
                    Physics2D.CircleCast(playerPos + groundChkOffset, groundChkRadius, Vector2.up, groundChkRadius, groundLayer);
                if (groundHit.collider != null)
                    _character.MovementStateTransition(onChangeType);
            }
        }

        public override void Exit()
        {
            
        }
    }
}