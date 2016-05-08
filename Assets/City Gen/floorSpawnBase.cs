using UnityEngine;
using System.Collections;

public class floorSpawnBase : MonoBehaviour {
	public GameObject floor;
	void Start () {
		Instantiate(floor, transform.position + new Vector3(0,3f,0), Quaternion.Euler(transform.rotation.eulerAngles));
	}
}
