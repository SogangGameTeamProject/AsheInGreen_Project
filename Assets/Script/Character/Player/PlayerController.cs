using AshGreen.Character;
using AshGreen.Character.Skill;
using UnityEngine;

namespace AshGreen.Character.Player
{
    public class PlayerController : CharacterController
    {
        public MovementController _movementController = null;
        public CharacterSkillManager _characterSkillManager = null;
    }
}
