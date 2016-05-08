using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using MathNet.Numerics;

public class DeltaDefense : MonoBehaviour 
{
	//x,y,z enumerations
	enum Axis{X,Y,Z};
	//Contains output strings
	public ArrayList output;

	//Transform of the visualizing cube
	public Transform predicted;
	//public Transform actual;

	//History Size (# of nodes in the polynomial)
	private readonly int HIST_SIZE = 5;

	//Prediction Step
	private float predictionStep = .15f;

	//Delta Movement, for each node
	private readonly float DELTA_MOVE = .01f;
	private float dX;
	private float dY;
	private float dZ;

	//ArrayList containing the times and positions [t,f(t)] of each node
	private ArrayList xTime;
	private ArrayList yTime;
	private ArrayList zTime;

	private ArrayList xPos;
	private ArrayList yPos;
	private ArrayList zPos;

    private double time;
    private uint i;
	private const int HISTORY_SIZE = 2;
	private double[] t = new double[HISTORY_SIZE];
	private double[] x = new double[HISTORY_SIZE];
	private double[] y = new double[HISTORY_SIZE];
	private double[] z = new double[HISTORY_SIZE];
	private double[] rx = new double[HISTORY_SIZE];
	private double[] ry = new double[HISTORY_SIZE];
	private double[] rz = new double[HISTORY_SIZE];
	private double[] rw = new double[HISTORY_SIZE];
    private double future = 0;
	private const double FUTURE_FACTOR = 5;
	//for physics-based predictions
	private double oldDx, newDx;
	private double oldDy, newDy;
	private double oldDz, newDz;
	//rotations
	private double oldDrx, newDrx;
	private double oldDry, newDry;
	private double oldDrz, newDrz;
	private double oldDrw, newDrw;
	//velocity
	private double oldVx, newVx;
	private double oldVy, newVy;
	private double oldVz, newVz;
	//angular velocity
	private double oldVrx, newVrx;
	private double oldVry, newVry;
	private double oldVrz, newVrz;
	private double oldVrw, newVrw;
	//all accelerations and time
	private double Ax, Ay, Az, Arx, Ary, Arz, Arw, oldT;
	//array of HISTORY_SIZE accelerations
	private double [] AxArr = new double[HISTORY_SIZE];
	private double [] AyArr = new double[HISTORY_SIZE];
	private double [] AzArr = new double[HISTORY_SIZE];
	//array of HISTORY_SIZE angular accelerations
	private double [] ArxArr = new double[HISTORY_SIZE];
	private double [] AryArr = new double[HISTORY_SIZE];
	private double [] ArzArr = new double[HISTORY_SIZE];
	//Queue for history
	Queue<KeyValuePair<double, Vector3>> history;
	private double totalError = 0;
	private double maxError = 0;

    //Position in the previous frame
    private Vector3 positionOld;

	//Current predicted Vector
	private Vector3 predictedVector;

	//Transform to interpolate from
	public Transform enemyTransform;

	private List<String> saveToFile;


	public Vector3 currentEnemyPosition;
	public Vector3 previousEnemyPosition;

	private Boolean flag = false;
	private Boolean animationStarted = true;
	private const double DISTANCE_ALLOWED = 3;
	private double startTime;

	void Start () 
	{
		saveToFile = new List<String>();

		// initialize time and index globals
		time = 0;
		startTime = 2;
		i = 0;
		history = new Queue <KeyValuePair<double, Vector3>>();

		transform.position = currentEnemyPosition;

		// initialize values needed for physics based prediction
		//initializePhysicsBasedPrediction ();
			
		// initialize values needed for Brian's linear extrapolation
		//initializeLinearExtrapolation ();

		// initialize values for fifth degree polynomial extrapolation
		//initializeFifthPolynomial();

		//Application.targetFrameRate = 5;
	}

