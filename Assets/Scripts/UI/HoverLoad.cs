using UnityEngine;
using System.Collections;

public class HoverLoad : MonoBehaviour {

	float counter;
	MenuNavigation MNScript;
	SpriteRenderer SR;

	// Use this for initialization
	void Start () {
		counter = 0;
		MNScript = GameObject.Find ("Button Select").GetComponent<MenuNavigation> ();
		SR = GetComponent<SpriteRenderer> ();
	}

	void OnMouseOver(){
		counter++;
		SR.color -= new Color (0f,0f,0.01f,0f);
		if(counter == 100){
			MNScript.LoadLevel ();
		}

	}

	void OnMouseExit(){
		counter = 0;
		SR.color = new Color (1f,1f,1f,1f);
	}

	public float getCounter(){
		return counter;
	}
}
