using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public class PlayerDeathState : DeathState
    {
        public float deathEventTimer = 0.5f;
        public override void Enter(CharacterController character)
        {
            base.Enter(character);

            Invoke("DeathEvent", deathEventTimer);
        }

        private void DeathEvent()
        {
            GameplayManager.OnPlayerDefeated?.Invoke(NetworkManager.LocalClientId);
        }
    }
}
