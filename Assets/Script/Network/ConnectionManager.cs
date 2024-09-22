using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private string ipAddress = "127.0.0.1"; // 기본 IP 주소
    private bool isServerStarted = false;
    private bool isClientConnected = false;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        // 호스트 시작 버튼
        if (!isServerStarted && GUILayout.Button("Start Host"))
        {
            NetworkManager.Singleton.StartHost();
            isServerStarted = true;
            Debug.Log("Server started");
        }

        // 클라이언트 연결 버튼
        if (!isClientConnected && GUILayout.Button("Connect as Client"))
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, 7777);
            NetworkManager.Singleton.StartClient();
            isClientConnected = true;
            Debug.Log("Connecting to server at " + ipAddress);
        }

        // IP 주소 입력 필드
        GUILayout.Label("IP Address:");
        ipAddress = GUILayout.TextField(ipAddress, 15);

        GUILayout.EndArea();
    }
}