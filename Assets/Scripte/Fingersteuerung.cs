using UnityEngine;
using System.Collections;

public class Fingersteuerung : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Animation>().Play();
	//	animation.Play("Pointer", PlayMode.StopAll);
	}
	

	// Update is called once per frame
	void Update () {

	/*	if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Vector3 position = this.transform.position;
			position.x++;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Vector3 position = this.transform.position;
			position.x--;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Vector3 position = this.transform.position;
			position.y++;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Vector3 position = this.transform.position;
			position.y--;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Vector3 position = this.transform.position;
			position.z--;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Vector3 position = this.transform.position;
			position.z++;
			this.transform.position = position;
		}
		*/
	}
}
