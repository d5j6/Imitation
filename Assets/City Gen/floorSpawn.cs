using UnityEngine;
using System.Collections;

public class floorSpawn : MonoBehaviour {
	void Start () {
		if ((transform.position.y <= 15) || (Random.Range (0, 100) >= 25 && transform.position.y <= 26)) {
			GameObject floor = (GameObject)Instantiate (gameObject, transform.position + new Vector3 (0, 3f, 0), Quaternion.Euler (transform.rotation.eulerAngles));
		}
	}
}
