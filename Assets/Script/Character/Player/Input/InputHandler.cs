using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Character.Player
{
    public class InputHandler : NetworkBehaviour
    {
        private CharacterController _player = null;//플레이어 컨트롤러

        //커맨드들
        public PlayerCommandInit _moveCommand = null;
        public PlayerCommandInit _jumpCommand = null;
        public PlayerCommandInit _downJumpCommand = null;
        public PlayerCommandInit _mainSkillCommand = null;
        public PlayerCommandInit _secondarySkillCommand = null;
        public PlayerCommandInit _specialSkillCommand = null;

        private MovementStateType _runningMovementType;//현재 진행중이 플레이어의 이동 상태
        private CombatStateType _runningCombatType;//

        //이동
        private Vector2 _moveVec = Vector2.zero;

        private void Start()
        {
            //로컬 객체가 아니면 인풋 시스템 제거
            if (!IsOwner)
            {
                PlayerInput playerInput = GetComponent<PlayerInput>();
                Destroy(playerInput);
            }
                
            _player = GetComponent<CharacterController>();//플레이어 컨트롤러 초기화
        }


        public void Update()
        {
            _runningMovementType = _player._movementController.runningMovementStateType;
            //이동 입력예외 처리 후 커맨드 호출
            if (_runningMovementType != MovementStateType.Unable)
            {
                _moveCommand.Execute(_player, _moveVec);
            }
        }

        //------입력 처리 부분------
        //이동 입력 처리
        public void OnMove(InputAction.CallbackContext context)
        {
            
            _moveVec = context.ReadValue<Vector2>();
        }

        //점프 입력 처리
        public void OnJump(InputAction.CallbackContext context)
        {
            //예외처리
            if (context.started &&
                _runningMovementType != MovementStateType.Unable &&
                _runningCombatType == CombatStateType.Idle &&
                (_player._movementController.isGrounded || (!_player._movementController.isGrounded && _player.JumMaxNum > _player.jumCnt))
                )
            {

                _jumpCommand.Execute(_player);
            }
        }

        //아래 점프 입력 처리
        public void OnDownJump(InputAction.CallbackContext context)
        {
            Debug.Log("아래점프 입력");
            //예외처리
            if (context.started &&
                _runningCombatType == CombatStateType.Idle &&
                (_runningMovementType == MovementStateType.Idle || _runningMovementType == MovementStateType.Move)&&
                _player._movementController.isPlatformed
                )
                _downJumpCommand.Execute(_player);
        }

        //메인 스킬 입력 처리
        public void OnMainSkill(InputAction.CallbackContext context)
        {
            if (context.started)
                _mainSkillCommand.Execute(_player);
        }

        //보조스킬 입력 처리
        public void OnSecondarySkill(InputAction.CallbackContext context)
        {
            if (context.started)
                _secondarySkillCommand.Execute(_player);
        }

        //특수스킬 입력 처리
        public void OnSpecialSkill(InputAction.CallbackContext context)
        {
            if (context.started)
                _specialSkillCommand.Execute(_player);
        }
    }
}

