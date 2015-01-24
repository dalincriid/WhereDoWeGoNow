using UnityEngine;
using System.Collections;

public class PlayerSync : MonoBehaviour
{
    protected void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            rigidbody.position = syncPosition;
        }
    }

	protected void Update()
	{
		//Halo(Vector3.zero);
	}

	[RPC]
	private void Halo(Vector3 pos)
	{
		//instantiate sound
		if (!networkView.isMine)
		{
			//instantiate sphere
		}
		else
		{
			networkView.RPC("Halo", RPCMode.OthersBuffered);
		}
		//Network.Instantiate(null, pos, Quaternion.identity, 0);
	}
}
