using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public AudioSource audio;

    private int numUnits=3;
	private int maxCountUnits = 800;
	private int criticalMinimum = 10;
	private int criticalDifference = 100;
	private bool gameDone;
	private float victoryTime;
	public static int levelNumber = 1;
	public Text victory;
	public Text Level;

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
	public float[] offense;
	public float[] defense;

	public float[] expOffense;
	public float[] expDefense;

	private float lastBigComputerUpdate, lastMiniComputerUpdate;

	private string spawnsGood = "", spawnsBad="";

	// to have to right ratio for the units on battlefield as the players command
	int GetNeededUnit(int[] current, float[] desired) {
		int bestIndex = 0;
		float lowestFactor = current [0] / (desired [0]+0.001f);
		for (int i=1; i<numUnits; i++) {
			if(lowestFactor > current [i] / (desired [i]+0.001f)) {
				bestIndex=i;
				lowestFactor = current [i] / (desired [i]+0.001f);
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
		if (Time.time > lastBigComputerUpdate + 4)
			lastBigComputerUpdate = Time.time+Random.value;
		if(lastMiniComputerUpdate+0.1f < Time.time &&
		   Time.time < lastBigComputerUpdate + 1) {
			lastMiniComputerUpdate = Time.time;
			int best = 0, worst = -1;
			for (int i=0; i<numUnits; i++) {
				if (expDefense [i] < expDefense [best])
					best = i;
				if (worst == -1 || (expDefense [i] > expDefense [worst] && defense [i] > 0))
					worst = i;
			}
			if (defense [worst] > 0) {
				defense [best]+=1;
				defense [worst]-=1;
			}
		}
	}

	void ShowGameOver() {
		gameDone = true;
		levelNumber = 1;
		Application.LoadLevel (3);
	}
	
	void ShowVictory() {
		Vector3 startPos = new Vector3 (Screen.width / 2, 2*Screen.height, 0);
		Vector3 endPos = new Vector3 (Screen.width / 2, Screen.height/2, 0);
		if (!gameDone) {
            audio.Stop();
            
            audio.PlayOneShot(winnerclip); 
			gameDone = true;
			levelNumber = Mathf.Min(levelNumber+1,5);
			int maxlevel = PlayerPrefs.GetInt("maxlevel",1);
			if(levelNumber>maxlevel) {
				PlayerPrefs.SetInt("maxlevel",levelNumber);
				if(levelNumber==3)
					victory.text = "Victory\nYou unlocked the dot product!";
			}
			victoryTime = Time.time;
			victory.transform.position = startPos;
			victory.enabled = true;
		} else {
			float elapsed = (Time.time-victoryTime)/1.5f;
			float nextY = Screen.height/2 + Mathf.Max (0f,(1.5f-elapsed))*Mathf.Max (0f,(1.5f-elapsed))*Mathf.Max (0f,(1.5f-elapsed))*(Screen.height/2)*Mathf.Cos(4*elapsed);
			endPos.y = nextY;
			victory.transform.position = endPos;//Vector3.Lerp (startPos, endPos, (Time.time-victoryTime) / 1.5f);

			if (Time.time > victoryTime + 5)
				Application.LoadLevel (1);

		}
		if (Input.GetKeyDown ("space"))
			Application.LoadLevel (1);
	}

    public AudioClip musicsound;
    public AudioClip winnerclip;
    // Use this for initialization
    void Start ()
    {
        audio = GetComponentInChildren<AudioSource>();
        audio.PlayOneShot(musicsound); 
        numSpawnedGood = new int[numUnits];
		numSpawnedBad = new int[numUnits];
		numKilledGood = new int[numUnits];
		numKilledBad = new int[numUnits];

		mat = new Matrix (numUnits);
		mat.GetMatrix (Mathf.Min (5,levelNumber));
		offense = new float[numUnits];
		defense = new float[numUnits];
		expOffense = new float[numUnits];
		expDefense = new float[numUnits];
		for (int i=0; i<100; i++) {
			offense [i % numUnits]+=1;
			defense [i % numUnits]+=1;
		}
		UpdateExpectations ();
		gameDone = false;
		victory.enabled = false;
		Level.text = "Level: " + levelNumber;
	}

	// Update is called once per frame
	void Update () {
		if (gameDone) {
			if(NumAliveGood ().Sum ()>0)
				ShowVictory();
			else
				ShowGameOver();
		}
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
