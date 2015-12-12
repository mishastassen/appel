using UnityEngine;
using System.Collections;

public class BadController : MonoBehaviour {
	
	public float speed = 5;
	
	private Rigidbody rb;
	private string targetTag = "Good";
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject target = FindClosestEnemy ();
		if (target == null)
			return;
		Vector3 diff = target.transform.position - transform.position;
		diff.y = 0;
		rb.AddForce (speed*diff);
		
	}
	
	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(targetTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Good") {
			Debug.Log ("Got hit");
			rb.mass=0.1f;
			rb.constraints = RigidbodyConstraints.None;
			rb.AddForce (100*Vector3.up);
		}
	}
	
}
