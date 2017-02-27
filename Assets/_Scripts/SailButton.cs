using UnityEngine;
using System.Collections;

public class SailButton : MonoBehaviour
{
    public void RaiseSail()
    {
        foreach (Player ship in GameObject.FindObjectsOfType<Player>())
        {
            if (ship.isLocalPlayer)
            {
                ship.RaiseSail();
            }
        }
    }

    public void LowerSail()
    {
        foreach (Player ship in GameObject.FindObjectsOfType<Player>())
        {
            if (ship.isLocalPlayer)
            {
                ship.LowerSail();
            }
        }
    }
}
