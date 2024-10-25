using UnityEngine;
using Unity.Netcode;
namespace AshGreen.UI
{
    public class BtnController : MonoBehaviour
    {
        public void OnShutdown()
        {
            ClientConnection.Instance.RemoveClient(NetworkManager.Singleton.LocalClientId);
        }
    } 
}