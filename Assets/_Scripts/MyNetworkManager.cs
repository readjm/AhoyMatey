using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(MyNetworkDiscovery))]
public class MyNetworkManager : NetworkManager
{
    private MyNetworkDiscovery localNetworkDiscovery;


    void start()
    {
        //localNetworkDiscovery = GetComponent<MyNetworkDiscovery>();
    }


    public void MyStartHost()
    {
        StopHost();
        //StartServer();
        //StartClient();
        StartHost();
    }


    public void MyStartClient()
    {
        StartClient();
    }


    public void StartLocalHost()
    {
        StopHost();
        //StartServer();
        //StartClient();
        StartHost();
        StartLocalBroadcast();
    }

    
    public void StartLocalClient()
    {
        StartReceiveLocalBroadcast();
    }

    public override void OnStartHost()
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Host start requested.");
    }


    public override void OnStartClient(NetworkClient nclient)
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Client start requested.");
        InvokeRepeating("ClientConnecting", 0f, 1f);
        //base.OnStartClient(nclient);
    }


    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log(Time.timeSinceLevelLoad + ": Client connected to IP: " + conn.address);
        CancelInvoke();
        //base.OnClientConnect(conn);
    }


    public void StartLocalBroadcast()
    {
        
        localNetworkDiscovery = GetComponent<MyNetworkDiscovery>();
        localNetworkDiscovery.Initialize();
        localNetworkDiscovery.StartAsServer();
    }


    public void StartReceiveLocalBroadcast()
    {
        localNetworkDiscovery = GetComponent<MyNetworkDiscovery>();
        localNetworkDiscovery.Initialize();
        localNetworkDiscovery.StartAsClient();
    }


    public void StopBroadcast()
    {
        localNetworkDiscovery.StopBroadcast();
    }


    public Dictionary<string, NetworkBroadcastResult> GetLocalMatches()
    {
        return localNetworkDiscovery.broadcastsReceived;
    }


    void ClientConnecting()
    {
        Debug.Log(".");
    }
}
