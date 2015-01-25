using UnityEngine;
using System.Collections;

public class GlowingSphere : MonoBehaviour
{
	public float maxScale;
	public float beginScale;
	public float growingSpeed;

	private Vector3 v3Scale;
	private AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		transform.localScale = new Vector3(beginScale, 0.5f, beginScale);
		v3Scale = new Vector3(maxScale, 0.5f, maxScale);
		this.renderer.material.color = Color.red;
		this.audioSource = this.GetComponentInChildren<AudioSource>();
		this.audioSource.minDistance = this.maxScale / 2.0f;
		this.audioSource.maxDistance = this.audioSource.minDistance + 0.5f;
		this.audioSource.Play();
	}

	// Update is called once per frame
	void Update()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, v3Scale, Time.deltaTime * growingSpeed);

		Color color = this.renderer.material.color;
		color.a -= maxScale / 2000.0f;
		this.renderer.material.color = color;

		if (transform.localScale.x > maxScale - 0.5f)
		{
			Destroy(gameObject);
		}
        
	}
}
