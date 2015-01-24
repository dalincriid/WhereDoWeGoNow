using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject ToSpawn;

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
