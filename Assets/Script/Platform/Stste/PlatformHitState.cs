using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformHitState : PlatformStateInit
    {
        private float hitTime = 0.3f;
        [SerializeField]
        private string hurtAniParam = "IsHurt";
        private float currentTime = 0f;
        public override void Enter(PlatformController context)
        {
            base.Enter(context);
            context.GetComponent<Animator>().SetTrigger(hurtAniParam);
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

