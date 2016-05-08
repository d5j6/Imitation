using UnityEngine;
using System.Collections;

public class HackOnStart : MonoBehaviour {

	public string jointName;

	// Use this for initialization
	void Start () {
		if (enabled) {
			enabled = false;
			LimbHacker.instance.severByJoint (gameObject, jointName);
		}
			
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
