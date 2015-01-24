using UnityEngine;
using System.Collections;

public class SpikesTrap : MonoBehaviour
{
	public float Interval = 2.0f;

	private float interval;
	private Transform[] childrenTransforms;

	// Use this for initialization
	void Start()
	{
		this.childrenTransforms = this.GetComponentsInChildren<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		this.interval += Time.deltaTime;

		if (this.interval >= this.Interval)
		{
			foreach (var item in this.childrenTransforms)
			{
				item.position = new Vector3(item.position.x, -item.position.y, item.position.z);
			}
			this.interval = 0.0f;
		}
	}
}
