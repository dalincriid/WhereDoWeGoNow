using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour
{
    private const string typeName = "WhereDoWeGoNow";
    private const string gameName = "RoomName";

    private GameManager m_gameManager;
    private HostData[] hostList;

    private bool m_isRefreshing = false;
    private bool m_isStarting = false;

    private Action m_onJoinServer;

    private void DoJoinServer()
    {
        foreach (HostData data in hostList)
        {
            if (data.gameName == gameName)
            {
                Network.Connect(data);
                break;
            }
        }
    }

    public void StartServer()
    {
        if (m_isStarting)
            return;
        m_isStarting = true;
        //MasterServer.ipAddress = "10.1.13.17";
        //MasterServer.port = 23466;
        //Network.natFacilitatorIP = "10.1.13.17";
        //Network.natFacilitatorPort = 50005;

        Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }
    public void RefreshHostList()
    {
        if (!m_isRefreshing)
        {
            m_isRefreshing = true;
            MasterServer.RequestHostList(typeName);
        }
    }
    public void JoinServer(Action action)
    {
        RefreshHostList();
        m_onJoinServer = action;
    }

    #region MonoBehaviour Implementation
    protected void Awake()
    {
        var go = GameObject.FindGameObjectWithTag("GameManager");
        m_gameManager = go.GetComponent<GameManager>();
    }
    protected void Update()
    {
        if (m_isRefreshing && MasterServer.PollHostList().Length > 0)
        {
            m_isRefreshing = false;
            hostList = MasterServer.PollHostList();
            DoJoinServer();
        }
    }
    protected void OnServerInitialized()
    {
    }
    protected void OnConnectedToServer()
    {
        m_onJoinServer.Invoke();
    }
    protected void OnPlayerConnected(NetworkPlayer player)
    {
        m_gameManager.AddPlayer(player);
    }
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            hostList = MasterServer.PollHostList();
        }
    }
    #endregion
}
