using UnityEngine;

namespace AshGreen.Character
{
    public class BossDeathState : DeathState
    {
        public override void Enter(CharacterController character)
        {
            base.Enter(character);


            GameplayManager.Instance.BossDefeat();
        }
    }

}
