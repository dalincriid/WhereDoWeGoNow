using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour {

    public float Interval = 2.0f;
    public float MalusTime = 5.0f;

    private float interval;
    private bool m_activated = true;

	protected void Update()
    {
        this.interval += Time.deltaTime;

        if (this.interval >= this.Interval)
        {
            m_activated = m_activated ? false : true;
            foreach (var fire in GetComponentsInChildren<ParticleEmitter>())
                fire.emit = m_activated;
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
            this.interval = 0.0f;
        }
	}

    protected void OnTriggerEnter(Collider other)
    {
        if (m_activated && other.tag == "Player")
        {
            other.GetComponent<PlayerSync>().TouchedByTrap(MalusTime);
            Network.Destroy(gameObject);
        }
    }
}
