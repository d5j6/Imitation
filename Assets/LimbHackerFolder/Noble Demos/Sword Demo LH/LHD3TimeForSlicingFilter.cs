using UnityEngine;
using System.Collections;

public class LHD3TimeForSlicingFilter : MonoBehaviour
{
	public string nameOfAnimationStateForSlicing = "Standing";
	public LHD3Spawner spawner;
	
	private Animator animator;
	
	void Awake()
	{
		spawner.instantiationListeners += HandleInstantiation;
	}
	
	void HandleInstantiation(GameObject go)
	{	
		animator = go.GetComponent<Animator>();
	}
	
	
}
