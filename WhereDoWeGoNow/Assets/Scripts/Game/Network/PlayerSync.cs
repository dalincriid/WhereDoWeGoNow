using UnityEngine;
using System.Collections;

public class PlayerSync : MonoBehaviour
{
	public GameObject WavePrefab;
    public float coolDownDuration = 5.0f;

    private float nextWave;

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
        if (!networkView.isMine)
            return ;
        if (!GetComponentInChildren<Camera>().enabled)
            GetComponentInChildren<Camera>().enabled = true;
		if (Input.GetKeyDown(KeyCode.E) && Time.time > nextWave)
		{
			this.Halo(this.transform.position);
            nextWave = Time.time + coolDownDuration;
		}
	}

	[RPC]
	private void Halo(Vector3 pos)
	{
		if (!networkView.isMine)
		{
			var wave = GameObject.Instantiate(this.WavePrefab, pos, Quaternion.identity);
		}
		else
		{
			networkView.RPC("Halo", RPCMode.OthersBuffered, pos);
		}
	}
}
