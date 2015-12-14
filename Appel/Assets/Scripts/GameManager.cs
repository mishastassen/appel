using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	private int numUnits=3;
	private int maxCountUnits = 800;
	private int criticalMinimum = 10;
	private int criticalDifference = 100;
	private bool gameDone;

	public List<GameObject> offenseUnit = new List<GameObject>();
	public List<GameObject> defenseUnit = new List<GameObject>();

	public List<Transform> spawnGood = new List<Transform>();
	public List<Transform> spawnBad = new List<Transform>();

	private float timeLastSpawn=0;
	private int[] numSpawnedGood;
	private int[] numSpawnedBad;
	public int[] numKilledGood;
	public int[] numKilledBad;

	public Matrix mat;
	public int[] offense;
	public int[] defense;

	public int[] expOffense;
	public int[] expDefense;

	private float lastBigComputerUpdate, lastMiniComputerUpdate;

	private string spawnsGood = "", spawnsBad="";

	// to have to right ratio for the units on battlefield as the players command
	int GetNeededUnit(int[] current, int[] desired) {
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

	bool IsEnoughRoomOnBattleField() {
		return NumAliveGood().Sum () + NumAliveBad ().Sum () < maxCountUnits;
	}
	
	void UpdateExpectations() {
		for (int i=0; i<numUnits; i++) {
			expOffense [i] = 0;
			expDefense [i] = 0;
		}
		for (int i=0; i<numUnits; i++) {
			for(int j=0;j<numUnits;j++) {
				expOffense[i]+=defense[j]*mat.mat[i,j];
				expDefense[j]+=offense[i]*mat.mat[i,j];
			}
		}
	}
	
	
	void UpdateComputerVector() {
		if (Time.time > lastBigComputerUpdate + 4) {
			if(Time.time > lastBigComputerUpdate + 6)
				lastBigComputerUpdate = Time.time+2*Random.value;
			if(Time.time > lastMiniComputerUpdate+0.1f) {
				lastMiniComputerUpdate = Time.time;
				int best = 0, worst = -1;
				for (int i=0; i<numUnits; i++) {
					if (expDefense [i] < expDefense [best])
						best = i;
					if (worst == -1 || (expDefense [i] > expDefense [worst] && defense [i] > 0))
						worst = i;
				}
				if (defense [worst] > 0) {
					defense [best]++;
					defense [worst]--;
				}
			}
		}
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
		expOffense = new int[numUnits];
		expDefense = new int[numUnits];
		for (int i=0; i<100; i++) {
			offense [i % numUnits]++;
			defense [i % numUnits]++;
		}
		UpdateExpectations ();
		gameDone = false;
	}

	void ShowGameOver() {
		gameDone = true;
		Application.LoadLevel (3);
	}

	void ShowVictory() {
		gameDone = true;
		GameInfo.levelNumber++;
	}

	// Update is called once per frame
	void Update () {
		if (gameDone)
			return;
		UpdateExpectations ();
		UpdateComputerVector ();
		if((NumAliveGood ().Sum () > 10 || NumAliveBad ().Sum () <50)&&(NumAliveBad ().Sum () > 10 || NumAliveGood ().Sum () <50))
		if (Time.time - timeLastSpawn > 0.2 && IsEnoughRoomOnBattleField()) {
			timeLastSpawn = Time.time;
			int goodIndex = -1, badIndex = -1;
			if(NumAliveGood ().Sum () > 10 || NumAliveBad ().Sum () <50) {
				goodIndex=GetNeededUnit(NumAliveGood(),offense);
				Instantiate(offenseUnit[goodIndex],spawnGood[goodIndex].position,Quaternion.identity);
				numSpawnedGood[goodIndex]++;
			}

			if(NumAliveBad ().Sum () > 10 || NumAliveGood ().Sum () <50) {
				badIndex=GetNeededUnit(NumAliveBad(),defense);
				Instantiate (defenseUnit[badIndex],spawnBad[badIndex].position,Quaternion.identity);
				numSpawnedBad[badIndex]++;
			}
			//Debug.Log ("goodIndex: "+goodIndex+", badIndex: "+badIndex);
			//spawnsGood+=""+goodIndex;
			//spawnsBad+=""+badIndex;
		}

		if ((NumAliveGood ().Sum () == 0 && NumAliveBad ().Sum () > 10))
			ShowGameOver ();
		if ((NumAliveBad ().Sum () == 0 && NumAliveGood ().Sum () > 10))
			ShowVictory ();

		/*
		if (spawnsGood.Length >= 20) {
			Debug.Log (spawnsGood + " " + spawnsBad);
			spawnsGood="";
			spawnsBad="";
		}
		//*/
		//Debug.Log ("Score: "+TotalKillsGood()+" - "+TotalKillsBad());
		//Debug.Log ("Total spawned good: " + numSpawnedGood.Sum () + ", total spawned bad: " + numSpawnedBad.Sum ());
		//Debug.Log ("alive good: "+NumAliveGood ().Sum()+": " + ArrayToString (NumAliveGood ()) + ", alive bad: "+NumAliveBad().Sum()+": " + ArrayToString (NumAliveBad ()));
	}

}
