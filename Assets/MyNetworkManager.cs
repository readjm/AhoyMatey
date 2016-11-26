﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public void MyStartHost()
    {
        StartHost();
    }

    public void MyStartClient()
    {
        StartClient();
    }

    public override void OnStartHost()
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Host start requested.");
    }

    public override void OnStartClient(NetworkClient nclient)
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Client start requested.");
        //base.OnStartClient(nclient);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Client connected to IP: " + conn.address);
        //base.OnClientConnect(conn);
    }
}