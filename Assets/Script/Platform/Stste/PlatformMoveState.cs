using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformMoveState : PlatformStateInit
    {
        [SerializeField]
        protected Vector2 moveVec = Vector2.zero;
        [SerializeField]
        protected float moveSpeed = 10f;
        private Rigidbody2D rbody;
        public override void Enter(PlatformController context)
        {
            base.Enter(context);
            if(rbody == null)
                rbody = _controller.GetComponent<Rigidbody2D>();
        }

        public override void StateUpdate()
        {
            if (IsServer)
            {
                rbody.linearVelocity = moveVec*moveSpeed;
            }
        }

        public override void Exit()
        {
            
        }
    }
}

