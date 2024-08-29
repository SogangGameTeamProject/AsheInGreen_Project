using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AshGreen.Player
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerController _player = null;//플레이어 컨트롤러

        //커맨드들
        public PlayerCommandInit _moveCommand = null;
        public PlayerCommandInit _jumpCommand = null;
        public PlayerCommandInit _downJumpCommand = null;
        public PlayerCommandInit _mainSkillCommand = null;
        public PlayerCommandInit _secondarySkillCommand = null;
        public PlayerCommandInit _specialSkillCommand = null;

        private void Start()
        {
            _player = GetComponent<PlayerController>();//플레이어 컨트롤러 초기화
        }

        //------입력 처리 부분------
        //이동 입력 처리
        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("이동");
            _moveCommand.Execute(_player);
        }

        //점프 입력 처리
        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log("점프");
            _jumpCommand.Execute(_player);
        }

        //아래 점프 입력 처리
        public void OnDownJump()
        {
            Debug.Log("아래 점프");
            _downJumpCommand.Execute(_player);
        }

        //메인 스킬 입력 처리
        public void OnMainSkill(InputAction.CallbackContext context)
        {
            Debug.Log("메인 스킬");
            _mainSkillCommand.Execute(_player);
        }

        //보조스킬 입력 처리
        public void OnSecondarySkill(InputAction.CallbackContext context)
        {
            Debug.Log("보조 스킬");
            _secondarySkillCommand.Execute(_player);
        }

        //특수스킬 입력 처리
        public void OnSpecialSkill(InputAction.CallbackContext context)
        {
            Debug.Log("특수 스킬");
            _specialSkillCommand.Execute(_player);
        }
    }
}

