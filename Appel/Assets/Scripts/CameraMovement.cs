using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float hspeed = 5.0F;
	public float vspeed = 5.0F;
	public float zoomspeed = 10.0F;

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

		zoom += Input.GetAxis("Mouse ScrollWheel");
		transform.position += transform.forward*zoom*zoomspeed; 
		//transform.position += new Vector3 (0.0F, 0.0F, zoom*zoomspeed);

		if (zoom > 0){
			if (zoom > 0.01f)
				zoom *= 0.90f;
			else
				zoom = 0f;
		}
		else{ // if zoom < 0
			if (zoom < -0.01f)
				zoom *= 0.90f; // divide by?
			else
				zoom = 0f;
		}
	}
}
