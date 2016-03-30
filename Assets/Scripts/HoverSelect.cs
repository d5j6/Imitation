using UnityEngine;
using System.Collections;

public class HoverSelect : MonoBehaviour {

	int counter;

	Select SScript;

	// Use this for initialization
	void Start () {
		counter = 0;
		SScript = GameObject.Find ("Character Selector").GetComponent<Select> ();
	}
	
	void OnMouseOver(){
		counter++;

		if(counter == 100){
			if (gameObject.name == "RightArrow") {
				SScript.onClickRight ();
			}
			if (gameObject.name == "LeftArrow") {
				SScript.onClickLeft();
			}

			counter = 0;
		}

	}

	void OnMouseExit(){
	
	}
}
