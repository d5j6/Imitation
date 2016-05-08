using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Animator anim = GetComponent<Animator>();
        anim.Play("Hand_Slash_2_Char00",0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
