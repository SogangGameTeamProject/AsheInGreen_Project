using Unity.Netcode;
using UnityEngine;

public class ObjAutoNetworkSpawn : NetworkBehaviour
{
    private NetworkObject networkObject;

    private void Awake()
    {
        // NetworkObject 컴포넌트 가져오기
        networkObject = GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("NetworkObject 컴포넌트가 없습니다. 이 스크립트는 NetworkObject가 필요합니다.");
        }
    }

    private void Start()
    {
        // 클라이언트에서 오브젝트가 생성된 후 서버에 스폰 요청
        if (IsClient && !IsServer)
        {
            RequestSpawnOnServer();
        }
        else if (IsServer)
        {
            // 서버에서 바로 생성되었을 경우 즉시 스폰
            networkObject.Spawn();
        }
    }

    // 클라이언트에서 서버로 총알 스폰 요청을 보내는 함수
    private void RequestSpawnOnServer()
    {
        // 클라이언트가 서버로 RPC를 통해 스폰 요청
        RequestSpawnServerRpc();
    }

    // 서버에서 총알을 네트워크에 스폰하는 RPC 함수
    [ServerRpc]
    private void RequestSpawnServerRpc()
    {
        if (!networkObject.IsSpawned)
        {
            // 서버에서 총알을 네트워크에 스폰
            networkObject.Spawn();
        }
    }
}
