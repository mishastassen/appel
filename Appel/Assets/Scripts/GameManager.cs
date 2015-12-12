using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	List<GameObject> badMammals = new List<GameObject>();
	List<GameObject> badFishes = new List<GameObject>();
	List<GameObject> badBirds = new List<GameObject>();

	public GameObject badMammal;
	public GameObject badFish;
	public GameObject badBird;

	List<GameObject> goodMammals = new List<GameObject>();
	List<GameObject> goodFishes = new List<GameObject>();
	List<GameObject> goodBirds = new List<GameObject>();
	
	public GameObject goodMammal;
	public GameObject goodFish;
	public GameObject goodBird;

	public Transform spawnGood, spawnBad;

	private float timeLastSpawn=0;
	

	void CreateWave(int number, List<GameObject> animals, GameObject animal, Vector3 positionFirst, Vector3 gap, Quaternion rotation, Vector3 speed, float randomRange) {
		for (int i=0; i<number; i++) {
			Vector3 random = new Vector3(Random.Range (-randomRange, randomRange),Random.Range (-randomRange, randomRange),0);
			GameObject x = Instantiate (animal, positionFirst + i*gap, rotation) as GameObject;
			x.GetComponent<Rigidbody>().velocity = speed;
			animals.Add(x);
		}
	}

	// Use this for initialization
	void Start () {
		/*
		int numBadMammals = 10;
		int numBadFishes = 10;
		int numBadBirds = 10;
		Vector3 gap = new Vector3 (0,0,2);
		Quaternion rotation = Quaternion.identity;
		Vector3 speed = new Vector3 (1,0,0);
		CreateWave (numBadMammals, badMammals, badMammal, new Vector3 (-50, 1f, 0), gap, rotation, 5*speed, 1);
		CreateWave (numBadFishes, badFishes, badFish, new Vector3 (-30, 0, -10), 0.5f*gap, rotation, 2.5f * speed, 1);
		CreateWave (numBadBirds, badBirds, badBird, new Vector3 (-100, 8, -5), 1.5f*gap, rotation, 8 * speed, 1);

		int numGoodMammals = 10;
		int numGoodFishes = 10;
		int numGoodBirds = 10;
		gap = new Vector3 (1,0,0);
		rotation = Quaternion.identity;
		speed = new Vector3 (-1,0,0);
		CreateWave (numGoodMammals, goodMammals, goodMammal, new Vector3 (0, 1f, 0), gap, rotation, 5*speed, 4);
		CreateWave (numGoodFishes, goodFishes, goodFish, new Vector3 (5, 0, -10), 0.5f*gap, rotation, 2.5f * speed, 4);
		CreateWave (numGoodBirds, goodBirds, goodBird, new Vector3 (0, 8, -5), 1.5f*gap, rotation, 8 * speed, 4);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeLastSpawn > 1) {
			timeLastSpawn = Time.time;
			Instantiate (goodMammal,spawnGood.position,Quaternion.identity);
			Instantiate (badMammal,spawnBad.position,Quaternion.identity);
		}
	}

}
