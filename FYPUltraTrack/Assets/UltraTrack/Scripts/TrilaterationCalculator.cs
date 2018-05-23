using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrilaterationCalculator {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	/// <summary>
	/// 
	/// </summary>
	/// <param name="r1"></param>
	/// <param name="r2"></param>
	/// <param name="d"></param>
	/// <returns></returns>
	float xSolutionLocal (float r1, float r2, float d)
	{

		return (r1 * r1 - r2 * r2 + d * d) / (2 * d);
	}


	float ySolutionLocal(float r1, float r3, float i, float j, float x)
	{

		return ((r1 * r1 - r3 * r3 + i * i + j*j) / (2 * j)) - i*x/j;
	}

	float zSolutionLocalPositive (float r1, float x, float y){ 
		float z = r1 * r1 - x * x - y * y;
		if (Mathf.Abs (z) < 0.0000000001) {
			z = 0;
		}

		float za = (float)System.Math.Sqrt (z);

		if (za != za) 
		{
			return 0;
		} else 
		{
			return za;
		}


	}



	public Vector3 findIntersection(Vector3 pos1,float distance1, Vector3 pos2, float distance2, Vector3 pos3, float distance3)
	{
		Vector3 e_x = (pos2 - pos1).normalized;

		float i = Vector3.Dot(e_x , (pos3 - pos1));

		Vector3 e_y = (pos3 - pos1 - i * e_x).normalized;

		Vector3 e_z = Vector3.Cross(e_x, e_y);

		float d = Vector3.Distance(pos2, pos1);

		float j = Vector3.Dot(e_y, (pos3 - pos1));

		float x_l = xSolutionLocal(distance1, distance2, d);

		float y_l = ySolutionLocal(distance1, distance3, i, j, x_l);

		float z_l = zSolutionLocalPositive(distance1, x_l, y_l);
		//Debug.Log (z_l);

		return pos1 + e_x * x_l + e_y * y_l + e_z * z_l;

	}
}
