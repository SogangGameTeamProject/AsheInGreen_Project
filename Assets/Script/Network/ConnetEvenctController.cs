using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace AshGreen.Network
{
    public class ConnetEvenctController: NetworkBehaviour
    {
        public void ConnectionToSceneLoad(string SceneName)
        {
            NetworkManager.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);

        }
    }

}
