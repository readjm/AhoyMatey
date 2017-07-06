using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Material[] flags;

    [SyncVar]
    public float windDirection; // { get; private set; }

    [SyncVar]
    public float windSpeed; //     { get; private set; }

    private float windTimer = 0f;

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            windDirection = Random.Range(0f, 359.9f);
        }
        windSpeed = 10;
    }
    //windDirection = Random.rotationUniform.z;
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        windTimer += Time.deltaTime;
        
        if (windTimer >= 30f)
        {
            windTimer = 0f;
            Debug.Log("Wind timer reset.");

            if (Random.Range(0f, 1f) <= .25f)
            {
                windDirection = Random.rotation.eulerAngles.x;
            }

            Debug.Log("New wind direction: " + windDirection);
        } 
    }

    [Command]
    public void CmdUpdateFlags()
    {
        Debug.Log("Updating flags.");
        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            //player.RpcUpdateFlag();
        } 
    }
}
