using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformHitState : PlatformStateInit
    {
        private float hitTime = 0.3f;
        private float currentTime = 0f;
        public override void Enter(PlatformController context)
        {
            base.Enter(context);
            currentTime = 0;
        }

        public override void StateUpdate()
        {
            if (!IsServer) return;

            currentTime += Time.deltaTime;
            if (currentTime >= hitTime)
            {
                _controller.StateTransitionRpc(PlatformStateType.IDLE);
            }
        }

        public override void Exit()
        {
            
        }
    }
}

