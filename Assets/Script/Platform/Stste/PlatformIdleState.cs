using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformIdleState : PlatformStateInit
    {
        public override void Enter(PlatformController context)
        {
            base.Enter(context);
            context.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        public override void StateUpdate()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

