using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float hspeed = 50.0F;
	public float vspeed = 50.0F;
	private float zoomspeed = 3.0F;

	private float hstart = 0.0F;
	private float vstart = 0.0F; 

	private float zoom = 0.0F;

	private float shake = 0.0f; //.6f; 

	// Use this for initialization
	void Start () {

	}
    int cc = 0;
    Vector3 altprev = new Vector3(0, 0, 0); 
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)) {
			hstart -= hspeed * Input.GetAxis ("Mouse Y");
			vstart += vspeed * Input.GetAxis ("Mouse X");
		}

		transform.eulerAngles = new Vector3 (hstart, vstart, 0.0F);

		//zoom

		zoom += Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * zoom * zoomspeed;

        cc++;
        var alt = shake * (new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        if (cc % 2 == 0) {
            transform.position += -altprev + alt;
            altprev = alt;
        }
        //transform.position += new Vector3 (0.0F, 0.0F, zoom*zoomspeed);

         shake *= .99f;

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
