using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public GameObject serverElement;
    public GameObject ipInputField;

    private Toggle[] tabs;
    private MyNetworkManager myNetworManager;

    // Use this for initialization
    void Start()
    {
        tabs = GetComponentsInChildren<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisplayLocalTab()
    {
        myNetworManager.StartReceiveLocalBroadcast();
        
    }

    private void DisplayInternetTab()
    {
        myNetworManager.StartMatchMaker();
    }

    private void DisplayIPTab()
    {

    }

}
