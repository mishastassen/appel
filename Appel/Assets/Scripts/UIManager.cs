using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public Text cell;
	public string str;
	private int[,] mat;
	public static int[] offense;
	private int[] defense;
	private Text[,] matrix;
	private int n;

	private int CalcDeterminant() {
		if (n != 3)
			throw new NotImplementedException();
		int res = 0;
		res+=mat[0,0]*(mat[1,1]*mat[2,2]-mat[2,1]*mat[1,2]);
		res-=mat[1,0]*(mat[0,1]*mat[2,2]-mat[2,1]*mat[0,2]);
		res+=mat[2,0]*(mat[0,1]*mat[1,2]-mat[1,1]*mat[0,2]);
		return res;
	}

	// System.Random: 1010
	void InitMatrix() {
		matrix  = new Text[n+1,n+1];
		for (int i=0; i<n+1; i++) {
			for (int j=0; j<n+1; j++) {
				matrix [i, j] = Instantiate<Text> (cell);
				//matrix [i, j].text = "" + mat [i, j];
				matrix [i, j].transform.SetParent (this.transform);
				matrix [i, j].transform.position = new Vector3 (j * 32 + 256, (3-i) * 32, 0);
			}
		}
		matrix[0,0].text = "";

		System.Random rng = new System.Random (1020);
		mat = new int[n,n];
		for (int i=0; i<n; i++) {
			for (int j=0; j<n; j++) {
				mat [i, j] = rng.Next(-5,6);
				matrix [i+1, j+1].text = "" + mat [i, j];
			}
		}
		int det = CalcDeterminant ();

		offense = new int[n];
		defense = new int[n];
		for (int i=0; i<n; i++) {
			offense[i]=100/n;
			defense[i]=100/n;
		}
		offense [0] += 100 % n;
		defense [0] += 100 % n;
		for (int i=0; i<n; i++) {
			matrix[i+1,0].text=""+offense[i];
			matrix[0,i+1].text=""+defense[i];
		}

		Debug.Log ("Det: "+det);
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
	}
	
	// Update is called once per frame
	void Update () {
		String erbij = "qwer";
		String eraf = "asdf";
		Debug.Log ("n: " + n);
		for(int i=0;i<n;i++) {

			if (Input.GetKeyDown (""+erbij[i]) && getSum (offense) < 100) {
				offense[i]++;
				matrix[1+i,0].text = ""+offense[i];
			}
			if (Input.GetKeyDown (""+eraf[i]) && offense[i]>0) {
				offense[i]--;
				matrix[1+i,0].text = ""+offense[i];
			}
		}

	}
}
