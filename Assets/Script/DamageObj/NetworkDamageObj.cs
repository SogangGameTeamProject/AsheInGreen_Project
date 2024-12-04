using UnityEngine;
using AshGreen.Character;
using Unity.Netcode;
using CharacterController = AshGreen.Character.CharacterController;
using UnityEngine.WSA;

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

        //추가타 오브젝트
        [Header("추가타 관련")]
        [SerializeField]
        private GameObject addMoreObj = null;
        [SerializeField]
        private float addMoreDamage = 0.3f;
        [SerializeField]
        private float addMoreLifeTime = 0.3f;

        //폭발 피해 관련
        [Header("폭발 관련")]
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
            if (isFire && isTarget)
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

                if(addMoreObj)
                {
                    //총알 발사
                    float damage = addMoreDamage;//데미지 설정

                    Vector2 fireDir = Vector2.zero;//발사 방향 조정
                    ProjectileFactory.Instance.RequestProjectileFire(caster, addMoreObj, AttackType.MainSkill, damage,
                    fireDir, target.transform.position, Quaternion.identity, addMoreLifeTime);
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

        // 
        [Rpc(SendTo.ClientsAndHost)]
        public void FireRpc(NetworkObjectReference caster, AttackType dealType, float damage,
            Vector2 bulletPos, bool isTarget = false)
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
                    this.targetPos = bulletPos;
                else
                    this.GetComponent<Rigidbody2D>().linearVelocity = bulletPos;
            }
        }
    }
}