using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Server data
    private Dictionary<NetworkPlayer, bool> m_players;
    private bool m_creatingPlayers = false;
    private bool m_gameStarted = false;
    private PlayerSync[] m_playerObjects;
    #endregion

    public float TimeToEraseMessage = 10.0f;
    public GameObject PlayerPrefab;

    private GameObject m_createOrJoin;
    private GameObject m_startGame;

    private float m_timeBeforeErase = .0f;
    private string m_message = "";

    private bool m_started = false;
    private bool m_firstPlayer = true;

    private int m_currentPlayer = 0;

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
        if (Network.isServer && m_creatingPlayers)
        {
            if (!m_gameStarted)
            {
                m_playerObjects = GameObject.FindObjectsOfType<PlayerSync>();
                if (m_playerObjects.Length == m_players.Count)
                {
                    networkView.RPC("DisableOtherPlayers", RPCMode.OthersBuffered);

                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Camera>().enabled = true;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<AudioListener>().enabled = true;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Light>().enabled = true;
                    m_gameStarted = true;
                }
            }
            else
            {
                if (Input.GetAxis("Fire1") != 0)
                {
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Camera>().enabled = false;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<AudioListener>().enabled = false;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Light>().enabled = false;

                    m_currentPlayer = ++m_currentPlayer % m_playerObjects.Length;

                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Camera>().enabled = true;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<AudioListener>().enabled = true;
                    m_playerObjects[m_currentPlayer].GetComponentInChildren<Light>().enabled = true;
                }
            }
        }
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
    public void SendMessage(string message)
    {
        PostMessage(message);
        networkView.RPC("PostMessage", RPCMode.OthersBuffered, message);
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
    private void DisableOtherPlayers()
    {
        if (Network.isClient)
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var cam in cameras)
                cam.enabled = false;

            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (var light in lights)
                light.enabled = false;

            AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
            foreach (var listener in listeners)
                listener.enabled = false;

            m_player.tag = "Player";
            m_player.GetComponentInChildren<AudioListener>().enabled = true;
            m_player.GetComponentInChildren<Camera>().enabled = true;
            m_player.GetComponentInChildren<Light>().enabled = true;
            m_player.GetComponent<Countdown>().enabled = true;
        }
    }
    [RPC]
    private void PostMessage(string message)
    {
        m_message = message;
        m_timeBeforeErase = .0f;
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
            m_creatingPlayers = true;
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

            var countdowns = GameObject.FindObjectsOfType<Countdown>();
			foreach (var item in countdowns)
			{
				item.enabled = false;
			}
        }
    }
    #endregion
}
