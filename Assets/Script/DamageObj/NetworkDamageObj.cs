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
        public bool isKnockback = false;
        public float knockbackPower = 100f;
        public float knockbackTime = 0.3f;
        public bool isDestroy = false;
        //타겟 추적 관련
        public bool isTarget = false;
        public Vector2 targetPos = Vector2.zero;
        public float trackingSpeed = 100f;

        //폭발 피해 관련
        public bool isExplosion = false;
        public float explosionRadius = 4;
        public LayerMask targetLayer;

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (isExplosion)
                ApplyExplosionDamage();
        }

        private void Update()
        {
            //타겟 추적 상태일 시 타겟을 향해 날라감
            if (isTarget)
            {
                TrackTarget();
            }
        }

        private void TrackTarget()
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, trackingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
                DestroyObjRpc();
        }

        [Rpc(SendTo.Server)]
        protected void DestroyObjRpc()
        {
            if (!NetworkObject.IsSpawned)
                return;

            NetworkObject.Destroy(this.gameObject);
        }

        private void ApplyExplosionDamage()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

            foreach (Collider2D target in targets)
            {
                ApplyDamage(target);
            }
        }

        private void ApplyDamage(Collider2D target)
        {
            IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
            bool isDamageImmunity = 
                target.gameObject?.GetComponent<CharacterController>()?.isDamageImmunity?.Value ?? false;
            Debug.Log("isDamageImmunity: " + isDamageImmunity);
            if (damageable != null && target.GetComponent<NetworkObject>().IsOwner && !isDamageImmunity)
            {
                if (isKnockback)
                    ApplyKnockback(target);

                if (caster == null || dealType == AttackType.Enemy)
                    damageable.TakeDamage(damage);
                else
                    caster.GetComponent<DamageReceiver>().DealDamage(target.GetComponent<CharacterController>(), damage, dealType);
            }
        }

        private void ApplyKnockback(Collider2D target)
        {
            MovementController movementController = target.gameObject?.GetComponent<MovementController>();
            if (movementController)
            {
                Vector2 knockBackForce = new Vector2(0, 1);
                movementController.ExcutNockBack(knockBackForce, knockbackPower, knockbackTime);
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null && collision.GetComponent<NetworkObject>().IsOwner)
            {
                if (isKnockback)
                    ApplyKnockback(collision);

                if (caster == null || dealType == AttackType.Enemy)
                    damageable.TakeDamage(damage);
                else
                    caster.GetComponent<DamageReceiver>().DealDamage(collision.GetComponent<CharacterController>(), damage, dealType);

                if (isDestroy)
                    DestroyObjRpc();
            }
        }
    }
}