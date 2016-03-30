using UnityEngine;
using System.Collections;
using UnityEditor;

public class Rotating : MonoBehaviour {

	Transform tran;
	int counter;
	Rotating RS;

	// Use this for initialization
	void Start () {
		tran = GetComponent<Transform> ();
		counter = 180;
		RS = GetComponent<Rotating> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (EditorApplication.currentScene == "Assets/Scenes/In Game.unity") {
			RS.enabled = false;
		}
		counter++;
		Quaternion target = Quaternion.Euler (0, counter, 0);
		tran.rotation = Quaternion.Slerp (tran.rotation, target, Time.deltaTime * 2.0f);
	}
}
