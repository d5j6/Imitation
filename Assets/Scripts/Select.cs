using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour {
		//These hold all the characters from the hierarchy view
	public GameObject p1;
	public GameObject p2;
	public GameObject p3;
	public GameObject p4;
	public GameObject p5;
	public GameObject p6;
	public GameObject p7;
	public GameObject p8;
	public GameObject p9;
	public GameObject p10;
	public GameObject p11;

		//Stores all characters from the hierarchy
	GameObject[] go = new GameObject[11];
	int i;

		
	void Start () {
			//Placing each character into array
		go [0] = p1;
		go [1] = p2;
		go [2] = p3;
		go [3] = p4;
		go [4] = p5;
		go [5] = p6;
		go [6] = p7;
		go [7] = p8;
		go [8] = p9;
		go [9] = p10;
		go [10] = p11;
			//This keeps track of what character is currently selected
		i = 0;
	}

		//Switches characters from left to right
	public void onClickRight(){
			//This turns off current character selected
		go [i].active = false;

			//This makes sure array is always in bounds. If it goes out of bounds,
			//then it sets it back to the beginning of the array.
		if (i == 10) {
			i = -1;
		}
			//This turns the next character to be selected on.
		go [i + 1].active = true;
			//i is incremented to reflect current character selected in array
		i++;
	}

	//Switches characters from right to left
	public void onClickLeft(){
			//This turns off current character selected
		go [i].active = false;
			//This makes sure array is always in bounds. If it goes out of bounds,
			//then it sets it back to the end of the array.
		if (i == 0) {
			i = 11;
		}
			//This turns the next character to be selected on.
		go [i - 1].active = true;
			//i is decremented to reflect current character selected in array
		i--;
	}

		//Loads the in game scene
	public void onClickSelect(){
		Application.LoadLevel ("In Game");
	}

		//Returns character currently selected
	public GameObject returnCharacterSelected(){
		return go [i];
	}
}
