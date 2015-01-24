using UnityEngine;
using System.Collections;

public class DecreasingAlpha : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float alpha = this.renderer.material.color.a;
        alpha = Mathf.Lerp(alpha, 0, Time.deltaTime);
        this.renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
	}
}
