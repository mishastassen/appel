using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	public Button[] buttons;

	public void StartTheGame(){
		Application.LoadLevel(1);
		Debug.Log ("Started the game");
    }

	public void StartLevel2() {
		GameManager.levelNumber = 2;
		Application.LoadLevel(1);
	}

	public void StartLevel3() {
		GameManager.levelNumber = 3;
		Application.LoadLevel(1);
	}
	
	public void StartLevel4() {
		GameManager.levelNumber = 4;
		Application.LoadLevel(1);
	}
	
	public void StartLevel5() {
		GameManager.levelNumber = 5;
		Application.LoadLevel(1);
	}
	
	public void StartHelp()
    {
        Application.LoadLevel(2);
        Debug.Log("Helped the game");
    }

    public void QuitTheGame(){
		Application.Quit();
		Debug.Log ("Quit the game");
	}

	void Start() {
		int maxlevel = PlayerPrefs.GetInt("maxlevel",1);
		Debug.Log ("maxlevel: "+maxlevel);
		for (int i=0; i<4; i++) {
			Debug.Log ("check level: " + (i + 2) + ", "+((i + 2) <= maxlevel));
			Vector3 pos = buttons [i].transform.position;
			if(i + 2 > maxlevel)
				pos.y=1000;
			buttons [i].transform.position = pos;
		}
	}
}
