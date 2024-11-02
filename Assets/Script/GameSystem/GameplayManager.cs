using AshGreen.Character;
using AshGreen.Obsever;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using AshGreen.Character.Player;
using Unity.VisualScripting;

public class GameplayManager : NetworkSingleton<GameplayManager>
{
    public static Action<ulong> OnPlayerDefeated;

    [SerializeField]
    private CharacterConfig[] m_charactersData;

    [SerializeField]
    private GameObject playerHUDPanel;

    [SerializeField]
    private GameObject playerUIPre;

    [SerializeField]
    private GameObject m_deathUI;

    [SerializeField]
    private GameObject m_clearUI;

    [SerializeField]
    private Transform[] m_StartingPositions;

    private int m_numberOfPlayerConnected;
    [SerializeField]
    private List<ulong> m_connectedClients = new List<ulong>();
    public List<PlayerController> m_player = new List<PlayerController>();


    //승리 패배 관련 전역 변수
    [SerializeField]
    private GameObject playerDefeatPopup;
    [SerializeField]
    private GameObject bossDefeatPopup;


    private void OnEnable()
    {
        OnPlayerDefeated += PlayerDeath;

        if (!IsServer)
            return;

        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnDisable()
    {
        OnPlayerDefeated -= PlayerDeath;

        if (!IsServer)
            return;

        // Since the NetworkManager could potentially be destroyed before this component, only
        // remove the subscriptions if that singleton still exists.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    public void PlayerDeath(ulong clientId)
    {
        Debug.Log("PlayerDeath");
        m_numberOfPlayerConnected--;

        if (m_numberOfPlayerConnected <= 0)
        {
            ActivateDeathUIClientRpc();
        }
    }

    // Event to check when a player disconnects
    private void OnClientDisconnect(ulong clientId)
    {
        foreach (var player in m_player)
        {
            if (player != null)
            {
                if (player.characterConfig.GetClientId(clientId) == clientId)
                {
                    player._damageReceiver.TakeDamage(999); // Do critical damage
                }
            }
        }
    }

    [ClientRpc]
    private void ActivateDeathUIClientRpc()
    {
        Debug.Log("사망 팝업");
        m_deathUI.SetActive(true);
    }

    [ClientRpc]
    private void ActivateClearUIClientRpc()
    {
        m_clearUI.SetActive(true);
    }

    [ClientRpc]
    private void LoadClientRpc()
    {
        if (IsServer)
            return;

        LoadingFadeEffect.Instance.FadeAll();
    }

    [ClientRpc]
    private void SetPlayerUIClientRpc(NetworkObjectReference playerObj)
    {
        //플레이어 UI 설정
        NetworkObject playerNetworkObj = null;
        playerObj.TryGet(out playerNetworkObj);
        PlayerController playerController = playerNetworkObj.GetComponent<PlayerController>();
        GameObject playerUI = Instantiate(playerUIPre, playerHUDPanel.transform);
        playerController.playerUI = playerUI.GetComponent<PlayerUI>();
        PlayerUI hud = playerController.playerUI;

        hud.player = playerController;

        int playerId = playerController.characterConfig.GetPlayerId(playerController.clientID);
        if (playerHUDPanel.transform.childCount == 1)
            hud.playerHud.p1.SetActive(true);
        else
            hud.playerHud.p2.SetActive(true);

        //스킬 아이콘 초기화
        CharacterConfig config = playerController.characterConfig;
        hud.playerHud.mainSkillIcon.sprite = config.skills[0].skillIcon;
        hud.playerHud.secondarySkillIcon.sprite = config.skills[1].skillIcon;
        hud.playerHud.specialSkillIcon.sprite = config.skills[2].skillIcon;
        hud.playerHud.playerIcon.sprite = config.iconSprite;
    }
    private IEnumerator HostShutdown()
    {
        // Tell the clients to shutdown
        ShutdownClientRpc();

        // Wait some time for the message to get to clients
        yield return new WaitForSeconds(0.5f);

        // Shutdown server/host
        Shutdown();
    }

    private void Shutdown()
    {
        NetworkManager.Singleton.Shutdown();
        LoadingSceneManager.Instance.LoadScene(SceneName.Menu, false);
    }

    [ClientRpc]
    private void ShutdownClientRpc()
    {
        if (IsServer)
            return;

        Shutdown();
    }

    public void BossDefeat()
    {
        ActivateClearUIClientRpc();
    }

    public void ExitToMenu()
    {
        if (IsServer)
        {
            StartCoroutine(HostShutdown());
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
            LoadingSceneManager.Instance.LoadScene(SceneName.Menu, false);
        }
    }

    // So this method is called on the server each time a player enters the scene.
    // Because of that, if we create the ship when a player connects we could have a sync error
    // with the other clients because maybe the scene on the client is no yet loaded.
    // To fix this problem we wait until all clients call this method then we create the ships
    // for every client connected 
    public void ServerSceneInit(ulong clientId)
    {
        // Save the clients 
        m_connectedClients.Add(clientId);
        
        // Check if is the last client
        if (m_connectedClients.Count < NetworkManager.Singleton.ConnectedClients.Count)
            return;
        // For each client spawn and set UI
        foreach (var client in m_connectedClients)
        {
            Debug.Log("client: " + client + ", m_connectedClients: " + m_connectedClients.Count);
            int index = 0;
            foreach (CharacterConfig data in m_charactersData)
            {
                Debug.Log("선택 클라이언트 ID 추력");
                foreach (var val in data.selectClientIds)
                {
                    Debug.Log(val.Key + ", " + val.Value);
                }
                Debug.Log("data.GetClientId(clientId): " + data.GetClientId(client) + " client: " + client);
                if (data.GetClientId(client) == client)
                {
                    Debug.Log("캐릭터 찾음: " + client);
                    GameObject player =
                        NetworkObjectSpawner.SpawnNewNetworkObjectAsPlayerObject(
                            data.playerPre,
                            m_StartingPositions[m_numberOfPlayerConnected].position,
                            client,
                            true);

                    PlayerController playerController =
                        player.GetComponent<PlayerController>();
                    playerController.gameplayManager = this;
                    m_player.Add(playerController);
                    SetPlayerUIClientRpc(playerController.GetComponent<NetworkObject>());

                    m_numberOfPlayerConnected++;
                    break;
                }

                index++;
            }
        }
    }
}