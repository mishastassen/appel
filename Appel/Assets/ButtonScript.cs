﻿using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
   
    public void GoBack()
    {
        Application.LoadLevel(0);
        Debug.Log("Go back");
    }
}
