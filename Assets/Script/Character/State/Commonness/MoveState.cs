using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character
{
    public class MoveState : CharacterStateBase
    {
        public CharacterStateType onChangeType = CharacterStateType.Idle;//이동 종료 시 전환할 상태타입
        private Rigidbody2D rBody = null;
        private Vector2 moveVec = Vector2.zero;

        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if(rBody == null)
                rBody = _character.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {

            if (moveVec == Vector2.zero)
            {
                _character.StateTransition(onChangeType);
                return;
            }
                
            if (rBody)
            {
                rBody.linearVelocityX = moveVec.x * _character.MoveSpeed;
                Debug.Log("이동"+ _character.MoveSpeed+ ", " + moveVec.x);
                Debug.Log(rBody.linearVelocityX);

            }
                
            
        }

        public override void Exit()
        {
            
        }

        //이동 입력 처리
        public void OnMove(InputAction.CallbackContext context)
        {
            moveVec = context.ReadValue<Vector2>().normalized;
        }
    }
}