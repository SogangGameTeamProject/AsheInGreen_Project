using UnityEngine;
using AshGreen.Character;
using Unity.Netcode;
using CharacterController = AshGreen.Character.CharacterController;

namespace AshGreen.DamageObj
{
    public class DamageObjBase : NetworkBehaviour
    {
        public CharacterController caster = null;
        public float damage = 1;
        public AttackType dealType = AttackType.None;
        public bool isCritical = false;
        public bool isNockback = false;
        public float nockbackPower = 100f;
        public float nockbackTime = 0.3f;
        public bool isDestroy = false;

        protected void DestoryObj()
        {
            if (!NetworkObject.IsSpawned)
                return;
            Debug.Log("IsOwner: "+IsOwner);
            NetworkObject.Destroy(this.gameObject);
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // 만약 인터페이스가 존재하면 실행
            if (damageable != null)
            {
                if (!collision.GetComponent<NetworkObject>().IsOwner)
                    return;

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
                        Vector2 nockBackForce = new Vector2(0, 1);

                        movementController.ExcutNockBack(nockBackForce, nockbackPower, nockbackTime);
                    }
                }

                if (caster == null)
                    damageable.TakeDamage(damage);
                else
                    caster.GetComponent<DamageReceiver>().DealDamage(collision.GetComponent<CharacterController>(), damage, dealType);

                if (isDestroy)
                    DestoryObj();
            }
        }
    }

}
