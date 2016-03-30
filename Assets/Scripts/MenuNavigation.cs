using UnityEngine;
using System.Collections;

public class MenuNavigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		Application.LoadLevel("Character Selector");
	}

	public void LoadLevel(){
		Application.LoadLevel ("Load Menu");
	}

	public void Options(){
		Application.LoadLevel ("Options");
	}
}
