using AshGreen.Character.Player;
using AshGreen.Platform;
using System.Collections;
using UnityEngine;

namespace AshGreen.Character
{
    public class StopState : CharacterStateBase
    {
        
        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            //캐릭터 멈춤 설정
            if (IsOwner)
            {
                _character.SetDamageimmunity(true);
                PlayerController player = _character as PlayerController;
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
                player._movementController.isUnableMove = true;
            }

            //플랫폼 멈춤 설정
            if (IsServer)
            {
                foreach (var platform in PlatformManager.Instance.platformList)
                {
                    platform.StateTransitionRpc(PlatformStateType.IDLE);
                }
            }
        }

        public override void StateUpdate()
        {
            
        }

        public override void Exit()
        {
            //캐릭터 멈춤 해제
            if (IsOwner)
            {
                _character.SetDamageimmunity(false);
                PlayerController player = _character as PlayerController;
                player.GetComponent<Rigidbody2D>().gravityScale = player.Gravity;
                player._movementController.isUnableMove = false;
            }
        }
    }
}