	void FixedUpdate ()
	{
		
		currentEnemyPosition = enemyTransform.position;
		if ((currentEnemyPosition - transform.position).magnitude > DISTANCE_ALLOWED) {
			//Debug.Log ("ARE YOU NUTS? Magnitude: " + (currentEnemyPosition - transform.position).magnitude);
			currentEnemyPosition = transform.position;
		} 
		time += Time.deltaTime;
		if (time == Time.deltaTime)
			future = FUTURE_FACTOR * Time.deltaTime;

		// Brian's linear extrapolation
		//linearExtrapolation ();

		linearRegression ();

		// based on Hogan 1984 paper on minimizing Jerk, resulting in 5th degree polynomial
		//fifthPolynomialPrediction ();

		// delta = v1*t + 1/2*a*t^2;
		//physicsBasedPrediction ();

		//noChange ();

		if (time <= 4 && time > 0.6)
			checkError ();
		else if (!flag && time >= 4) {
		//Debug.Log ("Total Error: " + Math.Sqrt (totalError) + " Max Error: " + maxError + " Average Error: " + (Math.Sqrt (totalError) / i));
			/*using(StreamWriter file = File.AppendText(transform.name + ".csv")){
				file.WriteLine(HISTORY_SIZE + "," + Math.Sqrt (totalError) + "," + maxError + "," + + (Math.Sqrt (totalError) / i));
			}*/
			//System.IO.File.WriteAllLines(transform.name + ".csv", saveToFile.ToArray());
			flag = true;
		}
		i++;
	}

	void initializeLinearExtrapolation ()
	{
		output = new ArrayList (50);

		xTime = new ArrayList (HIST_SIZE);
		yTime = new ArrayList (HIST_SIZE);
		zTime = new ArrayList (HIST_SIZE);
		xPos = new ArrayList (HIST_SIZE);
		yPos = new ArrayList (HIST_SIZE);
		zPos = new ArrayList (HIST_SIZE);
		dX = DELTA_MOVE;
		dY = DELTA_MOVE;
		dZ = DELTA_MOVE;
		positionOld = enemyTransform.localPosition;
	}

