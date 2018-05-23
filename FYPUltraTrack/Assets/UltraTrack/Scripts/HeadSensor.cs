using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSensor : ArduinoDataSource {



	public Transform Sensor1, Sensor2, Sensor3, Hand1;
   
	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
        //distance1 = distanceHandToSensor (Sensor1, Hand1);

        //distance2 = distanceHandToSensor (Sensor2, Hand1);

        //distance3 = distanceHandToSensor (Sensor3, Hand1);


        RaycastHit hit1, hit2, hit3;

        if (Physics.Raycast(Sensor1.transform.position, Sensor1.transform.forward, out hit1, 100f))
        {
            distance1 = hit1.distance;
            Debug.DrawLine(Sensor1.transform.position, Sensor1.transform.position + (Sensor1.transform.forward * 100f), Color.red, 0.2f);
        }
        if (Physics.Raycast(Sensor2.transform.position, Sensor2.transform.forward, out hit2, 100f))
        {
            distance2 = hit2.distance;
            Debug.DrawLine(Sensor2.transform.position, Sensor2.transform.position + (Sensor2.transform.forward * 100f), Color.red, 0.2f);
        }
        if (Physics.Raycast(Sensor3.transform.position, Sensor3.transform.forward, out hit3, 100f))
        {
            distance3 = hit3.distance;
            Debug.DrawLine(Sensor3.transform.position, Sensor3.transform.position + (Sensor3.transform.forward * 100f), Color.red, 0.2f);
        }


    }

	public float distanceHandToSensor(Transform sensor, Transform hand){
		Debug.DrawLine (sensor.position, hand.position, Color.blue, 0.01f);

		return Vector3.Distance(sensor.position, hand.position);
	}

		

}
