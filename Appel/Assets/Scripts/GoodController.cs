using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class GoodController : MonoBehaviour {
	//test:
	public AudioClip[] soundarray;
	public AudioClip testsound;
	private int randomClip;
	AudioSource audio;

	public float speed = 6, maxSpeed=8;

	private Rigidbody rb;
	private string targetTag = "Bad";
	public int row=-1;
	public bool alive = true;

	private GameManager GM;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		if (this.gameObject.name.Contains ("Guy"))
			row = 0;
		else if (this.gameObject.name.Contains ("Axe"))
			row = 1;
		else if (this.gameObject.name.Contains ("Spear"))
			row = 2;

		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive)
			return;
		GameObject target = FindClosestEnemy ();
		if (target == null)
			return;
		Vector3 diff = target.transform.position - transform.position;
		diff.y = 0;
		if (rb.velocity.magnitude > maxSpeed)
			rb.velocity = rb.velocity.normalized * maxSpeed;
		transform.rotation = Quaternion.LookRotation (rb.velocity);
		transform.Rotate (0, 90, 0);
		if (row == 0)
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
		
	void OnTriggerEnter(Collider other) {


		if (other.tag == "Bad") {
			if (!alive || !other.GetComponent<BadController>().alive)
				return;

			int col=-1;
			if (other.gameObject.name.Contains ("Guy"))
				col = 0;
			else if (other.gameObject.name.Contains ("Axe"))
				col = 1;
			else if (other.gameObject.name.Contains ("Spear"))
				col = 2;

			int value = GM.mat.mat[row,col];
			Debug.Log ("row: "+row+", col: "+col+", value: "+value); // other.GetComponent<BadController>().col
			GameObject winner = other.gameObject;
			GameObject looser = this.gameObject;
			if(Random.Range(-5,6)<value) {
				winner = this.gameObject;
				looser = other.gameObject;
			}

			Rigidbody rb2 = looser.GetComponent<Rigidbody>();
			rb2.constraints = RigidbodyConstraints.None;
			rb2.AddForce (200*Vector3.up);
			rb2.AddTorque(new Vector3(Random.Range(25,40),Random.Range(25,40),Random.Range(25,40)));
			if(this.gameObject==looser) {
				alive=false;
				GM.numKilledGood[row]++;

				randomClip = Random.Range (0, soundarray.Length);
				audio.PlayOneShot (soundarray[randomClip], 1.0F);
				Debug.Log ("randomClip value: " + randomClip);
				// ??? Debug.Log("Array index: " + Array.IndexOf(soundarray, randomClip));
			}
			else {
				looser.GetComponent<BadController>().alive = false;
				GM.numKilledBad[col]++;
			}

			Destroy(looser.gameObject,1);

		}
		/*
		public playRandomSound(){
			if (audio.isPlaying) return;
			int soundtoplay = floor(Random(4));
			switch(soundtoplay){
			case 1: Audio.PlayOneShot("hueh1"); break;
			case 2: Audio.PlayOneShot("hueh2"); break;
			default: Audio.PlayOneShot("hueh3"); break; 
			*/
	}
		
}
