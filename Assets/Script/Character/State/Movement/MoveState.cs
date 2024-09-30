using AshGreen.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class MoveState : CharacterStateBase
    {
        public MovementStateType onChangeType = MovementStateType.Idle;//이동 종료 시 전환할 상태타입
        private Rigidbody2D rBody = null;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if(rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            if (!IsOwner)
                return;

            //이동 방향에 따른 방향 전환
            if (rBody.linearVelocityX > 0.1f && _character.CharacterDirection == CharacterDirection.Left)
            {
                _character.SetCharacterDirectionRpc(CharacterDirection.Right);
                Debug.Log("오른쪽 방향 전환");
            }
            else if (rBody.linearVelocityX < -0.1f && _character.CharacterDirection == CharacterDirection.Right)
            {
                _character.SetCharacterDirectionRpc(CharacterDirection.Left);
                Debug.Log("왼쪽 방향 전환");
            }

            //이동 상태 종료 체크
            if (rBody.linearVelocityX == 0 || !((PlayerController)_character)._movementController.isGrounded)
            {
                ((PlayerController)_character)._movementController.MovementStateTransitionRpc(onChangeType);
                return;
            }
                
            
        }

        public override void Exit()
        {
            
        }
    }
}