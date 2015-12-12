using UnityEngine;
using System.Collections;

public class GoodController : MonoBehaviour {

	public float speed = 5;

	private Rigidbody rb;
	private string targetTag = "Bad";

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
		if (other.tag == "Bad") {
			Debug.Log ("Hit");
		}
	}

}
