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
        float spacing = Screen.width / 24.0f;
        float yspacing = Screen.width / 30.0f;
        for (int i=0; i<n+2; i++) {
			for (int j=0; j<n+2; j++) {
				matrix [i, j] = Instantiate<Text> (cell);
<<<<<<< HEAD
                matrix[i, j].fontSize = (int)(Screen.width / 50.0f);
                matrix[i, j].alignment = TextAnchor.MiddleCenter;
                matrix[i, j].transform.SetParent(this.transform);
                // matrix [i, j].rectTransform.SetParent (this.transform);
                // matrix[i, j].rectTransform.anchoredPosition = new Vector2(0f, 1f);
                // matrix[i, j].rectTransform.position = new Vector3(j * 40 + 400, (3 - i) * 40 + 40, 0);
                Debug.Log(Screen.width); 
                //matrix[i, j].transform.position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
                matrix [i, j].transform.position = new Vector3 (j * spacing + Screen.width / 12.0f, (3-i) * yspacing + Screen.width / 20f, 0);
                matrix[i,j].color = Color.white;
=======
				matrix [i, j].transform.SetParent(this.transform);//GetComponentInChildren<Image>().rectTransform); //(this.transform);
				matrix[i,j].rectTransform.localScale = new Vector3(1,1,1);// = GameObject.Find ("Dummy").rectTransform.localScale;
				matrix [i, j].transform.position = new Vector3 (j * 40+300, (3-i) * 40, 0);
				matrix[i,j].color = Color.white;
>>>>>>> origin/master
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

	private string MakeString(int a) {
		String res = "";
		if (a < 0) {
			a*=-1;
			res+="-";
		}
		return res + (a / 100) + "." + ((a / 10) % 10);// + (a % 10);
	}

	// Use this for initialization
	void Start () {
		n = 3;
		InitMatrix ();
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("hoi");
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

		if (Input.GetKeyDown ("p")) {
			if(Time.timeScale==1.0f)
				Time.timeScale=0f;
			else
				Time.timeScale=1.0f;
		}

		// update whole table
		for (int i=0; i<n; i++) {
			for (int j=0; j<n; j++) {
				matrix [i+1, j+1].text = "" + GM.mat.mat [i, j];
			}
		}
		for (int i=0; i<n; i++) {
			matrix[i+1,0].text=MakeString(GM.offense[i]);
			matrix[0,i+1].text=MakeString(GM.defense[i]);
		}
		for(int i=0;i<n;i++) {
			matrix[i+1,n+1].text=MakeString(GM.expOffense[i]);
			matrix[n+1,i+1].text=MakeString(GM.expDefense[i]);
		}
	}
}
