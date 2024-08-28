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
        public ICommand _moveCommand;

        private void Start()
        {
            _player = GetComponent<PlayerController>();//플레이어 컨트롤러 초기화
        }

        //------입력 처리 부분------
        //이동 입력 처리
        public void OnMove(InputAction.CallbackContext context)
        {
            
        }

        //점프 입력 처리
        public void OnJump(InputAction.CallbackContext context)
        {

        }

        //메인 스킬 입력 처리
        public void OnMainSkill(InputAction.CallbackContext context)
        {

        }

        //보조스킬 입력 처리
        public void OnSecondarySkill(InputAction.CallbackContext context)
        {

        }

        //특수스킬 입력 처리
        public void OnSpecialSkill(InputAction.CallbackContext context)
        {

        }
    }
}

