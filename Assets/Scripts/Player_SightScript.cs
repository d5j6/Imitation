using UnityEngine;
using System.Collections;

public class Player_SightScript : MonoBehaviour {
	public LevelGen_PrefabHolder[] visiblePrefabs = new LevelGen_PrefabHolder[9];
	public LevelGen_PrefabHolder[] prefablist = new LevelGen_PrefabHolder[10];
	public LevelGen_MasterPrefabScript serverLists;
	public int xCoord = 0;
	public int yCoord = 0;


	public GameObject probe;
	public GameObject cube;
	public ReflectionProbe probey;
	public Texture testTexture;
	public Texture testTexture2;
	public Material testMaterial;
	public Cubemap cubey;
	string coordinateString;
	// Use this for initialization
	void Start () {
		probey = probe.GetComponent<ReflectionProbe> ();
		testTexture = probey.bakedTexture;
		testMaterial.SetTexture("_MainTex", testTexture);
	}
	
	// Update is called once per frame
	void Update () 
	{
		xCoord = serverLists.trackerX;
		yCoord = serverLists.trackerY;
		this.transform.position = new Vector3 (xCoord * 64, 0, yCoord * -64);
		//Side 1
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [0] = prefablist[serverLists.prefabVisibleAt (coordinateString)];

		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [1] = prefablist[serverLists.prefabVisibleAt (coordinateString)];

		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [2] = prefablist[serverLists.prefabVisibleAt (coordinateString)];

		//Side 2
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [3] = prefablist[serverLists.prefabVisibleAt (coordinateString)];
		
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [4] = prefablist[serverLists.prefabVisibleAt (coordinateString)];
		
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [5] = prefablist[serverLists.prefabVisibleAt (coordinateString)];

		//Side 3
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [6] = prefablist[serverLists.prefabVisibleAt (coordinateString)];
		
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [7] = prefablist[serverLists.prefabVisibleAt (coordinateString)];
		
		coordinateString = (xCoord).ToString () + "," + (yCoord).ToString ();
		visiblePrefabs [8] = prefablist[serverLists.prefabVisibleAt (coordinateString)];
		/*
		camera1.targetTexture = side1.GetComponent<RenderTexture> ();
		camera2.targetTexture = side2.GetComponent<RenderTexture> ();
		camera3.targetTexture = side3.GetComponent<RenderTexture> ();
		camera4.targetTexture = side4.GetComponent<RenderTexture> ();
		*/
	

	}
}
