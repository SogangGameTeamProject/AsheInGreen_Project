using UnityEngine;
using AshGreen.Character;
namespace AshGreen.DamageObj
{
    public class TestDamageObj : MonoBehaviour
    {
        private int damage = 1;
        public bool isNockback = false;
        public float nockbackPower = 100f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // 만약 인터페이스가 존재하면 실행
            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                //넉백 여부에 따른 넉백
                if (isNockback)
                {
                    MovementController movementController = collision.gameObject.GetComponent<MovementController>();
                    float nockBackForceX =
                        collision.gameObject.transform.position.x > this.transform.position.x ?
                        1 : -1;
                    Vector2 nockBackForce = new Vector2(nockBackForceX, 1);

                    movementController.ExcutNockBack(nockBackForce, nockbackPower);
                }
            }

        }
    }

}
