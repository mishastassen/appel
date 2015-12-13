using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List<GameObject> offenseUnit = new List<GameObject>();
	public List<GameObject> defenseUnit = new List<GameObject>();

	public Transform spawnGood, spawnBad;

	private float timeLastSpawn=0;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		int[] offense = UIManager.offense;
		if (Time.time - timeLastSpawn > 0.2) {
			timeLastSpawn = Time.time;
			Vector3 offset = new Vector3(0,0,Random.Range(-15,15));
			for(int i=0;i<offense.Length;i++)
				if(100*Random.value<offense[i])
					Instantiate(offenseUnit[i],spawnGood.position+offset,Quaternion.identity);

			Instantiate (defenseUnit[Random.Range (0,3)],spawnBad.position+offset,Quaternion.identity);
		}
	}

}
