using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
	public float Speed = 10.0f;

	private Camera myCamera;
	private Rigidbody myRigidBody;

	public float MouseSensitivity = 15.0f;

	private float minimumX = -360F;
	private float maximumX = 360F;

	private float minimumY = -60F;
	private float maximumY = 60F;

	private float rotationY = 0F;

	// Use this for initialization
	void Start()
	{
		Screen.showCursor = false;
		this.myCamera = this.GetComponentInChildren<Camera>();
		this.myRigidBody = this.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		float rotationX = this.myCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.MouseSensitivity;

		this.rotationY += Input.GetAxis("Mouse Y") * this.MouseSensitivity;
		this.rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

		this.myCamera.transform.localEulerAngles = new Vector3(-this.rotationY, rotationX, 0);
	}

	public void FixedUpdate()
	{
		//this.rigidbody.AddForce(Vector3.forward * this.Speed * Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
