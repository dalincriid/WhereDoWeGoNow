using UnityEngine;
using System.Collections;

public class SpikesTrap : MonoBehaviour
{
	public float Interval = 2.0f;
    public float MalusTime = 5.0f;

	private float interval;

	protected void Update()
	{
		this.interval += Time.deltaTime;

		if (this.interval >= this.Interval)
		{
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
			this.interval = 0.0f;
		}
	}

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerSync>().TouchedByTrap(MalusTime);
            Network.Destroy(gameObject);
        }
    }
}
