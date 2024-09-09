using AshGreen.Singleton;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen
{
    public class GameManager : Singleton<GameManager>
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                if (GUILayout.Button("Start Host"))
                {
                    NetworkManager.Singleton.StartHost();
                }

                if (GUILayout.Button("Start Client"))
                {
                    NetworkManager.Singleton.StartClient();
                }
            }

            if (NetworkManager.Singleton.IsServer)
            {
                GUILayout.Label("Server is running...");
            }

            if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                GUILayout.Label("Client is connected...");
            }

            GUILayout.EndArea();
        }
    }

}
