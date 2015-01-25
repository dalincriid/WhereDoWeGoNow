using UnityEngine;
using System.Collections;

public class PlayerSync : MonoBehaviour
{
	public GameObject WavePrefab;
    public int MessageCount = 5;
    public int CryCount = 5;
    public float coolDownDuration = 5.0f;

    private int m_remainingMessages;
    private bool m_typingMessage = false;
    private string m_message = "";
    private float nextWave;

    private GameManager m_gameManager;

    protected void Awake()
    {
        var go = GameObject.FindGameObjectWithTag("GameManager");
        m_gameManager = go.GetComponent<GameManager>();
    }

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
        if (Input.GetKeyDown(KeyCode.E) && Time.time > nextWave && !m_typingMessage && CryCount > 0)
		{
            CryCount--;
			this.Halo(this.transform.position);
            nextWave = Time.time + coolDownDuration;
		}
        else if (Input.GetKeyDown(KeyCode.Y) && !m_typingMessage)
        {
            m_typingMessage = true;
        }
	}

    protected void OnGUI()
    {
        if (m_typingMessage)
        {
            GUI.SetNextControlName("ChatBox");

            Event e = Event.current;

            if (e.keyCode == KeyCode.Return)
            {
                if (m_message != "" && 0 < MessageCount)
                {
                    m_gameManager.SendMessage(m_message);
                    m_message = "";
                    MessageCount--;
                }
                m_typingMessage = false;
            }
            else
            {
                GUI.FocusControl("ChatBox");
                m_message = GUI.TextField(new Rect(0, 0, Screen.width - 100, 25), m_message, 35);
            }
        }
    }

	[RPC]
	private void Halo(Vector3 pos)
	{
		var wave = GameObject.Instantiate(this.WavePrefab, pos, Quaternion.identity) as GameObject;
		if (networkView.isMine)
		{
			wave.renderer.enabled = false;
			networkView.RPC("Halo", RPCMode.OthersBuffered, pos);
		}
	}
}
