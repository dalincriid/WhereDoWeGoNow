using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Server data
    private Dictionary<NetworkPlayer, bool> m_players;
    #endregion

    public float TimeToEraseMessage = 10.0f;
    public GameObject PlayerPrefab;

    private GameObject m_createOrJoin;
    private GameObject m_startGame;

    private float m_timeBeforeErase = .0f;
    private string m_message = "";

    private bool m_started = false;
    private bool m_firstPlayer = true;

    private Labyrinth m_labyrinth;
    private NetworkManager m_networkManager;
    private GameObject m_player;

    protected void Awake()
    {
        m_players = new Dictionary<NetworkPlayer, bool>();

        m_createOrJoin = GameObject.FindGameObjectWithTag("CreateOrJoinHUD");
        m_startGame = GameObject.FindGameObjectWithTag("StartGameHUD");

        var go = GameObject.FindGameObjectWithTag("NetworkManager");
        m_networkManager = go.GetComponent<NetworkManager>();

        m_labyrinth = GetComponent<Labyrinth>();

        m_startGame.SetActive(false);
    }
    protected void Update()
    {
        m_timeBeforeErase += Time.deltaTime;
        if (m_timeBeforeErase >= TimeToEraseMessage)
        {
            m_message = "";
        }
    }
    protected void OnGUI()
    {
        if (m_timeBeforeErase < TimeToEraseMessage && m_message != "")
        {
            GUI.Label(new Rect(0, Screen.height - 25, Screen.width - 100, 25), "You hear wispering: \"" + m_message + "\"");
        }
    }

    #region Server methods
    public void AddPlayer(NetworkPlayer player)
    {
        m_players.Add(player, false);
    }
    #endregion

    #region NetworkCallbacks
    void OnServerJoined()
    {
        m_startGame.SetActive(true);
    }
    #endregion

    #region HUD Callbacks
    public void OnButtonClicked(string buttonName)
    {
        if (buttonName == "CreateGameButton")
        {
            m_networkManager.StartServer();
            m_createOrJoin.SetActive(false);
        }
        else if (buttonName == "JoinGameButton")
        {
            m_createOrJoin.SetActive(false);
            m_networkManager.JoinServer(OnServerJoined);
        }
        else if (buttonName == "StartGameButton")
        {
            LaunchGame(Network.player);
        }
    }
    #endregion

    #region Network Callbacks
    [RPC]
    public void PostMessage(string message)
    {
        m_message = message;
        m_timeBeforeErase = .0f;
        if (networkView.isMine)
            networkView.RPC("PostMessage", RPCMode.OthersBuffered, message);
    }

    [RPC]
    private void LaunchGame(NetworkPlayer player)
    {
        if (Network.isServer)
        {
            m_players[player] = true;
            foreach (var keyPlayer in m_players.Keys)
            {
                if (!m_players[keyPlayer])
                    return;
            }
            int seed = UnityEngine.Random.Range(0, 100);
            networkView.RPC("SpawnLabyrinth", RPCMode.OthersBuffered, seed);

            m_labyrinth.Compute(seed);
            var spawnPoints = m_labyrinth.SitePlayer(Network.connections.Length);
            for (int i = 0; i < Network.connections.Length; ++i)
            {
                networkView.RPC("SpawnPlayer", Network.connections[i], spawnPoints[i]);
            }
        }
        else if (Network.isClient && !m_started)
        {
            m_started = true;
            networkView.RPC("LaunchGame", RPCMode.Server, player);
        }
    }
    [RPC]
    private void SpawnLabyrinth(int seed)
    {
        if (Network.isClient)
            m_labyrinth.Compute(seed);
    }
    [RPC]
    private void SpawnPlayer(Vector3 spawnPoint)
    {
        if (Network.isClient)
        {
            m_player = Network.Instantiate(PlayerPrefab, spawnPoint, Quaternion.identity, 0) as GameObject;

            Camera[] cameras = Resources.FindObjectsOfTypeAll<Camera>();
            foreach (var cam in cameras)
                cam.enabled = false;

            Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
            foreach (var light in lights)
                light.enabled = false;

            AudioListener[] listeners = Resources.FindObjectsOfTypeAll<AudioListener>();
            foreach (var listener in listeners)
                listener.enabled = false;
        }
    }
    #endregion
}
