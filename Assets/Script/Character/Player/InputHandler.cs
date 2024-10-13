    using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using AshGreen.Character.Skill;
namespace AshGreen.Character.Player
{
    public class InputHandler : NetworkBehaviour
    {
        private PlayerController _player = null;//플레이어 컨트롤러

        private MovementStateType _runningMovementType;//현재 진행중이 플레이어의 이동 상태
        private CombatStateType _runningCombatType;//

        //이동
        private Vector2 _moveVec = Vector2.zero;


        private void Start()
        {
            _player = GetComponent<PlayerController>();//플레이어 컨트롤러 초기화

            //로컬 객체가 아니면 인풋 시스템 제거
            if (!IsOwner)
            {
                PlayerInput playerInput = GetComponent<PlayerInput>();
                Destroy(playerInput);
            }
        }


        public void Update()
        {
            _runningMovementType = _player._movementController.runningMovementStateType;
            _runningCombatType = _player.runningCombatStateType;
            //이동 입력예외 처리 후 커맨드 호출
            if (!_player._movementController.isUnableMove
                && _moveVec != Vector2.zero &&
                _runningCombatType != CombatStateType.Death)
            {
                _player._movementController.ExecuteMove(_moveVec, _player.MoveSpeed);
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
                !_player._movementController.isUnableMove &&
                _runningCombatType != CombatStateType.Death &&
                (_player._movementController.isGrounded || (!_player._movementController.isGrounded && _player.JumMaxNum > _player.jumCnt))
                )
            {
                _player._movementController.ExecutJump(_player.JumpPower);
            }
        }

        //아래 점프 입력 처리
        public void OnDownJump(InputAction.CallbackContext context)
        {
            //예외처리
            if (context.started &&
                _runningCombatType != CombatStateType.Death &&
                (_runningMovementType == MovementStateType.Idle || _runningMovementType == MovementStateType.Move)&&
                _player._movementController.isPlatformed
                )
                _player._movementController.ExecutDownJump(_player.JumpPower);
        }

        //메인 스킬 입력 처리
        public void OnMainSkill(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _player._characterSkillManager.PresseSkillRpc(0);
            }
            else if (context.canceled)
            {
                _player._characterSkillManager.ReleaseSkillRpc(0);
            }
        }

        //보조스킬 입력 처리
        public void OnSecondarySkill(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _player._characterSkillManager.PresseSkillRpc(1);
            }
            else if (context.canceled)
            {
                _player._characterSkillManager.ReleaseSkillRpc(1);
            }
        }

        //특수스킬 입력 처리
        public void OnSpecialSkill(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _player._characterSkillManager.PresseSkillRpc(2);
            }
            else if (context.canceled)
            {
                _player._characterSkillManager.ReleaseSkillRpc(2);
            }
        }
    }
}

