using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using System;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using System.Collections;


namespace AshGreen.Network
{
    public class NetcodeManager : MonoBehaviour
    {
        public Button hostButton;  // 호스트 버튼
        public Button clientButton; // 클라이언트 버튼
        public InputField joinCodeInput; // 클라이언트가 입력할 Join 코드

        public string gameSceneName = "GameScene"; // 전환될 게임 씬 이름

        async void Start()
        {
            // Unity Services 초기화 및 로그인
            await InitializeUnityServicesAsync();

            // 버튼에 클릭 이벤트 연결
            hostButton.onClick.AddListener(() => StartCoroutine(StartHostAfterInitialization()));
            clientButton.onClick.AddListener(StartClient);
        }

        private async Task InitializeUnityServicesAsync()
        {
            try
            {
                // Unity Services 초기화
                await UnityServices.InitializeAsync();

                // 익명 사용자로 로그인
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log("Signed in anonymously.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
            }
        }

        private IEnumerator StartHostAfterInitialization()
        {
            yield return new WaitForEndOfFrame();  // 프레임이 끝날 때까지 대기하여 초기화 문제 방지

            // 실제로 StartHost를 호출하는 부분
            StartHost();
        }

        public async void StartHost()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(10);

                if (allocation == null)
                {
                    Debug.LogError("Allocation is null.");
                    return;
                }

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                if (NetworkManager.Singleton == null)
                {
                    Debug.LogError("NetworkManager is not initialized or is missing.");
                    return;
                }

                var relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartHost();
                Debug.Log("Host started with Relay. Join Code: " + joinCode);

                SceneManager.LoadScene(gameSceneName);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to start host: {e.Message}");
            }
        }

        public async void StartClient()
        {
            string joinCode = joinCodeInput.text;

            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                var relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();
                Debug.Log("Client joined with Relay using join code: " + joinCode);

                SceneManager.LoadScene(gameSceneName);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join host: {e.Message}");
            }
        }
    }
}
