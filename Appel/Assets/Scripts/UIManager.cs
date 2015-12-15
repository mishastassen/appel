using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public Slider[] sliders;
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
                matrix[i, j].fontSize = (int)(Screen.width / 50.0f);
                matrix[i, j].alignment = TextAnchor.MiddleCenter;
                matrix[i, j].transform.SetParent(this.transform);
                // matrix [i, j].rectTransform.SetParent (this.transform);
                // matrix[i, j].rectTransform.anchoredPosition = new Vector2(0f, 1f);
                // matrix[i, j].rectTransform.position = new Vector3(j * 40 + 400, (3 - i) * 40 + 40, 0);
                //Debug.Log(Screen.width); 
                //matrix[i, j].transform.position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
                matrix [i, j].transform.position = new Vector3 (j * spacing + Screen.width / 11.0f, (3-i) * yspacing + Screen.width / 20f, 0);
                matrix[i,j].color = Color.white;
				/*matrix [i, j].transform.SetParent(this.transform);//GetComponentInChildren<Image>().rectTransform); //(this.transform);
				matrix[i,j].rectTransform.localScale = new Vector3(1,1,1);// = GameObject.Find ("Dummy").rectTransform.localScale;
				matrix [i, j].transform.position = new Vector3 (j * 40+300, (3-i) * 40, 0);
				matrix[i,j].color = Color.white;*/
				matrix[i,j].text = "";

			}
		}
	}

	float getSum(float[] q) {
		float res = 0;
		for (int i=0; i<q.Length; i++)
			res += q [i];
		return res;
	}

	private string MakeString1(float a) {
		String res = "";
		if (a < 0) {
			a*=-1;
			res+="-";
		}
		int b = (int)(a + 0.5);
		return "" + b;//res + (a / 100) + "." + ((a / 10) % 10);// + (a % 10);
	}

	private string MakeString2(float a) {
		if(a>0)
			a+=0.5f;
		else
			a-=0.5f;
		return MakeString ((int)a);
	}

	private string MakeString(int a) {
		String res = "";
		if (a < 0) {
			a*=-1;
			res+="-";
		}
		a += 5; // voor afronding op 1 decimaal
		return res + (a / 100) + "." + ((a / 10) % 10);// + (a % 10);
	}

	void CheckSliders() {
		int indexChanged = -1;
		for (int i=0; i<n; i++)
			if (Mathf.Abs (sliders[i].value - GM.offense[i])>0.001f)
				indexChanged = i;
		if (indexChanged == -1)
			return;

		GM.offense [indexChanged] = sliders [indexChanged].value;
		float sum = 0;
		for(int i=0;i<n;i++)
			if(i!=indexChanged)
				sum+=sliders[i].value;
		float desiredSum = 100 - GM.offense [indexChanged];
		if (sum == 0) {
			for(int i=0;i<n;i++)
				if(i!=indexChanged)
					GM.offense[i]=desiredSum/(n-1);
		} else {
			float factor = desiredSum / sum;
			for(int i=0;i<n;i++)
				if(i!=indexChanged)
					GM.offense[i]*=factor;
		}
		for (int i=0; i<n; i++)
			sliders [i].value = GM.offense [i];
	}
	

	// Use this for initialization
	void Start () {
		n = 3;
		InitMatrix ();
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckSliders ();
		String erbij = "qwer";
		String eraf = "asdf";
		// process keyboard input
		for(int i=0;i<n;i++) {
			if (Input.GetKeyDown (""+erbij[i]) && getSum (GM.offense) <=99) {
				GM.offense[i]+=1;
			}
			if (Input.GetKeyDown (""+eraf[i]) && GM.offense[i]>=1) {
				GM.offense[i]-=1;
			}
		}

		// should pause it
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

				float ratio = (GM.mat.mat [i, j]+5.01f)/10.1f; // .01 en .1, anders misschien erbuiten
				if(ratio<0.0f||ratio>1.0f)
					Debug.Log ("ratio: "+ratio);
				matrix[i+1,j+1].color = new Vector4(1-ratio,ratio,0,1); 
			}
		}
		for (int i=0; i<n; i++) {
			matrix[i+1,0].text=MakeString1(GM.offense[i]);
			matrix[0,i+1].text=MakeString1(GM.defense[i]);
		}
		for(int i=0;i<n;i++) {
			matrix[i+1,n+1].text=MakeString2(GM.expOffense[i]);
			matrix[n+1,i+1].text=MakeString2(GM.expDefense[i]);
			float ratio = (GM.mat.mat [i, n-1]/100.0f+5.01f)/10.1f; // anders misschien erbuiten
			if(ratio<0.0f||ratio>1.0f)
				Debug.Log ("ratio: "+ratio);
			matrix[i+1,n+1].color = new Vector4(1-ratio,ratio,0,1); 

			ratio = (GM.mat.mat [n-1, i]/100.0f+5.01f)/10.1f; // anders misschien erbuiten
			if(ratio<0.0f||ratio>1.0f)
				Debug.Log ("ratio: "+ratio);
			matrix[n+1,i+1].color = new Vector4(1-ratio,ratio,0,1); 
		}
	}
}
