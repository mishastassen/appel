using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public Text cell;
	public string str;
	private int[,] mat;
	private Text[,] matrix;


	void InitMatrix(int n) {
		//line.transform.SetParent(this.transform);
		//line.transform.localPosition = new Vector3(0, 10, 0);
		//scores.Add(line);

		/*
		mat = new int[n,n];
		matrix  = new Text[n,n];
		for(int i=0;i<n;i++)
			for(int j=0;j<n;j++) {
				mat[i,j] = 3*i+j;
			if(cell==null)
				Debug.Log (str);
				//matrix[i,j] = Instantiate(cell);
			matrix[i,j].text = ""+str;//mat[i,j];
				matrix[i,j].transform.position = new Vector3(1,30*i,30*j);
			}
		*/
	}

	// Use this for initialization
	void Start () {
		int n = 3;
		InitMatrix (n);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
