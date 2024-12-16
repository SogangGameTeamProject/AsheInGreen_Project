using AshGreen.Character.Player;
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
            if (IsOwner)
            {
                //스킬 캔슬
                ((PlayerController)_character)._characterSkillManager.AllStop();
                _character.SetDamageimmunity(true);
            }
            Invoke("DeathEvent", deathEventTimer);
        }

        private void DeathEvent()
        {
            GameplayManager.OnPlayerDefeated?.Invoke(NetworkManager.LocalClientId);
        }
    }
}
