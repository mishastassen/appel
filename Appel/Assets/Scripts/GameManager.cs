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

	public Matrix mat;
	public int[] offense;
	public int[] defense;

	private string spawnsGood = "", spawnsBad="";


	int GetBestUnit(int[] current, int[] desired) {
		int bestIndex = 0;
		float lowestFactor = current [0] / (desired [0]+0.001f);
		for (int i=1; i<numUnits; i++) {
			if(lowestFactor > current [i] / (desired [i]+0.001f)) {
				bestIndex=i;
				lowestFactor = current [i] / (desired [i]+1e-6f);
			}
		}
		return bestIndex;
	}

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

		mat = new Matrix (numUnits);
		mat.InitMatrix ();
		offense = new int[numUnits];
		defense = new int[numUnits];
		for (int i=0; i<100; i++) {
			offense [i % numUnits]++;
			defense [i % numUnits]++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timeLastSpawn > 0.2) {
			timeLastSpawn = Time.time;
			int goodIndex=GetBestUnit(NumAliveGood(),offense);
			Instantiate(offenseUnit[goodIndex],spawnGood.position,Quaternion.identity);
			numSpawnedGood[goodIndex]++;

			int badIndex=GetBestUnit(NumAliveBad(),defense);
			Instantiate (defenseUnit[badIndex],spawnBad.position,Quaternion.identity);
			numSpawnedBad[badIndex]++;
			//Debug.Log ("goodIndex: "+goodIndex+", badIndex: "+badIndex);
			spawnsGood+=""+goodIndex;
			spawnsBad+=""+badIndex;
		}
		if (spawnsGood.Length >= 20) {
			Debug.Log (spawnsGood + " " + spawnsBad);
			spawnsGood="";
			spawnsBad="";
		}
		//Debug.Log ("Score: "+TotalKillsGood()+" - "+TotalKillsBad());
		//Debug.Log ("Total spawned good: " + numSpawnedGood.Sum () + ", total spawned bad: " + numSpawnedBad.Sum ());
		//Debug.Log ("alive good: "+NumAliveGood ().Sum()+": " + ArrayToString (NumAliveGood ()) + ", alive bad: "+NumAliveBad().Sum()+": " + ArrayToString (NumAliveBad ()));
	}

}
