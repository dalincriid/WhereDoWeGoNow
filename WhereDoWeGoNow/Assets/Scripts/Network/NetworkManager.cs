using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string typeName = "WhereDoWeGoNow";
    private const string gameName = "RoomName";

    private GameManager m_gameManager;
    private HostData[] hostList;

    public void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());

        MasterServer.RegisterHost(typeName, gameName);
    }
    public void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }
    public bool JoinServer()
    {
        foreach (HostData data in hostList)
        {
            if (data.gameName == gameName)
            {
                Network.Connect(data);
                return true;
            }
        }
        return false;
    }

    #region MonoBehaviour Implementation
    protected void Awake()
    {
        var go = GameObject.FindGameObjectWithTag("GameManager");
        if (go)
        {
            m_gameManager = go.GetComponent<GameManager>();
        }
    }
    protected void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            hostList = MasterServer.PollHostList();
        }
    }
    protected void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
    #endregion
}
