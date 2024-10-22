using AshGreen.Character;
using AshGreen.Obsever;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using AshGreen.Character.Player;

public class GameplayManager : NetworkSingleton<GameplayManager>
{
    public static Action<ulong> OnPlayerDefeated;

    [SerializeField]
    private CharacterConfig[] m_charactersData;

    [SerializeField]
    private PlayerUI[] m_playersUI;

    [SerializeField]
    private GameObject m_deathUI;

    [SerializeField]
    private Transform[] m_shipStartingPositions;

    private int m_numberOfPlayerConnected;
    private List<ulong> m_connectedClients = new List<ulong>();
    private List<PlayerController> m_playerShips = new List<PlayerController>();

    private void OnEnable()
    {
        if (!IsServer)
            return;

        OnPlayerDefeated += PlayerDeath;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnDisable()
    {
        if (!IsServer)
            return;

        OnPlayerDefeated -= PlayerDeath;

        // Since the NetworkManager could potentially be destroyed before this component, only
        // remove the subscriptions if that singleton still exists.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    public void PlayerDeath(ulong clientId)
    {
        m_numberOfPlayerConnected--;

        if (m_numberOfPlayerConnected <= 0)
        {
            LoadClientRpc();
            LoadingSceneManager.Instance.LoadScene(SceneName.Defeat);
        }
        else
        {
            // Send a client rpc to check which client was defeated, and activate their death UI
            ActivateDeathUIClientRpc(clientId);
        }
    }

    // Event to check when a player disconnects
    private void OnClientDisconnect(ulong clientId)
    {
        foreach (var player in m_playerShips)
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
    private void ActivateDeathUIClientRpc(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            m_deathUI.SetActive(true);
        }
    }

    [ClientRpc]
    private void LoadClientRpc()
    {
        if (IsServer)
            return;

        LoadingFadeEffect.Instance.FadeAll();
    }

    [ClientRpc]
    private void SetPlayerUIClientRpc(int charIndex, string playerShipName)
    {
        // Not optimal, but this is only called one time per ship
        // We do this because we can not pass a GameObject in an RPC
        GameObject playerSpaceship = GameObject.Find(playerShipName);

        PlayerController playerController =
            playerSpaceship.GetComponent<PlayerController>();

        /*m_playersUI[m_charactersData[charIndex].playerId].SetUI(
            m_charactersData[charIndex].playerId,
            m_charactersData[charIndex].iconSprite,
            m_charactersData[charIndex].iconDeathSprite,
            playerController.nowHp.Value,
            m_charactersData[charIndex].darkColor);*/

        // Pass the UI to the player
        //playerController.playerUI = m_playersUI[m_charactersData[charIndex].playerId];
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
        LoadClientRpc();
        LoadingSceneManager.Instance.LoadScene(SceneName.Victory);
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
            int index = 0;

            foreach (CharacterConfig data in m_charactersData)
            {
                if (data.GetClientId(clientId) == clientId)
                {
                    GameObject playerSpaceship =
                        NetworkObjectSpawner.SpawnNewNetworkObjectAsPlayerObject(
                            data.playerPre,
                            m_shipStartingPositions[m_numberOfPlayerConnected].position,
                            client,
                            true);

                    PlayerController playerController =
                        playerSpaceship.GetComponent<PlayerController>();
                    playerController.characterConfig = data;
                    playerController.gameplayManager = this;

                    m_playerShips.Add(playerController);
                    SetPlayerUIClientRpc(index, playerSpaceship.name);

                    m_numberOfPlayerConnected++;
                }

                index++;
            }
        }
    }
}