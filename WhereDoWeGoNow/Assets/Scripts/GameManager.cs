using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private GameObject m_createOrJoin;
    private GameObject m_startGame;

    private Labyrinth m_labyrinth;
    private NetworkManager m_networkManager;

    protected void Awake()
    {
        m_createOrJoin = GameObject.FindGameObjectWithTag("CreateOrJoinHUD");
        m_startGame = GameObject.FindGameObjectWithTag("StartGameHUD");

        var go = GameObject.FindGameObjectWithTag("NetworkManager");
        m_networkManager = go.GetComponent<NetworkManager>();
        m_networkManager.RefreshHostList();

        m_labyrinth = GetComponent<Labyrinth>();

        m_startGame.SetActive(false);
    }

    #region HUD Callbacks
    public void OnButtonClicked(string buttonName)
    {
        if (buttonName == "CreateGameButton")
        {
            m_createOrJoin.SetActive(false);
            m_startGame.SetActive(true);

            m_networkManager.StartServer();
        }
        else if (buttonName == "JoinGameButton")
        {
            m_createOrJoin.SetActive(false);

            m_networkManager.RefreshHostList();
            while (!m_networkManager.JoinServer()) { }
        }
        else if (buttonName == "StartGameButton")
        {
            LaunchGame();
        }
    }
    #endregion

    #region Network Callbacks
    [RPC]
    private void LaunchGame()
    {
        m_labyrinth.GenerateLabyrinth(0);

        if (networkView.isMine)
            networkView.RPC("LaunchGame", RPCMode.OthersBuffered);
    }
    #endregion
}
