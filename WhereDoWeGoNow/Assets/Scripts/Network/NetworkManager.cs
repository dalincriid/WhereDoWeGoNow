using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string typeName = "WhereDoWeGoNow";
    private const string gameName = "RoomName";

    private HostData[] hostList;

    public void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    #region MonoBehaviour Implementation
    protected void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
    }

    protected void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    protected void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
    

    protected void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }
    #endregion
}
