using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	public void StartTheGame(){
		Application.LoadLevel(1);
		Debug.Log ("Started the game");
	}

	public void QuitTheGame(){
		Application.Quit();
		Debug.Log ("Quit the game");
	}
}