	void linearRegression(){
		//fifth degree polynomial 
		t[i % HISTORY_SIZE] = time;
		x[i % HISTORY_SIZE] = enemyTransform.localPosition.x;
		y[i % HISTORY_SIZE] = enemyTransform.localPosition.y;
		z [i % HISTORY_SIZE] = enemyTransform.localPosition.z;
		rx[i % HISTORY_SIZE]  = enemyTransform.localRotation.x;
		ry [i % HISTORY_SIZE] = enemyTransform.localRotation.y;
		rz [i % HISTORY_SIZE] = enemyTransform.localRotation.z;
		rw [i % HISTORY_SIZE] = enemyTransform.localRotation.w;

		//set new positions to current position values
		newDx = enemyTransform.localPosition.x;
		newDy = enemyTransform.localPosition.y;
		newDz = enemyTransform.localPosition.z;
		//set new rotations to current rotation values
		newDrx = enemyTransform.localRotation.x;
		newDry = enemyTransform.localRotation.y;
		newDrz = enemyTransform.localRotation.z;
		newDrw = enemyTransform.localRotation.w;

		//for velocity
		if (i >= 1) {	//i must have let at least one frame happen in order to have at least two positions
			//find current velocity
			newVx = findVelocity (newDx, oldDx, time, oldT);
			newVy = findVelocity (newDy, oldDy, time, oldT);
			newVz = findVelocity (newDz, oldDz, time, oldT);
			//find current angular velocity
			newVrx = findVelocity (newDrx, oldDrx, time, oldT);
			newVry = findVelocity (newDry, oldDry, time, oldT);
			newVrz = findVelocity (newDrz, oldDrz, time, oldT);
			newVrw = findVelocity (newDrw, oldDrw, time, oldT);
			//for acceleration
			if (i >= 2) {	//i must have let at least two frames happen in order to have at least two velocities
				/*
				Ax = findAcceleration (newVx, oldVx, time, oldT);
				Ay = findAcceleration (newVy, oldVy, time, oldT);
				Az = findAcceleration (newVz, oldVz, time, oldT);
				*/
				//fill acceleration arrays with current acceleration value
				AxArr[i%HISTORY_SIZE] = findAcceleration(newVx, oldVx, time, oldT);
				AyArr[i%HISTORY_SIZE] = findAcceleration(newVy, oldVy, time, oldT);
				AzArr[i%HISTORY_SIZE] = findAcceleration(newVz, oldVz, time, oldT);

				Ax = findAvgAcceleration (AxArr);
				Ay = findAvgAcceleration (AyArr);
				Az = findAvgAcceleration (AzArr);

				/*
				Arx = findAcceleration (newVrx, oldVrx, time, oldT);
				Ary = findAcceleration (newVry, oldVry, time, oldT);
				Arz = findAcceleration (newVrz, oldVrz, time, oldT);
				Arw = findAcceleration (newVrw, oldVrw, time, oldT);
				*/
				//fill angular acceleration arrays with current angular acceleration values
				ArxArr[i%HISTORY_SIZE] = findAcceleration(newVrx, oldVrx, time, oldT);
				AryArr[i%HISTORY_SIZE] = findAcceleration(newVry, oldVry, time, oldT);
				ArzArr[i%HISTORY_SIZE] = findAcceleration(newVrz, oldVrz, time, oldT);

				//Debug.Log (transform.name + " OLD: Arx: " + Arx + " Ary: " + Ary + " Arz: " + Arz);
				Arx = findAvgAcceleration (ArxArr);
				Ary = findAvgAcceleration (AryArr);
				Arz = findAvgAcceleration (ArzArr);
				//Debug.Log (transform.name + " NEW: Arx: " + Arx + " Ary: " + Ary + " Arz: " + Arz);
			}
		}

		if(Math.Abs(x[0]-x[1])<.01 ||  Math.Abs(y[0]-y[1])<.01 || Math.Abs(z[0]-z[1])<.01)


		//linear regression
		if (i >= HISTORY_SIZE) {
			Tuple<double, double> fx = Fit.Line (t, x);
			Tuple<double, double> fy = Fit.Line (t, y);
			Tuple<double, double> fz = Fit.Line (t, z);

			Tuple<double, double> frx = Fit.Line (t, rx);
			Tuple<double, double> fry = Fit.Line (t, ry);
			Tuple<double, double> frz = Fit.Line (t, rz);
			Tuple<double, double> frw = Fit.Line (t, rw);

			double x1 = 0, y1 = 0, z1 = 0, rx1 = 0, ry1 = 0, rz1 = 0, rw1 = 0;
			if(Math.Abs(x[0]-x[1])<.01)
				x1 = fx.Item1 + fx.Item2 * (time + future);
			else
				x1 = fx.Item1 + fx.Item2 * (time);
			if(Math.Abs(y[0]-y[1])<.01)
				y1 = fy.Item1 + fy.Item2 * (time + future);
			else
				y1 = fy.Item1 + fy.Item2 * (time);
			if(Math.Abs(z[0]-z[1])<.01)
				z1 = fz.Item1 + fz.Item2 * (time + future);
			else
				z1 = fz.Item1 + fz.Item2 * (time);
			rx1 = frx.Item1 + frx.Item2 * (time + future);
			ry1 = fry.Item1 + fry.Item2 * (time + future);
			rz1 = frz.Item1 + frz.Item2 * (time + future);
			rw1 = frw.Item1 + frw.Item2 * (time + future);


			//Debug.Log( (future - (Math.Abs(newVx)/50)));

			//			if (transform.name.Contains ("Robot_LeftToeBase"))
			//				Debug.Log ("t: " + time + " x1: " + x1 + " y1: " + y1 + " z1: " + z1 + "\n");
			transform.localPosition = new Vector3 ((float)x1, (float)y1, (float)z1);
			transform.localRotation = new Quaternion ((float)rx1, (float)ry1, (float)rz1, (float)rw1);
		}
	
	
	
	}




