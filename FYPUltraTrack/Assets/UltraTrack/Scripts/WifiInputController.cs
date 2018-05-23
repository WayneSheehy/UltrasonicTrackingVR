using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class WifiInputController 
{
    public Int32 Port = 26;                                   // Port
    public string Host = "127.0.0.1";                           // Host IP or domain.
    public string myText = "";                                  // Line received from the TCP stream
    public Vector3 currentMeasures = new Vector3(0, 0, 0);
    public Vector3 currentRotation = new Vector3(0, 0, 0);      // Line converted into a Vector3 for Gyro
    public Vector3 currentForce = new Vector3(0, 0, 0);     // Line converted into a Vector3 for accelerometer
    public string readData = "";
    private bool socketReady = false;

    TcpClient client;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    // Opens the network TCP socket and starts read and write streams.
    public void setupSocket()
    {
        try
        {
            client = new TcpClient(Host, Port);
            theStream = client.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);

            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }


    public void Begin(string ipAddress, int port)
    {
        Host = ipAddress;
        Port = port;
        Thread connectThread = new Thread(new ThreadStart(setupSocket));
        //setupSocket();
        connectThread.Start();
        //Thread wifiTread = new Thread(() =>
        //{

        //    while (client.Connected)
        //    {
        //        string receivedText = readSocket();

        //        if (receivedText != "")
        //        {
        //            readData = receivedText;
        //            string[] splitText = myText.Split("|"[0]);


        //        }
        //    }

        //});

    }

    internal bool isConnected()
    {
        if (client != null)
            return client.Connected;
        else return false;
    }

    public void CheckForData()
    {
        //Debug.Log("ReadData");
        if (client != null)
        {
            if (client.Connected)
            {
                string receivedText = readSocket();
                theStream.Flush();
                
                if (receivedText != "")
                {
                    readData = receivedText;
                    //string[] splitText = myText.Split("|"[0]);
                    //Debug.Log("Data: " + readData);

                }
            }
        }

    }


    // Reads a line of text from the TCP stream if available
    public String readSocket()
    {
        if (!socketReady)
        {
            return "";
        }

        if (theStream.DataAvailable)
        {
            return theReader.ReadLine();
        }
        return "";
    }


    // Writes text through the stream over TCP
    public void writeSocket(string theLine)
    {
        if (!socketReady)
        {
            return;
        }

        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
    }

    // Converts String "Value,Value,Value" into Vector3
    Vector3 parseVector3(string source)
    {
        Vector3 outVector3;
        String[] splitString = new String[3];

        if (source == "nan,nan,nan")
        {
            Debug.Log("Error: Received nans");
            return currentRotation;
        }
        splitString = source.Split(";"[0]);

        outVector3.x = float.Parse(splitString[0]);
        outVector3.y = float.Parse(splitString[1]);
        outVector3.z = float.Parse(splitString[2]);

        return outVector3;
    }

    // Closes TCP connection and streams.
    public void closeSocket()
    {
        if (!socketReady)
        {
            return;
        }

        theWriter.Close();
        theReader.Close();
        client.Close();
        socketReady = false;
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }
}
