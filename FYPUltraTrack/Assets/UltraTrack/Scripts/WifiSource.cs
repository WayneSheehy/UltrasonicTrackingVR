using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiSource : ArduinoDataSource
{

    public UltraTrackManager tracker;
    




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tracker.isTrackerRunning())
        {
            dataThisRound = tracker.readFromWiFi();

            SortAndSeperateData();
            collectData();
            HasData = true;
        }
        else HasData = false;
        dataThisRound = "";
    }
}
