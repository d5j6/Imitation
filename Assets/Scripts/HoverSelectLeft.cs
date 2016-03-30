using UnityEngine;
using System.Collections;

public class HoverSelectLeft : MonoBehaviour {

	int counter;
	SpriteRenderer SR;
	Select SScript;

	// Use this for initialization
	void Start () {
		counter = 0;
		SScript = GameObject.Find ("Character Selector").GetComponent<Select> ();
		SR = GetComponent<SpriteRenderer> ();
	}

	void OnMouseOver(){
		counter++;
		SR.color -= new Color (0f,0f,0.01f,0f);
		if(counter == 100){
			SScript.onClickLeft ();
			counter = 0;
		}

	}

	void OnMouseExit(){
		counter = 0;
		SR.color = new Color (1f,1f,1f,1f);
	}
}
