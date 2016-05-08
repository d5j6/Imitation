using UnityEngine;
using System.Collections;

public class HoverPlay : MonoBehaviour {

	int counter;
	SpriteRenderer SR;
	Select SScript;
	Player PS;

	// Use this for initialization
	void Start () {
		counter = 0;
		SScript = GameObject.Find ("Character Selector").GetComponent<Select> ();
		SR = GetComponent<SpriteRenderer> ();
		PS = GameObject.Find ("Protagonist").GetComponent<Player> ();
	}

	void OnMouseOver(){
		counter++;
		SR.color -= new Color (0f,0f,0.01f,0f);
		if(counter == 100){
			PS.setPlayer (SScript.returnCharacterSelected());
			SScript.onClickSelect ();
			counter = 0;
		}

	}

	void OnMouseExit(){
		counter = 0;
		SR.color = new Color (1f,1f,1f,1f);
	}
}
