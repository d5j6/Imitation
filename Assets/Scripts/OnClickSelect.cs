using UnityEngine;
using System.Collections;

public class OnClickSelect : MonoBehaviour {


	Select SScript;
	public Rigidbody rb;
	public LayerMask smack = -1;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
	
		SScript = GameObject.Find ("Character Selector").GetComponent<Select> ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Physics.Raycast (ray, out hit, 100f, smack);
			rb = hit.rigidbody;

			if (rb.name == "LeftArrow") {
				SScript.onClickLeft ();
			}

			if (rb.name == "RightArrow") {
				SScript.onClickLeft ();
			}

		}
	}

	void onMouseHover(){
	}

	void onMouseExit(){
	}

}
