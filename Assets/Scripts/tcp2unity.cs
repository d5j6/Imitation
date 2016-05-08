using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using RedCorona.Net;
using MathNet.Numerics;
using SimpleJSON;



public class tcp2unity : MonoBehaviour {
	private GameObject kl;												//Finger
	private GameObject ri;
	private GameObject mi;
	private GameObject ze;
	private GameObject da;
	
	GameObject dir;


	float [] pot = new float[5];

	int [] rot = new int[3];
	int [] rotCAL = new int[3];
	
	float [] offset = new float[3];
	float [] acc = new float[3];
	float [] old_acc = new float[3];
	float [] v = new float[3];
	float [] vges = new float[3];
	float [,] ballPosition= new float[6,3];
	
	float delt=0;
	float t;
	
	GameObject hand;
	Rigidbody rigid;

	String receivedText;
	Socket sock;
	ClientInfo client;

	void Start(){
		hand = GameObject.Find ("Hand");

		kl = GameObject.Find ("FKl");
		ri = GameObject.Find ("FRi");
		mi = GameObject.Find ("FMi");
		ze = GameObject.Find ("FZe");
		da = GameObject.Find ("FDa");

		rotCAL [0] = 0;
		rotCAL [1] = 0;
		rotCAL [2] = 0;

		offset [0] = hand.transform.position.x;
		offset [1] = hand.transform.position.y;
		offset [2] = hand.transform.position.z;

		QualitySettings.antiAliasing = 8;
		sock = Sockets.CreateTCPSocket(/*"127.0.0.1"*/ "hoffmail.me", 1337);
		client = new ClientInfo(sock, false); // Don't start receiving yet
		client.OnRead += new ConnectionRead(ReadData);
		client.Delimiter = "~";  // this is the default, shown for illustration
		client.BeginReceive();

		rigid = hand.GetComponent<Rigidbody> ();
		rigid.useGravity = false;
	}
	
	void ReadData(ClientInfo ci, String text) {
		receivedText = text;
		if (receivedText [0] == '{') {
			//Debug.Log("Received text message: "+text);
			SimpleJSON.JSONNode root = JSON.Parse (receivedText);

			pot [0] = root ["pot"] [0].AsFloat /1023F;
			pot [1] = root ["pot"] [1].AsFloat /1023F;
			pot [2] = root ["pot"] [2].AsFloat /1023F;
			pot [3] = root ["pot"] [3].AsFloat /1023F;
			pot [4] = root ["pot"] [4].AsFloat /1023F;
		
			acc [2] = -(root ["IMU"] ["a"] [0].AsFloat / 20);
			acc [0] = root ["IMU"] ["a"] [1].AsFloat / 20;
			acc [1] = root ["IMU"] ["a"] [2].AsFloat / 20;
		
			rot [1] =  root ["IMU"] ["o"] [0].AsInt;
			rot [0] =  root ["IMU"] ["o"] [1].AsInt;
			rot [2] = -root ["IMU"] ["o"] [2].AsInt;

			//Debug.Log ("x" + rot[0] + "y" + rot[1] + "z" + rot[2] + "\n");

			delt = root ["dt"].AsFloat;
		}
	}
	
