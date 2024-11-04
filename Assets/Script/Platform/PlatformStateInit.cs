using UnityEngine;
using Unity.Netcode;
using AshGreen.State;
using UnityEngine.TextCore.Text;

namespace AshGreen.Platform
{
    public class PlatformStateInit : MonoBehaviour, IState<PlatformController>
    {
        protected PlatformController _controller = null;
        protected Animator _animator = null;

        public void Enter(PlatformController context)
        {

            //플레이어 초기화
            if (_controller == null)
                _controller = context;
            //애니메이터 초기화
            if (_animator == null)
                _animator = _controller.GetComponent<Animator>();
        }

        public void Exit()
        {

        }

        public void StateUpdate()
        {

        }


    }
}
