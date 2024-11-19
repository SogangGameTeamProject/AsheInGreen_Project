using UnityEngine;
using Unity.Netcode;

namespace AshGreen.Character
{
    public class BossDeathState : DeathState
    {
        public float deathEventTimer = 0.5f;
        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            Invoke("DeathEvent", deathEventTimer);
        }

        private void DeathEvent()
        {
            if(IsServer)
                NetworkObject.Despawn(this.gameObject);
            GameplayManager.Instance.BossDefeat();
        }
    }

}
