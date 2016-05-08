using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGen_MasterPrefabScript : MonoBehaviour {
	public LevelGen_PrefabHolder[] prefablist = new LevelGen_PrefabHolder[10];
	public Dictionary<string, int> advancedCoordinateSystem = new Dictionary<string, int>();
	string coordinateString;
	string coordinateString2;
	string coordinateString3;

	public int trackerX = 0;
	public int trackerY = 0;
	int i_randomPicker;
	int i_randomDirection;
	public int currentX = 0;
	public int currentY = 0;
	int currentDisplaceX = 0;
	int currentDisplaceY = 0;
	int lastGeneration = 0;
	int currentGeneration  = 0;
	// Use this for initialization
	void Start () {
		lastGeneration = Random.Range (0, 9);
		SpawnPrefab (0);



	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.UpArrow)) 
		{
				currentY += 128;
				trackerY -= 1;
				SpawnPrefab(2);

		}
		else if (Input.GetKeyUp (KeyCode.DownArrow)) 
		{
				currentY -= 128;
				trackerY += 1;
				SpawnPrefab(3);

		}
		else if (Input.GetKeyUp (KeyCode.LeftArrow)) 
		{
				currentX -= 128;
				trackerX -= 1;
				SpawnPrefab(1);

		}
		else if (Input.GetKeyUp (KeyCode.RightArrow)) 
		{
	
				currentX += 128;
				trackerX += 1;
				SpawnPrefab(0);
		}
	}
	
	void SpawnPrefab(int direction) // up or down
	{

		i_randomDirection = Random.Range (0, 2);
		i_randomDirection = (i_randomDirection == 0) ? -1 : 1;
		currentGeneration = lastGeneration + i_randomDirection;
		if (currentGeneration < 0) {
			currentGeneration = 9;
		} else if (currentGeneration > 9) {
			currentGeneration = 0;
		}


		//Check and spawn each top thing.
		coordinateString = (trackerX).ToString () + "," + (trackerY + 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX, 0, currentY - 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}

		coordinateString = (trackerX).ToString () + "," + (trackerY).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX, 0, currentY), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}

		coordinateString = (trackerX).ToString () + "," + (trackerY - 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX, 0, currentY + 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}


		//Spawn side 1
		coordinateString = (trackerX + 1).ToString () + "," + (trackerY + 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX + 128, 0, currentY - 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}

		coordinateString = (trackerX + 1).ToString () + "," + (trackerY).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX + 128, 0, currentY ), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}
		
		coordinateString = (trackerX + 1).ToString () + "," + (trackerY - 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX + 128, 0, currentY + 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}

		//Spawn side 2
		coordinateString = (trackerX - 1).ToString () + "," + (trackerY + 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX - 128, 0, currentY - 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}
		
		coordinateString = (trackerX - 1).ToString () + "," + (trackerY).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX - 128, 0, currentY ), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}
		
		coordinateString = (trackerX - 1).ToString () + "," + (trackerY - 1).ToString ();
		if(!advancedCoordinateSystem.ContainsKey(coordinateString))
		{
			Instantiate ((Object)prefablist[currentGeneration], new Vector3(currentX - 128, 0, currentY + 128), transform.rotation);
			advancedCoordinateSystem[coordinateString] = currentGeneration;
			Debug.Log (coordinateString);
		}








		lastGeneration = currentGeneration;
		}

	public int prefabVisibleAt(string positionName)
	{
		return advancedCoordinateSystem [positionName];
	}
}
