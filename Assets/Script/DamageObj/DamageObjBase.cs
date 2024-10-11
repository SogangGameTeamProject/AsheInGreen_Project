using UnityEngine;
using AshGreen.Character;
using Unity.Netcode;
namespace AshGreen.DamageObj
{
    public class DamageObjBase : NetworkBehaviour
    {
        public int damage = 1;
        public bool isCritical = false;
        public bool isNockback = false;
        public float nockbackPower = 100f;
        public float nockbackTime = 0.3f;
        public bool isDestroy = false;

        protected void DestoryObj()
        {
            if (!NetworkObject.IsSpawned)
                return;

            NetworkObject.Despawn();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsClient)
                return;
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // 만약 인터페이스가 존재하면 실행
            if (damageable != null)
            {
                Debug.Log("데미지 오브젝트 데미지 부여");
                //넉백 여부에 따른 넉백
                if (isNockback)
                {
                    MovementController movementController = collision.gameObject.GetComponent<MovementController>();
                    if (movementController)
                    {
                        float nockBackForceX =
                        collision.gameObject.transform.position.x > this.transform.position.x ?
                        1 : -1;
                        Vector2 nockBackForce = new Vector2(nockBackForceX, 1);

                        movementController.ExcutNockBack(nockBackForce, nockbackPower, nockbackTime);
                    }
                }

                damageable.TakeDamage(damage);

                if (isDestroy)
                    DestoryObj();
            }

        }
    }

}
