using UnityEngine;
using System.Collections;

public class createCorner : MonoBehaviour {
	private Transform block;
	private float[,] loc = new float [,]{{-12.5f,24f},{-12.5f,5.75f},{-12.5f,-12.5f},{5.75f,-12.5f},{24f,-12.5f}};
	public blockLibrary librarian;
	public Transform floor;
	void build(){
		Vector3 temp;

		for (int i = 0; i < 5; i++) {
			// Place the building
			block = Instantiate(librarian.buildOut());
			// make it part of parent
			block.transform.parent = this.transform;
			// relocate to chosen location within local coordinates.
			temp = block.transform.localPosition;
			temp.x = loc [i, 0];
			temp.z = loc [i, 1];
			block.transform.localPosition = temp;
		}
	}
	// Use this for initialization
	void Start () {
		build ();
		Instantiate(floor, transform.position - new Vector3(32f,0,32f), Quaternion.Euler(transform.rotation.eulerAngles));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
