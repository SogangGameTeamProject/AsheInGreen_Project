using UnityEngine;
using AshGreen.Character;
using Unity.Netcode;
using CharacterController = AshGreen.Character.CharacterController;

namespace AshGreen.DamageObj
{
    public class NetworkDamageObj : NetworkBehaviour
    {
        public CharacterController caster = null;
        public float damage = 1;
        public AttackType dealType = AttackType.None;
        public bool isCritical = false;
        public bool isNockback = false;
        public float nockbackPower = 100f;
        public float nockbackTime = 0.3f;
        public bool isDestroy = false;
        //타겟 추적 관련
        public bool isTarget = false;
        public Vector2 targetPos = Vector2.zero;
        public float trackingSpeed = 100f;

        //폭발 피해 관련
        public bool isExplosion = false;
        public float explosionRadius = 4;
        public LayerMask targetLayer;

        private void Update()
        {
            //타겟 추적 상태일 시 타겟을 향해 날라감
            if (isTarget)
            {
                transform.position = Vector2.Lerp(transform.position, targetPos, trackingSpeed * Time.deltaTime);

                float distance = Vector2.Distance(transform.position, targetPos);
                if (distance <= 0.1f)
                    DestoryObjRpc();
            }
        }
        [Rpc(SendTo.Server)]
        protected void DestoryObjRpc()
        {
            if (!NetworkObject.IsSpawned)
                return;

            //폭발 여부에 따른 추가 피해
            // 폭발 중심점에서 explosionRadius 반경 내의 콜라이더 탐색
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

            foreach (Collider2D target in targets)
            {
                IDamageable damageable = target.gameObject.GetComponent<IDamageable>();

                // 만약 인터페이스가 존재하면 실행
                if (damageable != null)
                {
                    if (!target.GetComponent<NetworkObject>().IsOwner)
                        return;

                    //넉백 여부에 따른 넉백
                    if (isNockback)
                    {
                        MovementController movementController = target.gameObject.GetComponent<MovementController>();
                        if (movementController)
                        {
                            float nockBackForceX =
                            target.gameObject.transform.position.x > this.transform.position.x ?
                            1 : -1;
                            Vector2 nockBackForce = new Vector2(0, 1);

                            movementController.ExcutNockBack(nockBackForce, nockbackPower, nockbackTime);
                        }
                    }

                    if (caster == null || dealType == AttackType.Enemy)
                        damageable.TakeDamage(damage);
                    else
                        caster.GetComponent<DamageReceiver>().DealDamage(target.GetComponent<CharacterController>(), damage, dealType);

                }
            }

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

                if (caster == null || dealType == AttackType.Enemy)
                    damageable.TakeDamage(damage);
                else
                    caster.GetComponent<DamageReceiver>().DealDamage(collision.GetComponent<CharacterController>(), damage, dealType);

                if (isDestroy)
                    DestoryObjRpc();
            }
        }
    }

}