	// Update is called once per frame
	void Update () {
		ballPosition [0, 0] = GameObject.Find("Sphere").transform.position.x;
		ballPosition [0, 1] = GameObject.Find("Sphere").transform.position.y;
		ballPosition [0, 2] = GameObject.Find("Sphere").transform.position.z;

		ballPosition [1, 0] = GameObject.Find("Sphere 1").transform.position.x;
		ballPosition [1, 1] = GameObject.Find("Sphere 1").transform.position.y;
		ballPosition [1, 2] = GameObject.Find("Sphere 1").transform.position.z;

		ballPosition [2, 0] = GameObject.Find("Sphere 2").transform.position.x;
		ballPosition [2, 1] = GameObject.Find("Sphere 2").transform.position.y;
		ballPosition [2, 2] = GameObject.Find("Sphere 2").transform.position.z;

		ballPosition [3, 0] = GameObject.Find("Sphere 3").transform.position.x;
		ballPosition [3, 1] = GameObject.Find("Sphere 3").transform.position.y;
		ballPosition [3, 2] = GameObject.Find("Sphere 3").transform.position.z;

		ballPosition [4, 0] = GameObject.Find("Sphere 4").transform.position.x;
		ballPosition [4, 1] = GameObject.Find("Sphere 4").transform.position.y;
		ballPosition [4, 2] = GameObject.Find("Sphere 4").transform.position.z;

		ballPosition [5, 0] = GameObject.Find("Sphere 5").transform.position.x;
		ballPosition [5, 1] = GameObject.Find("Sphere 5").transform.position.y;
		ballPosition [5, 2] = GameObject.Find("Sphere 5").transform.position.z;

		if (Input.GetKeyDown ("space")) {
			rotCAL [0] = rot [0];
			rotCAL [1] = rot [1];
			rotCAL [2] = rot [2];
		}

		if (Input.GetKeyDown ("1")) {
			client.Send ("{\"action\":1,\"LED\":[[100,0,100],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("2")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[100,0,100],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("3")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[100,0,100],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("4")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[100,0,100],[0,0,0],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("5")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[100,0,100],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("6")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[100,0,100],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("7")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[100,0,100],[0,0,0]]}~");
		} else if (Input.GetKeyDown ("8")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[100,0,100]]}~");
		} else if (Input.GetKeyDown ("9")) {
			client.Send ("{\"action\":1,\"LED\":[[100,0,100],[100,0,100],[100,0,100],[100,0,100],[100,0,100],[100,0,100],[100,0,100],[100,0,100]]}~");
		} else if (Input.GetKeyDown ("0")) {
			client.Send ("{\"action\":1,\"LED\":[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]]}~");
		} else if (Input.GetKeyDown (KeyCode.G)) {
			client.Send ("{\"action\":0,\"servo\":[100,100,100,100,100]}~");
			Thread.Sleep(750);
			client.Send ("{\"action\":0,\"servo\":[0,0,0,0,0]}~");
		}


		int [] rotF = new int[3];
		rotF [0] = rot [0] - rotCAL [0];
		rotF [1] = rot [1] - rotCAL [1];
		rotF [2] = rot [2] - rotCAL [2];

		for(int i=0;i<3;i++) {
			if(rotF[i]<0) rotF[i]=360+rotF[i];
		}

		hand.transform.localEulerAngles = new Vector3 (rotF[0], rotF[1], rotF[2]);

		hand.transform.Translate(acc[0], 0, acc[2]);

		//average
		float avg = (pot [0] + pot [1] + pot [2] + pot [3] + pot [4]) / 5;

		/*
		if (avg <= 0.4F) {
			dir.GetComponent<Light> ().color = Color.blue;
			//Thread.Sleep(100);
			
		}
		
		//Finger oben
		else if (avg () >= 0.5F){
			dir.GetComponent<Light> ().color = Color.green;
			//Thread.Sleep(100);
		}
		*/

		da.GetComponent<Animation> () ["Thumb"].time  =	pot [0];  	da.GetComponent<Animation> () ["Thumb"].speed = 0.0F;		da.GetComponent<Animation> ().Play ("Thumb");
		ze.GetComponent<Animation> () ["Pointer"].time=	pot [1];	ze.GetComponent<Animation> () ["Pointer"].speed = 0.0F;		ze.GetComponent<Animation> ().Play ("Pointer");
		mi.GetComponent<Animation> () ["Middle"].time =	pot [2];	mi.GetComponent<Animation> () ["Middle"].speed = 0.0F;		mi.GetComponent<Animation> ().Play ("Middle");
		ri.GetComponent<Animation> () ["Ringman"].time=	pot [3];	ri.GetComponent<Animation> () ["Ringman"].speed = 0.0F;		ri.GetComponent<Animation> ().Play ("Ringman");
		kl.GetComponent<Animation> () ["Pinky"].time  =	pot [4];	kl.GetComponent<Animation> () ["Pinky"].speed = 0.0F;		kl.GetComponent<Animation> ().Play ("Pinky");

		//getVelocity
		/*for (int i = 0; i < 3; i++ ) {
			if (acc [i] < 0.3)
				acc [i] = 0;

			v[i] = (old_acc[i] * delt - ((acc[i] - old_acc[i]) * delt)/2)/2000;
			vges[i] += v[i];
			t += delt;
			old_acc[i] = acc[i];
		}

		Debug.Log(vges[0] + ";" + vges[1] + ";" + vges[2]);

		hand.transform.position = new Vector3 (vges[0]*delt + offset[0],
		                                       vges[1]*delt + offset[1],
		                                       vges[2]*delt + offset[2]);
		*/
		//rigid.AddForce (new Vector3 (acc[0], acc[1], acc[2]), ForceMode.Acceleration);
		/*if (acc [0] < 0.1)
			acc [0] = 0;

		if (acc [1] < 0.1)
			acc [1] = 0;

		if (acc [2] < 0.1)
			acc [2] = 0;*/
		//rigid.velocity =(new Vector3 (acc[0]*delt, acc[1]*delt, acc[2]*delt));
	}

	/*	int demo ( int * num){
 	 * 		for (int i = 0; i < 3; ++i) {
 	 * 			&(num + i) = Math.Abs( &(num + i) ) < 3 ? 0 : &(num + i);
 	 * 		}
 	 * 	}
	 */

	void OnApplicationQuit(){
		sock.Close();
	}
}
