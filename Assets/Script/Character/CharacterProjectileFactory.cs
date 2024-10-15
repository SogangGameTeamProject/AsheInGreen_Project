using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character{
    public class CharacterProjectileFactory : NetworkBehaviour
    {
        public NetworkVariable<float> projectilePre = new NetworkVariable<float>(0);

        /// <summary>
        /// 서버에서 투사체 발사 요청 메서드
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="attackType"></param>
        /// <param name="damage"></param>
        /// <param name="fireDir"></param>
        /// <param name="firePos"></param>
        /// <param name="fireRotation"></param>
        /// <param name="destroyTime"></param>
        [ServerRpc]
        public void RequestProjectileFireServerRpc
            (NetworkObjectReference owner, AttackType attackType, float damage, Vector2 fireDir, Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {
            if (projectilePre != null)
                ProjectileFire(owner, attackType, damage, fireDir, firePos, fireRotation, destroyTime);
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
            (NetworkObjectReference owner, AttackType attackType, float damage, Vector2 fireDir, Vector3 firePos, Quaternion fireRotation, float destroyTime = 0)
        {

        }
    }
}