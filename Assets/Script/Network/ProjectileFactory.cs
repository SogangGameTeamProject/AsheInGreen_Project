using AshGreen.DamageObj;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character{
    public class ProjectileFactory : NetworkSingleton<ProjectileFactory>
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
        public void RequestProjectileFire
            (CharacterController owner, GameObject pre, AttackType attackType, float damage, Vector2 fireDir,
            Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            int index = projectileObjts.IndexOf(pre);
            NetworkObject networkOwer = owner.GetComponent<NetworkObject>();
            if (projectileObjts != null)
                ProjectileFireRpc(networkOwer, index, attackType, damage, fireDir, firePos, fireRotation, destroyTime);
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
        [Rpc(SendTo.Server)]
        private void ProjectileFireRpc
            (NetworkObjectReference owner, int index, AttackType attackType, float damage, Vector2 fireDir,
            Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            GameObject bullet = Instantiate(projectileObjts[index], firePos, fireRotation);

            NetworkObject ownerObj = null;
            owner.TryGet(out ownerObj);

            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(ownerObj.OwnerClientId);
            //투사체 설정
            NetworkDamageObj damageObj = bullet.GetComponent<NetworkDamageObj>();
            damageObj.FireRpc(owner, attackType, damage, fireDir);

            // 시간 경과 후 총알 파괴
            if (destroyTime > 0)
                NetworkObject.Destroy(bullet, destroyTime);
        }

        /// <summary>
        /// 서버에서 투사체 조준 발사 요청 메서드
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index">사용할 프리펩 인덱스</param>
        /// <param name="attackType"></param>
        /// <param name="damage"></param>
        /// <param name="targetPos">타겟 위치</param>
        /// <param name="firePos"></param>
        /// <param name="fireRotation"></param>
        /// <param name="destroyTime"></param>
        public void RequestProjectileTargetFire
            (CharacterController owner, GameObject pre, AttackType attackType, float damage, Vector2 targetPos,
            Vector3 firePos, Quaternion fireRotation)
        {
            int index = projectileObjts.IndexOf(pre);

            NetworkObject networkOwer = owner.GetComponent<NetworkObject>();
            if (projectileObjts != null)
                ProjectileTargetFireRpc(networkOwer, index, attackType, damage, targetPos, firePos, fireRotation);
        }

        /// <summary>
        /// 투사체 발사 메서드
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="attackType"></param>
        /// <param name="damage"></param>
        /// <param name="targetPos">타겟 위치</param>
        /// <param name="firePos"></param>
        /// <param name="fireRotation"></param>
        /// <param name="destroyTime"></param>
        [Rpc(SendTo.Server)]
        private void ProjectileTargetFireRpc
            (NetworkObjectReference owner, int index, AttackType attackType, float damage, Vector2 targetPos,
            Vector3 firePos, Quaternion fireRotation)
        {
            GameObject bullet = Instantiate(projectileObjts[index], firePos, fireRotation);

            NetworkObject ownerObj = null;
            owner.TryGet(out ownerObj);

            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(ownerObj.OwnerClientId);

            //투사체 설정
            NetworkDamageObj damageObj = bullet.GetComponent<NetworkDamageObj>();
            damageObj.FireRpc(owner, attackType, damage, targetPos, true);
        }

        
        public void RequestWaringTargetFire
            (GameObject pre, Vector2 fireDir, Vector3 firePos, float destroyTime = 0)
        {
            int index = projectileObjts.IndexOf(pre);

            if (projectileObjts != null)
                ProjectileWaringFireRpc(index, fireDir, firePos, destroyTime);
        }

        
        [Rpc(SendTo.Server)]
        private void ProjectileWaringFireRpc
            (int index, Vector2 fireDir, Vector3 firePos, float destroyTime = 0)
        {
            GameObject bullet = Instantiate(projectileObjts[index], firePos, Quaternion.identity);

            bullet.GetComponent<NetworkObject>().Spawn();

            if (destroyTime > 0)
                NetworkObject.Destroy(bullet, destroyTime);

            // 총알의 물리적 움직임 처리
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = fireDir; // 발사 방향 설정
        }

        public void RequestObjectSpawn(GameObject pre, Vector3 spawnPoint, float destroyTime = 0, Transform parent = null)
        {
            int index = projectileObjts.IndexOf(pre);
            if(parent != null)
                ObjectSpawnInParentServerRpc(index, spawnPoint, parent.GetComponent<NetworkObject>(), destroyTime);
            else
                ObjectSpawnServerRpc(index, spawnPoint, destroyTime);
        }

        [ServerRpc]
        public void ObjectSpawnServerRpc(int preIndex, Vector3 spawnPoint, float destroyTime = 0)
        {
            GameObject obj = Instantiate(projectileObjts[preIndex], spawnPoint, Quaternion.identity);

            obj.GetComponent<NetworkObject>().Spawn();// 시간 경과 후 총알 파괴

            if (destroyTime > 0)
                NetworkObject.Destroy(obj, destroyTime);
        }

        [ServerRpc]
        public void ObjectSpawnInParentServerRpc(int preIndex, Vector3 spawnPoint, NetworkObjectReference parent
            ,float destroyTime = 0)
        {
            NetworkObject parentNetObj;
            Transform parentT = null;
            if (parent.TryGet(out parentNetObj))
                parentT = parentNetObj.GetComponent<Transform>();
            GameObject obj = Instantiate(projectileObjts[preIndex], spawnPoint, Quaternion.identity
                , parentT);
            obj.GetComponent<NetworkObject>().Spawn();// 시간 경과 후 총알 파괴

            // 모든 클라이언트에서 부모 설정을 다시 수행하도록 ClientRpc 호출
            if (parentT != null)
            {
                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                SetParentServerRpc(netObj.NetworkObjectId, parentNetObj.NetworkObjectId);
            }

            if (destroyTime > 0)
                NetworkObject.Destroy(obj, destroyTime);
        }

        [ServerRpc]
        private void SetParentServerRpc(ulong objectID, ulong parentID)
        {
            // 네트워크 오브젝트와 부모 오브젝트 찾기
            NetworkObject netObj = NetworkManager.SpawnManager.SpawnedObjects[objectID];
            NetworkObject parentObj = NetworkManager.SpawnManager.SpawnedObjects[parentID];

            // 부모 설정
            if (netObj != null && parentObj != null)
            {
                netObj.transform.SetParent(parentObj.transform);
            }
        }
    }
}