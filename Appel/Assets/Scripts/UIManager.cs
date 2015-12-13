using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public Text cell;
	private Text[,] matrix;
	private int n;
	private GameManager GM;


	// System.Random: 1010
	void InitMatrix() {
		matrix  = new Text[n+2,n+2];
		for (int i=0; i<n+2; i++) {
			for (int j=0; j<n+2; j++) {
				matrix [i, j] = Instantiate<Text> (cell);
				matrix [i, j].transform.SetParent (this.transform);
				matrix [i, j].transform.position = new Vector3 (j * 32 + 256, (3-i) * 32, 0);
				matrix[i,j].text = "";

			}
		}
	}

	int getSum(int[] q) {
		int res = 0;
		for (int i=0; i<q.Length; i++)
			res += q [i];
		return res;
	}

	// Use this for initialization
	void Start () {
		n = 3;
		InitMatrix ();
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		String erbij = "qwer";
		String eraf = "asdf";
		// process keyboard input
		for(int i=0;i<n;i++) {
			if (Input.GetKeyDown (""+erbij[i]) && getSum (GM.offense) < 100) {
				GM.offense[i]++;
				matrix[1+i,0].text = ""+GM.offense[i];
			}
			if (Input.GetKeyDown (""+eraf[i]) && GM.offense[i]>0) {
				GM.offense[i]--;
				matrix[1+i,0].text = ""+GM.offense[i];
			}
		}

		// update whole table
		for (int i=0; i<n; i++) {
			for (int j=0; j<n; j++) {
				matrix [i+1, j+1].text = "" + GM.mat.mat [i, j];
			}
		}
		for (int i=0; i<n; i++) {
			matrix[i+1,0].text=""+GM.offense[i];
			matrix[0,i+1].text=""+GM.defense[i];
		}
		for(int i=0;i<n;i++) {
			matrix[i+1,n+1].text=""+GM.expOffense[i];
			matrix[n+1,i+1].text=""+GM.expDefense[i];
		}
	}
}
