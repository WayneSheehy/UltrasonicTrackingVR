using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltraTrackManager : MonoBehaviour {


    WifiInputController HeadsetInput, ControllerInput ;
    
    string HeadsetIp = "192.168.137.69";
    string ControllerIp = "192.168.137.137";
    public InputField HeadsetIpField;
    public InputField ControllerIpField;
    private float syncTimer;
    bool headsetConnected = false;
    bool controllerConnected = false;

    // Use this for initialization
    void Start () {

        HeadsetInput = new WifiInputController();
        ControllerInput = new WifiInputController();
        HeadsetIpField.text = HeadsetIp;
        ControllerIpField.text = ControllerIp;




        

    }
    public void ConnectToHeadset()
    {
        HeadsetIp = HeadsetIpField.text;
        
        HeadsetInput.Begin(HeadsetIp, 26);
    }

    public void ConnectToController()
    {
        ControllerIp = ControllerIpField.text;
        
        ControllerInput.Begin(ControllerIp, 27);
    }

    public string readFromWiFi()
    {
        return HeadsetInput.readData + ControllerInput.readData;
    }

    public bool isTrackerRunning()
    {
        return headsetConnected ;
    }

    // Update is called once per frame
    void Update () {
        if (HeadsetInput != null)
        {
            headsetConnected = HeadsetInput.isConnected();
            if (headsetConnected)
            {
                HeadsetInput.CheckForData();
            }
        }
        if (ControllerInput != null)
        {
            controllerConnected = ControllerInput.isConnected();
            if (controllerConnected)
            {
                ControllerInput.CheckForData();
            }
        }
        
        
        
        Debug.Log("Wifi Data: "+ readFromWiFi());

        if(syncTimer < 0)
        {
            syncTimer = 0.05f;
           // HeadsetInput.writeSocket("T");
            //ControllerInput.writeSocket("T");

        }
        syncTimer -= Time.deltaTime;


	}
}
