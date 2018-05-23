using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltrasonicScanner : MonoBehaviour {
    public float yRotation { get; private set; }
    public float xRotation { get; private set; }
    public float timeLeft { get; private set; }

    public ArduinoDataSource Source;
    public Transform SensorR, SensorM, SensorL;
    Vector3 contactPt1, contactPt2, contactPt3;
    GameObject plane;
    private Vector3 gyroChange;
    private Vector3 lastOrientation;
    

    // Use this for initialization
    void Start () {
        Input.gyro.enabled = true;
        
        plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        timeLeft = 5f;
    }

    GameObject cube;

    // Update is called once per frame
    void Update () {


        
        gyroChange = lastOrientation - Source.OrientationChange.eulerAngles;
        lastOrientation = Source.OrientationChange.eulerAngles;

        //transform.eulerAngles = transform.rotation * (gyroChange);
        //transform.Rotate(gyroChange );

        yRotation += -Input.gyro.rotationRateUnbiased.y;
        xRotation += -Input.gyro.rotationRateUnbiased.x;

        //yRotation += -gyroChange.y;
        //xRotation += -gyroChange.x;

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);

        contactPt1 = SensorR.position + SensorR.forward * Source.distance1;
        contactPt2 = SensorM.position + SensorM.forward * Source.distance2;
        contactPt3 = SensorL.position + SensorL.forward * Source.distance3;


         timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            Destroy(plane);
            Destroy(cube);
            plane = GameObject.CreatePrimitive(PrimitiveType.Quad);

            plane.transform.position = contactPt2;
            plane.transform.forward = (new Plane(contactPt1, contactPt2, contactPt3).normal);
            plane.transform.GetComponent<Renderer>().material.color = Color.blue;
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localPosition = plane.transform.localPosition + (plane.transform.forward * (cube.transform.lossyScale.z * 0.5f ));
            
            cube.transform.forward = plane.transform.forward;
            cube.transform.GetComponent<Renderer>().material.color = Color.green;
            timeLeft = 0.5f; 
        }

        Debug.DrawLine(SensorR.position, contactPt1, Color.blue);
        Debug.DrawLine(SensorM.position, contactPt2, Color.blue);
        Debug.DrawLine(SensorL.position, contactPt3, Color.blue);


    }

}

