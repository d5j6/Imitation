using UnityEngine;
using System.Collections;

public class carSpawn : MonoBehaviour {
	private int num;
	public blockLibrary librarian;
	private Transform car;
	// Use this for initialization
	void Start () {
		if (Random.Range (0, 100) > 30) {carDrop();}
	}

	void carDrop (){
		Vector3 temp;

		// Place the car
		car = Instantiate(librarian.carOut());
		// make it part of parent
		car.transform.parent = transform;
		// relocate to random location within local coordinates.
		temp = car.transform.localPosition;
		temp.x = Random.Range(0, 32);
		temp.z = Random.Range(-10, 0);
		car.transform.localPosition = temp;
		car.transform.rotation = Quaternion.Euler(new Vector3 (0f, Random.Range (0, 360f), 0f));

	}
}