	void linearExtrapolation ()
	{
		//use positionOld
		dX -= Mathf.Abs (positionOld.x - currentEnemyPosition.x);
		dY -= Mathf.Abs (positionOld.y - currentEnemyPosition.y);
		dZ -= Mathf.Abs (positionOld.z - currentEnemyPosition.z);
		positionOld = currentEnemyPosition;
		float predX = predictedVector.x;
		float predY = predictedVector.y;
		float predZ = predictedVector.z;
		if (dX < 0) {
			xTime.Insert (0, Time.time);
			xPos.Insert (0, currentEnemyPosition.x);
			if (xPos.Count > HIST_SIZE) {
				xPos.Remove (HIST_SIZE);
				xTime.Remove (HIST_SIZE);
				predX = TwoPointLinearExtrapolation (((float)xTime [0]) + predictionStep, Axis.X);
				//Lagrange interpolation method
				//predX = F (((float)xTime [0]) + predictionStep, Axis.X);
			}
			dX = DELTA_MOVE;
		}
		if (dY < 0) {
			yTime.Insert (0, Time.time);
			yPos.Insert (0, currentEnemyPosition.y);
			if (yPos.Count > HIST_SIZE) {
				yPos.Remove (HIST_SIZE);
				yTime.Remove (HIST_SIZE);
				predY = TwoPointLinearExtrapolation (((float)yTime [0]) + predictionStep, Axis.Y);
				//Lagrange interpolation method
				//predY = F (((float)yTime [0]) + predictionStep, Axis.Y);
			}
			dY = DELTA_MOVE;
		}
		if (dZ < 0) {
			zTime.Insert (0, Time.time);
			zPos.Insert (0, currentEnemyPosition.z);
			if (zPos.Count > HIST_SIZE) {
				zPos.Remove (HIST_SIZE);
				zTime.Remove (HIST_SIZE);
				predZ = TwoPointLinearExtrapolation (((float)zTime [0]) + predictionStep, Axis.Z);
				//Lagrange interpolation method
				//predZ = F (((float)zTime [0]) + predictionStep, Axis.Z);
			}
			dZ = DELTA_MOVE;
		}
		predictedVector = new Vector3 (predX, predY, predZ);
		transform.rotation = enemyTransform.rotation;
		transform.position = Vector3.Lerp (transform.position, predictedVector, 0.25f) + new Vector3 (1, 0, 0);
		//output.Insert (0, +"\t"++"\t"++"\t"++"\t"++"\t");
	}

	void initializeFifthPolynomial()
	{
		// nothing needed here yet
	}

	void fifthPolynomialPrediction ()
	{
		//fifth degree polynomial 
		t[i % HISTORY_SIZE] = time;
		x[i % HISTORY_SIZE] = enemyTransform.localPosition.x;
		y[i % HISTORY_SIZE] = enemyTransform.localPosition.y;
		z [i % HISTORY_SIZE] = enemyTransform.localPosition.z;
		rx[i % HISTORY_SIZE]  = enemyTransform.localRotation.x;
		ry [i % HISTORY_SIZE] = enemyTransform.localRotation.y;
		rz [i % HISTORY_SIZE] = enemyTransform.localRotation.z;
		rw [i % HISTORY_SIZE] = enemyTransform.localRotation.w;

		//5th degree polynomial
		if (i >= HISTORY_SIZE) {
			double[] fx = Fit.Polynomial (t, x, 5);
			double[] fy = Fit.Polynomial (t, y, 5);
			double[] fz = Fit.Polynomial (t, z, 5);
			double[] frx = Fit.Polynomial (t, rx, 5);
			double[] fry = Fit.Polynomial (t, ry, 5);
			double[] frz = Fit.Polynomial (t, rz, 5);
			double[] frw = Fit.Polynomial (t, rw, 5);
			double tP = 1;
			double x1 = 0, y1 = 0, z1 = 0, rx1 = 0, ry1 = 0, rz1 = 0, rw1 = 0;
			for (int j = 0; j < 6; j++) {
				x1 += fx [j] * tP;
				y1 += fy [j] * tP;
				z1 += fz [j] * tP;
				rx1 += frx [j] * tP;
				ry1 += fry [j] * tP;
				rz1 += frz [j] * tP;
				rw1 += frw [j] * tP;
				tP *= (time + future);
			}
//			if (transform.name.Contains ("Robot_LeftToeBase"))
//				Debug.Log ("t: " + time + " x1: " + x1 + " y1: " + y1 + " z1: " + z1 + "\n");
			transform.localPosition = new Vector3 ((float)x1, (float)y1, (float)z1);
			transform.localRotation = new Quaternion ((float)rx1, (float)ry1, (float)rz1, (float)rw1);
		}
	}

