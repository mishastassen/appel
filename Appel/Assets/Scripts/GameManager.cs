using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	private int numUnits=3;

	public List<GameObject> offenseUnit = new List<GameObject>();
	public List<GameObject> defenseUnit = new List<GameObject>();

	public Transform spawnGood, spawnBad;

	private float timeLastSpawn=0;
	private int[] numSpawnedGood;
	private int[] numSpawnedBad;
	public int[] numKilledGood;
	public int[] numKilledBad;

	public int[] NumAliveGood() {
		int[] res = new int[numUnits];
		for (int i=0; i<numUnits; i++)
			res [i] = numSpawnedGood [i] - numKilledGood [i];
		return res;
	}

	public int[] NumAliveBad() {
		int[] res = new int[numUnits];
		for (int i=0; i<numUnits; i++)
			res [i] = numSpawnedBad [i] - numKilledBad [i];
		return res;
	}

	public int TotalKillsGood() {
		return numKilledBad.Sum ();
	}

	public int TotalKillsBad() {
		return numKilledGood.Sum ();
	}

	private string ArrayToString(int[] arr) {
		string res="[";
		for(int i=0;i<arr.Length;i++)
			res+=arr[i]+(i+1<arr.Length?", ":"]");
		return res;
	}
	
	// Use this for initialization
	void Start () {
		numSpawnedGood = new int[numUnits];
		numSpawnedBad = new int[numUnits];
		numKilledGood = new int[numUnits];
		numKilledBad = new int[numUnits];
	}
	
	// Update is called once per frame
	void Update () {
		int[] offense = UIManager.offense;
		if (Time.time - timeLastSpawn > 0.2) {
			timeLastSpawn = Time.time;
			Vector3 offset = new Vector3(0,0,Random.Range(-15,15));
			for(int i=0;i<offense.Length;i++) {
				if(100*Random.value<offense[i]) {
					Instantiate(offenseUnit[i],spawnGood.position+offset,Quaternion.identity);
					numSpawnedGood[i]++;
				}
			}

			int badIndex=Random.Range (0,numUnits);
			Instantiate (defenseUnit[badIndex],spawnBad.position+offset,Quaternion.identity);
			numSpawnedBad[badIndex]++;
		}
		Debug.Log ("Score: "+TotalKillsGood()+" - "+TotalKillsBad());
		Debug.Log ("Total spawned good: " + numSpawnedGood.Sum () + ", total spawned bad: " + numSpawnedBad.Sum ());
		Debug.Log ("alive good: "+NumAliveGood ().Sum()+": " + ArrayToString (NumAliveGood ()) + ", alive bad: "+NumAliveBad().Sum()+": " + ArrayToString (NumAliveBad ()));
	}

}
