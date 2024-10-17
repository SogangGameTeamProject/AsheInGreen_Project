using AshGreen.Character;
using AshGreen.Character.Skill;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public class PlayerController : CharacterController
    {
        [HideInInspector]
        public PlayerUI playerUI;//플레이어 UI

        public MovementController _movementController = null;
        public CharacterSkillManager _characterSkillManager = null;
        public Transform firePoint = null;
    }
}
