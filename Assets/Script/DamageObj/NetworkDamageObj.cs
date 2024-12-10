using UnityEngine;
using AshGreen.Character;
using Unity.Netcode;
using CharacterController = AshGreen.Character.CharacterController;

namespace AshGreen.DamageObj
{
    public class NetworkDamageObj : NetworkBehaviour
    {
        [Header("기본설정 관련")]
        [HideInInspector]
        public bool isFire = false;
        [HideInInspector]
        public CharacterController caster = null;
        public AttackType dealType = AttackType.None;
        public float damage = 1;
        [Header("넉백 관련")]
        public bool isKnockback = false;
        public float knockbackPower = 100f;
        public float knockbackTime = 0.3f;
        public bool isDestroy = false;
        //타겟 추적 관련
        [Header("위치 타겟팅 관련")]
        public bool isTarget = false;
        public Vector2 targetPos = Vector2.zero;
        public float trackingSpeed = 100f;
        [SerializeField]
        private bool isParabola = false;//포물선 여부
        [SerializeField]
        private bool isReversParabola = false;//역포물선 여부
        public float height = 5f; // 포물선 높이

        //폭발 피해 관련
        [Header("폭발 관련")]
        public bool isExplosion = false;
        public float explosionRadius = 4;
        public LayerMask targetLayer;

        private Vector2 startPos;
        private float parabolaTime;

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (isExplosion)
                ApplyExplosionDamage();
        }

        private void Update()
        {
            //타겟 추적 상태일 시 타겟을 향해 날라감
            if (isFire && isTarget)
            {
                if (isParabola)
                {
                    TrackParabolaTarget();
                }
                else
                {
                    TrackTarget();
                }
            }
        }

        private void TrackTarget()
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, trackingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
                DestroyObjRpc();
        }

        private void TrackParabolaTarget()
        {
            parabolaTime += Time.deltaTime * trackingSpeed;
            float parabolaHeight =
                Mathf.Sin(Mathf.PI * parabolaTime) * (isReversParabola ? -height : height);
            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, parabolaTime);
            transform.position = new Vector3(currentPos.x, currentPos.y + parabolaHeight, transform.position.z);

            if (parabolaTime >= 1f)
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

        private bool ApplyDamage(Collider2D target)
        {
            if (!isFire) return false;

            IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
            bool isDamageImmunity =
                target.gameObject?.GetComponent<CharacterController>()?.isDamageImmunity?.Value ?? false;

            if (damageable != null && target.GetComponent<NetworkObject>().IsOwner && !isDamageImmunity)
            {
                if (isKnockback)
                    ApplyKnockback(target);

                if (caster == null || dealType == AttackType.Enemy)
                    damageable.TakeDamageRpc(damage);
                else
                {
                    NetworkObject targetNobj = target.GetComponent<NetworkObject>();
                    caster.GetComponent<DamageReceiver>().DealDamageRpc(targetNobj, damage, dealType);
                }
                return true;
            }

            return false;
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
            if (ApplyDamage(collision) && isDestroy)
                DestroyObjRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void FireRpc(NetworkObjectReference caster, AttackType dealType, float damage,
            Vector2 bulletPos, bool isTarget = false, float trackingSpeed = 0)
        {
            this.isFire = true;
            NetworkObject casterObj = null;
            caster.TryGet(out casterObj);
            CharacterController casterController = casterObj.GetComponent<CharacterController>();
            this.caster = casterController.GetComponent<CharacterController>();
            this.dealType = dealType;
            this.damage = damage;

            // 소유자일 경우에만 총알 발사 설정
            if (IsOwner)
            {
                //타겟 추적 여부 설정
                this.isTarget = isTarget;
                if (this.isTarget)
                {
                    this.targetPos = bulletPos;
                    this.trackingSpeed = trackingSpeed > 0 ? trackingSpeed/20 : this.trackingSpeed;
                    this.startPos = transform.position;
                }
                else
                {
                    this.GetComponent<Rigidbody2D>().linearVelocity = bulletPos;
                }
            }
        }
    }
}