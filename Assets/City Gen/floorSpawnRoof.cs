using UnityEngine;
using System.Collections;

public class floorSpawnRoof : MonoBehaviour {
	public GameObject roof;

	void Start () {
		if (Random.Range (0, 100) >= 25 && transform.position.y <= 26) {
			GameObject floor = (GameObject)Instantiate (gameObject, transform.position + new Vector3 (0, 3f, 0), Quaternion.Euler (transform.rotation.eulerAngles));
		} else{
			Instantiate (roof, transform.position + new Vector3 (0, 3f, 0), Quaternion.Euler (transform.rotation.eulerAngles));
		}
	}
}
