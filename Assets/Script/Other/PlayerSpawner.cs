using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        // 현재 클라이언트의 플레이어 NetworkObject를 찾기
        NetworkObject localPlayer = GetLocalPlayer();
        if (localPlayer != null)
        {
            localPlayer.transform.position = this.transform.position;
            Debug.Log("찾음");
        }
    }

    // 로컬 플레이어를 찾는 함수
    public NetworkObject GetLocalPlayer()
    {
        // 현재 클라이언트 ID를 얻음
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        // 모든 네트워크 객체 중 OwnerClientId가 현재 클라이언트 ID와 일치하는 객체 찾기
        foreach (var networkObject in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
        {
            if (networkObject.OwnerClientId == localClientId)
            {
                return networkObject; // 로컬 플레이어 객체 반환
            }
        }

        return null; // 찾지 못했을 경우 null 반환
    }
}
