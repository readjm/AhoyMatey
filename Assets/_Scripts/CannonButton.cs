using UnityEngine;
using System.Collections;

public class CannonButton : MonoBehaviour {

	public void FirePort()
    {
        

        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            if (player.isLocalPlayer)
            {
                player.CmdFirePort();
            }
        }
    }

    public void FireStbd()
    {
        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            if (player.isLocalPlayer)
            {
                player.CmdFireStarboard();
            }
        }
    }
}