	void initializePhysicsBasedPrediction ()
	{
		//oldPositions = initialPositions
		oldDx = enemyTransform.localPosition.x;
		newDx = oldVx = newVx = 0;
		oldDy = enemyTransform.localPosition.y;
		newDy = oldVy = newVy = 0;
		oldDz = enemyTransform.localPosition.z;
		newDz = oldVz = newVz = 0;
		//oldRotations = initialRotations
		oldDrx = enemyTransform.localRotation.x;
		newDrx = oldVrx = newVrx = 0;
		oldDry = enemyTransform.localRotation.y;
		newDry = oldVry = newVry = 0;
		oldDrz = enemyTransform.localRotation.z;
		newDrz = oldVrz = newVrz = 0;
		oldDrw = enemyTransform.localRotation.w;
		newDrw = oldVrw = newVrw = 0;

		//initialize accelArrays to 0
		for (int j = 0; j < HISTORY_SIZE; j++)
			AxArr[j] = AyArr[j] = AzArr[j] = 0;
		
		//initialize all accelerations to 0
		Ax = Ay = Az = Arx = Ary = Arz = Arw = oldT = 0;
	}

	void physicsBasedPrediction ()
	{
		//set new positions to current position values
		newDx = enemyTransform.localPosition.x;
		newDy = enemyTransform.localPosition.y;
		newDz = enemyTransform.localPosition.z;
		//set new rotations to current rotation values
		newDrx = enemyTransform.localRotation.x;
		newDry = enemyTransform.localRotation.y;
		newDrz = enemyTransform.localRotation.z;
		newDrw = enemyTransform.localRotation.w;

		//for velocity
		if (i >= 1) {	//i must have let at least one frame happen in order to have at least two positions
			//find current velocity
			newVx = findVelocity (newDx, oldDx, time, oldT);
			newVy = findVelocity (newDy, oldDy, time, oldT);
			newVz = findVelocity (newDz, oldDz, time, oldT);
			//find current angular velocity
			newVrx = findVelocity (newDrx, oldDrx, time, oldT);
			newVry = findVelocity (newDry, oldDry, time, oldT);
			newVrz = findVelocity (newDrz, oldDrz, time, oldT);
			newVrw = findVelocity (newDrw, oldDrw, time, oldT);
			if (i < 20 && transform.name.Contains ("Robot_LeftToeBase")) {
				//Debug.Log ("newDx: " + newDx + "oldDx: " + oldDx + "newVx: " + newVx + " newVY: " + newVy + " newVz: " + newVz);
			}
			//for acceleration
			if (i >= 2) {	//i must have let at least two frames happen in order to have at least two velocities
				/*
				Ax = findAcceleration (newVx, oldVx, time, oldT);
				Ay = findAcceleration (newVy, oldVy, time, oldT);
				Az = findAcceleration (newVz, oldVz, time, oldT);
				*/
				//fill acceleration arrays with current acceleration value
				AxArr[i%HISTORY_SIZE] = findAcceleration(newVx, oldVx, time, oldT);
				AyArr[i%HISTORY_SIZE] = findAcceleration(newVy, oldVy, time, oldT);
				AzArr[i%HISTORY_SIZE] = findAcceleration(newVz, oldVz, time, oldT);

				Ax = findAvgAcceleration (AxArr);
				Ay = findAvgAcceleration (AyArr);
				Az = findAvgAcceleration (AzArr);

				/*
				Arx = findAcceleration (newVrx, oldVrx, time, oldT);
				Ary = findAcceleration (newVry, oldVry, time, oldT);
				Arz = findAcceleration (newVrz, oldVrz, time, oldT);
				Arw = findAcceleration (newVrw, oldVrw, time, oldT);
				*/
				//fill angular acceleration arrays with current angular acceleration values
				ArxArr[i%HISTORY_SIZE] = findAcceleration(newVrx, oldVrx, time, oldT);
				AryArr[i%HISTORY_SIZE] = findAcceleration(newVry, oldVry, time, oldT);
				ArzArr[i%HISTORY_SIZE] = findAcceleration(newVrz, oldVrz, time, oldT);

				//Debug.Log (transform.name + " OLD: Arx: " + Arx + " Ary: " + Ary + " Arz: " + Arz);
				Arx = findAvgAcceleration (ArxArr);
				Ary = findAvgAcceleration (AryArr);
				Arz = findAvgAcceleration (ArzArr);
				//Debug.Log (transform.name + " NEW: Arx: " + Arx + " Ary: " + Ary + " Arz: " + Arz);

				//if (i < 20 && transform.name.Contains ("Robot_LeftToeBase")){
					//Debug.Log ("newAx: " + Ax + " newAy: " + Ay + " newAz: " + Az);
			}
		}

		double predictedTime = Time.deltaTime + future;		//time since last frame + future
		double timesquare = predictedTime * predictedTime;	//used for distance equation
		//(newV(x) * predictedTime     x = 1/2 at^2
		double deltaDx = (.5 * Ax * timesquare);
		double deltaDy = (.5 * Ay * timesquare);
		double deltaDz = (.5 * Az * timesquare);
		double deltaDrx = (.5 * Arx * timesquare);
		double deltaDry = (.5 * Ary * timesquare);
		double deltaDrz = (.5 * Arz * timesquare);
		double deltaDrw = (.5 * Arw * timesquare);

		//if (i<20 && transform.name.Contains("Robot_LeftToeBase")) Debug.Log ("newDx: " + newDx + " newDy: " + newDy + " newDz: " + newDz);
		//assign positions

		//check if difference is too great
		while (deltaDx > 1)
			deltaDx /= 2;
		while (deltaDy > 1)
			deltaDy /= 2;
		while (deltaDz > 1)
			deltaDz /= 2;
		
		//set position
		transform.localPosition = new Vector3 (
			enemyTransform.localPosition.x + (float)deltaDx, 
			enemyTransform.localPosition.y + (float)deltaDy, 
			enemyTransform.localPosition.z + (float)deltaDz);
		/*
		transform.localPosition.Set(
			enemyTransform.localPosition.x + (float)deltaDx, 
			enemyTransform.localPosition.y + (float)deltaDy, 
			enemyTransform.localPosition.z + (float)deltaDz);
		*/
		//set rotation
		transform.localRotation = new Quaternion (
			enemyTransform.localRotation.x + (float)deltaDrx, 
			enemyTransform.localRotation.y + (float)deltaDry, 
			enemyTransform.localRotation.z + (float)deltaDrz, 
			enemyTransform.localRotation.w + (float)deltaDrw);
		
		//assign old positions
		oldDx = newDx;
		oldDy = newDy;
		oldDz = newDz;
		//assign old rotations
		oldDrx = newDrx;
		oldDry = newDry;
		oldDrz = newDrz;
		oldDrw = newDrw;
		//assign old velocities
		oldVx = newVx;
		oldVy = newVy;
		oldVz = newVz;
		//assign old angular velocities
		oldVrx = newVrx;
		oldVry = newVry;
		oldVrz = newVrz;
		oldVrw = newVrw;
		//for deltaTime
		oldT = time;
	}

