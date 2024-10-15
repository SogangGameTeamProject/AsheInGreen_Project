using AshGreen.DamageObj;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.WSA;

namespace AshGreen.Character{
    public class CharacterProjectileFactory : NetworkBehaviour
    {
        public List<GameObject> projectileObjts = new List<GameObject>();//사용할 투사체 프리펩 리스트

        /// <summary>
        /// 서버에서 투사체 발사 요청 메서드
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index">사용할 프리펩 인덱스</param>
        /// <param name="attackType"></param>
        /// <param name="damage"></param>
        /// <param name="fireDir"></param>
        /// <param name="firePos"></param>
        /// <param name="fireRotation"></param>
        /// <param name="destroyTime"></param>
        [ServerRpc]
        public void RequestProjectileFireServerRpc
            (NetworkObjectReference owner, int index, AttackType attackType, float damage, Vector2 fireDir,
            Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            if (projectileObjts != null)
                ProjectileFire(owner, index, attackType, damage, fireDir, firePos, fireRotation, destroyTime);
        }

        /// <summary>
        /// 투사체 발사 메서드
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="attackType"></param>
        /// <param name="damage"></param>
        /// <param name="fireDir"></param>
        /// <param name="firePos"></param>
        /// <param name="fireRotation"></param>
        /// <param name="destroyTime"></param>
        private void ProjectileFire
            (NetworkObjectReference owner, int index, AttackType attackType, float damage, Vector2 fireDir,
            Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            GameObject bullet = Instantiate(projectileObjts[index], firePos, fireRotation);

            bullet.GetComponent<NetworkObject>().Spawn();

            // 시간 경과 후 총알 파괴
            if (destroyTime > 0)
                Destroy(bullet, destroyTime);

            //투사체 설정
            DamageObjBase damageObj = bullet.GetComponent<DamageObjBase>();

            NetworkObject ownerObj = null;
            owner.TryGet(out ownerObj);
            CharacterController ownerController = ownerObj.GetComponent<CharacterController>();
            damageObj.caster = ownerController;
            damageObj.dealType = attackType;
            damageObj.damage = damage;

            // 총알의 물리적 움직임 처리
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = fireDir; // 발사 방향 설정
        }
    }
}