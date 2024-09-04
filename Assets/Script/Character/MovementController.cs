using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace AshGreen.Character
{
    public class MovementController : NetworkBehaviour
    {
        private CharacterController _character;
        private Rigidbody2D rBody = null;

        private void Start()
        {
            _character = GetComponent<CharacterController>();
            rBody = GetComponent<Rigidbody2D>();
        }

        //이동 구현
        public void OnMove(Vector2 moveVec)
        {
            if (rBody)
            {
                rBody.linearVelocityX = moveVec.x * _character.MoveSpeed;
            }
        }


    }
}