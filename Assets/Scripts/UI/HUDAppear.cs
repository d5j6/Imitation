using UnityEngine;
using System.Collections;

public class HUDAppear : MonoBehaviour {

	public GameObject go;
	CanvasGroup cv;
	bool flag = false;

	// Use this for initialization
	void Start () {
		cv = go.GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			flag = true;

		}
		if (flag && cv.alpha <= 1) {
			cv.alpha += 0.01f;  
		}
		if (cv.alpha == 1) {
			flag = false;
		}
		if (!flag && cv.alpha >= 0) {
			cv.alpha -= 0.01f;
		}

	}
}
