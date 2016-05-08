using UnityEngine;
using System.Collections;

public class blankCornerSpawn : MonoBehaviour {

	public Transform corner;

	void Start () {
		Instantiate (corner, transform.position + new Vector3 (0, 0, 64f), Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0f, (90.0f), 0f)));
		Instantiate (corner, transform.position + new Vector3 (64f, 0, 64f), Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0f, (180.0f), 0f)));
		Instantiate (corner, transform.position + new Vector3 (64f, 0, 0), Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0f, (270.0f), 0f)));

		//corner.transform.parent = this.transform;
	}
}
