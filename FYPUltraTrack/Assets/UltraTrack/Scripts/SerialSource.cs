using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SerialSource : ArduinoDataSource
{


    SerialPort headSerial = new SerialPort("COM5", 115200);
    SerialPort gunSerial = new SerialPort("COM9", 115200);

    // Use this for initialization
    void Start()
    {
        headSerial.Open();
        headSerial.ReadTimeout = 1;
        gunSerial.Open();
        gunSerial.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            dataThisRound += headSerial.ReadLine();
            dataThisRound += gunSerial.ReadLine();
        }
        catch (System.Exception)
        {
            Debug.Log("Serial unable to read");
        }
        
                
                SortAndSeperateData();
                collectData();
                HasData = true;
            

        
        Debug.Log("Serial Data: " + dataThisRound);
        dataThisRound = "";
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        headSerial.Close();
        gunSerial.Close();
    }
}
