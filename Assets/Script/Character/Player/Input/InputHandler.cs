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
            _moveCommand.Execute(_player);
        }

        //점프 입력 처리
        public void OnJump(InputAction.CallbackContext context)
        {
            _jumpCommand.Execute(_player);
        }

        //아래 점프 입력 처리
        public void OnMouseDown()
        {
            _downJumpCommand.Execute(_player);
        }

        //메인 스킬 입력 처리
        public void OnMainSkill(InputAction.CallbackContext context)
        {
            _mainSkillCommand.Execute(_player);
        }

        //보조스킬 입력 처리
        public void OnSecondarySkill(InputAction.CallbackContext context)
        {
            _secondarySkillCommand.Execute(_player);
        }

        //특수스킬 입력 처리
        public void OnSpecialSkill(InputAction.CallbackContext context)
        {
            _specialSkillCommand.Execute(_player);
        }
    }
}