    float F(float t, Axis axis)
	{
		float sum = 0.0f;
		for (int i = 0; i < HIST_SIZE; i++)
		{
			if (axis == Axis.X) 
			{
				sum += (((float)xPos[i]) * Lagrange (t,i,axis));
			}
			else if (axis == Axis.Y) 
			{
				sum += (((float)yPos[i]) * Lagrange(t,i,axis));
			}
			else if (axis == Axis.Z) 
			{
				sum += (((float)zPos[i]) * Lagrange (t,i,axis));
			}
		}

		return sum;

	}

	float Lagrange(float x, int i, Axis axis)
	{
		float product = 1.0f;

		for (int j = 0; j < HIST_SIZE; j++) 
		{
			if(i != j)
			{
				if (axis == Axis.X) 
				{
					product *= ((x - ((float)xTime[j]))/( ((float)xTime[i]) - ((float)xTime[j])));
				}
				else if (axis == Axis.Y) 
				{
					product *= ((x - ((float)yTime[j]))/( ((float)yTime[i]) - ((float)yTime[j])));
				}
				else if (axis == Axis.Z) 
				{
					product *= ((x - ((float)zTime[j]))/( ((float)zTime[i]) - ((float)zTime[j])));
				}

			}
		}

		return product;
	}

	float TwoPointLinearExtrapolation(float x, Axis axis)
	{
		float prediction = 0.0f;
		float x1 = 0.0f, x2 = 0.0f, y1 = 0.0f, y2 = 0.0f;

		if (axis == Axis.X) 
		{
			y1 = (float) xPos [1];
			y2 = (float) xPos [0];

			x1 = (float) xTime [1];
			x2 = (float) xTime [0];
		}
		else if (axis == Axis.Y) 
		{

			y1 = (float) yPos [1];
			y2 = (float) yPos [0];

			x1 = (float) yTime [1];
			x2 = (float) yTime [0];
		}
		else if (axis == Axis.Z) 
		{

			y1 = (float) zPos [1];
			y2 = (float) zPos [0];

			x1 = (float) zTime [1];
			x2 = (float) zTime [0];
		}
		prediction = y1 + (((x - x1) / (x2 - x1)) * (y2 - y1));

		return prediction;
	}

