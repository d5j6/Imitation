using UnityEngine;
using System.Collections;

public class blockLibrary : MonoBehaviour {
	public Transform[] cars;
	public Transform[] buildings;

	void Awake () {
	}

	public Transform buildOut(){
		return buildings[Random.Range (0, buildings.Length)];
	}

	public Transform carOut(){
		return cars[Random.Range (0, cars.Length)];
	}
}
