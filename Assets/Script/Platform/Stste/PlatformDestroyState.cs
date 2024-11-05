using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformDestroyState : PlatformStateInit
    {
        [SerializeField]
        private float destroyDelayTime = 0.5f;
        private float currentTime = 0;
        
        public override void Enter(PlatformController context)
        {
            base.Enter(context);
            currentTime = 0;
        }

        public override void StateUpdate()
        {
            if (!IsServer) return;
            currentTime += Time.deltaTime;
            if (currentTime > destroyDelayTime)
                NetworkObject.Despawn(this.gameObject);
        }

        public override void Exit()
        {
            
        }
    }
}

