using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoDataSource : MonoBehaviour
{

    public float distance1, distance2, distance3;
    internal float yaw, pitch, roll;
    internal Quaternion orientationChange = new Quaternion();
    public bool triggerPulled = false;
    public bool HasData { get; internal set; }
    internal string dataThisRound ="";
    internal string distanceData, gyroData, triggerData;


    [HideInInspector]
    public Quaternion OrientationChange { get { return orientationChange; } private set { } }
    

    public string[] vec3;

    internal float readdis1, readdis2, readdis3, readYaw, readPitch, readRoll;
    float[] tempdis1 = new float[10];
    float[] tempdis2 = new float[10];
    float[] tempdis3 = new float[10];
    int i;

    
    



    // Use this for initialization
    void Start()
    {
        
        //sp.Open();
        //sp.ReadTimeout = 1;
        i = 0;
    }

   
    internal  void collectData()
    {
        

        tempdis1[i] = readdis1;
        tempdis2[i] = readdis2;
        tempdis3[i] = readdis3;


        if (i >= 9)
        {
            i = 0;
            distance1 = GetMeanDistance(tempdis1);
            distance2 = GetMeanDistance(tempdis2);
            distance3 = GetMeanDistance(tempdis3);
        }
        else
        {

        }
        i++;

        orientationChange.eulerAngles = new Vector3(yaw, pitch, roll);
    }

   


    internal void SortAndSeperateData()
    {
        string[] splitAll = dataThisRound.Split('|');
        foreach (string t in splitAll)
        {
            if (t.StartsWith("D"))
            {
                distanceData = t;
            }
            else if (t.StartsWith("G"))
            {
                gyroData = t;
            }
            else if (t.StartsWith("F"))
            {
                triggerData = t;
            }
        }
        if (distanceData != null)
        {
            string[] splitDistances = distanceData.Split(';');
            readdis1 = float.Parse(splitDistances[1]);
            readdis2 = float.Parse(splitDistances[2]);
            readdis3 = float.Parse(splitDistances[3]);
            validateInput(ref readdis1, ref readdis2, ref readdis3);
        }


        if (gyroData != null)
        {
            string[] splitGyro = gyroData.Split(';');

            readYaw = float.Parse(splitGyro[1]);
            readPitch = float.Parse(splitGyro[2]);
            readRoll = float.Parse(splitGyro[3]);
        }
        if (triggerData != null)
        {
            string[] splitTrigger = triggerData.Split(';');
            //if (splitTrigger[1] == "1")
            //{
            //    triggerPulled = true;
            //}
            //else triggerPulled = false;
        }



        yaw = readYaw;
        pitch = readPitch;
        roll = readRoll;
        orientationChange.eulerAngles = new Vector3(yaw, pitch, roll);
    }

    private float GetMeanDistance(float[] distances)
    {
        if (distances.Length == 0)
            return 0f;
        float total = 0f;

        foreach (float d in distances)
        {
            total += d;

        }
        return (total / distances.Length);
    }

    

    float scaler = 0.1f;

    internal void validateInput(ref float tempdis1, ref float tempdis2, ref float tempdis3)
    {
        if (tempdis1 < 1000)
        {
            tempdis1 = tempdis1 * scaler;
        }
        else { tempdis1 = tempdis1 * scaler * 0.5f; }

        if (tempdis2 < 1000)
        {
            tempdis2 = tempdis2 * scaler;
        }
        else { tempdis2 = tempdis2 * scaler * 0.5f; }

        if (tempdis3 < 1000)
        {
            tempdis3 = tempdis3 * scaler;
        }
        else { tempdis3 = tempdis3 * scaler * 0.5f; }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        //sp.Close();
    }
}