	void writeOut()
	{
		String filename = "whatever.txt";
		if (File.Exists (filename)) 
		{
			return;
		}


		/*var sr = File.CreateText(filename);
		for (int i = 0; i < output.Count; i++) 
		{
			sr.WriteLine ((String) output [i]);
		}
		sr.Close();
        */
	}

	//for physics-based
	private double findVelocity(double newD, double oldD, double newT, double olDT){
		return (newD - oldD) / (newT - oldT);
	}
	private double findAcceleration(double newV, double oldV, double newT, double oldT){
		return (newV - oldV) / (newT - oldT);
	}
	private double findAvgAcceleration(double [] accelerationArr){
		double sum = 0;
		for (int j = 0; j < accelerationArr.Length; j++)
			sum += accelerationArr [j];
		return sum / accelerationArr.Length;
	}

	private void noChange(){
		transform.localPosition = enemyTransform.localPosition;
		transform.localRotation = enemyTransform.localRotation;
	}

	private void checkError(){
		history.Enqueue (new KeyValuePair<double, Vector3>(time + future, transform.position));

		if (history.Count > FUTURE_FACTOR) {
			KeyValuePair<double, Vector3> temp = history.Dequeue ();
			Vector3 historicalPosition = temp.Value;
			double historyTime = temp.Key;

			if (transform.name.Contains("Robot_LeftToeBase"))
				//Debug.Log ("i: " + i + " Difference: " + (currentEnemyPosition - historicalPosition).magnitude);
			
			saveToFile.Add (Time.deltaTime + "," + time + "," + historicalPosition.x + "," + currentEnemyPosition.x + ","+ 
							historicalPosition.y + "," + currentEnemyPosition.y + "," + 
							historicalPosition.z + "," + currentEnemyPosition.z + "," +
							Ax + "," + Ay + "," + Az + "," + newVx + "," + newVy + "," + newVz);
			

			//max error calc
			if ((currentEnemyPosition - historicalPosition).magnitude > maxError) {
				maxError = (currentEnemyPosition - historicalPosition).magnitude;
			//	if(transform.name.Contains("Robot_LeftToeBase"))
			//		Debug.Log ("new MaxError" + i);
			}

			totalError += Mathf.Pow ((currentEnemyPosition - historicalPosition).magnitude, 2);

			//saveToFile.Add (HISTORY_SIZE + "," + maxError + "," + totalError);
			//if(transform.name.Contains("Robot_RightArm"))
				//Debug.Log (HISTORY_SIZE + "," + maxError + "," + totalError);
		}
	}

}