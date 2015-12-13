using UnityEngine;
using System.Collections;

public class BadController : MonoBehaviour {
	
	public float speed = 6;
	public float maxSpeed = 8;
	
	private Rigidbody rb;
	private string targetTag = "Good";
	public bool alive = true;

	public int col=-1;

	public GameObject target = null;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		if (this.gameObject.name.Contains ("Guy"))
			col = 0;
		else if (this.gameObject.name.Contains ("Axe"))
			col = 1;
		else if (this.gameObject.name.Contains ("Spear"))
			col = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive)
			return;
		if (target == null)
			target = FindRandomEnemy ();
		if (target == null)
			return;
		Vector3 diff = target.transform.position - transform.position;
		diff.y = 0;
		if (rb.velocity.magnitude > maxSpeed)
			rb.velocity = rb.velocity.normalized * maxSpeed;
		transform.rotation = Quaternion.LookRotation(rb.velocity);
		transform.Rotate (0, 90, 0);
		if(col==0)
			transform.Rotate (0, 180, 0);
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
	
	GameObject FindRandomEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(targetTag);
		if (gos.Length == 0)
			return null;
		return gos[Random.Range(0,gos.Length)];
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Good") {
		}
	}
	
}
