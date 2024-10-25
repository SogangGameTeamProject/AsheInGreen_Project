using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public class PlayerDeathState : DeathState
    {
        public override void Enter(CharacterController character)
        {
            base.Enter(character);

            Debug.Log("GameplayManager"+GameplayManager.Instance);
            GameplayManager.OnPlayerDefeated?.Invoke(NetworkManager.LocalClientId);
        }

        
    }
}
