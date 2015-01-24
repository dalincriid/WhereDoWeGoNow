using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject ToSpawn;
    public UIPopupList PopupGameList;

    private GameObject m_createOrJoin;
    private GameObject m_createGame;
    private GameObject m_joinGame;

    private NetworkManager m_networkManager;

    private UIInput m_roomName;

    protected void Awake()
    {
        m_createOrJoin = GameObject.FindGameObjectWithTag("CreateOrJoinHUD");
        m_createGame = GameObject.FindGameObjectWithTag("CreateGameHUD");
        m_joinGame = GameObject.FindGameObjectWithTag("JoinGameHUD");

        var go = GameObject.FindGameObjectWithTag("RoomNameHUD");
        m_roomName = go.GetComponent<UIInput>();

        go = GameObject.FindGameObjectWithTag("NetworkManager");
        m_networkManager = go.GetComponent<NetworkManager>();
        m_networkManager.RefreshHostList();

        go = GameObject.FindGameObjectWithTag("GameListHUD");
        PopupGameList = go.GetComponent<UIPopupList>();

        m_createGame.SetActive(false);
        m_joinGame.SetActive(false);
    }

    #region HUD Callbacks
    public void OnButtonClicked(string buttonName)
    {
        if (buttonName == "CreateGameButton")
        {
            m_createOrJoin.SetActive(false);
            m_createGame.SetActive(true);

            m_networkManager.StartServer();
        }
        else if (buttonName == "JoinGameButton")
        {
            m_createOrJoin.SetActive(false);
            m_joinGame.SetActive(true);

            m_networkManager.RefreshHostList();
            while (!m_networkManager.JoinServer()) { }
        }
    }
    #endregion

    #region Network Callbacks
    [RPC]
    public void LaunchGame()
    {
        GameObject.Instantiate(ToSpawn);

        if (networkView.isMine)
            networkView.RPC("LaunchGame", RPCMode.OthersBuffered);
    }
    #endregion
}
