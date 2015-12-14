using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float hspeed = 5.0F;
	public float vspeed = 5.0F;
	public float zoomspeed = 2;

	private float hstart = 0.0F;
	private float vstart = 0.0F; 

	private float zoom = 0.0F;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		hstart -= hspeed * Input.GetAxis ("Mouse Y");
		vstart += vspeed * Input.GetAxis ("Mouse X");

		transform.eulerAngles = new Vector3 (hstart, vstart, 0.0F);

		//zoom
		if (Input.GetKey ("Up"))
			transform.position += (0.0F,0.0F,100);
		if (Input.GetKeyDown ("Down"))
			zoom -= zoomspeed;

		/*
		zoom += Input.GetAxis("Mouse ScrollWheel");
		transform.position += new Vector3 (0.0F, 0.0F, zoom*zoomspeed);

		if (Mathf.Abs(zoom) > 0.1f)
			 *= 0.9f;
		else
			zoom = 0f;
		*/
	}
}
