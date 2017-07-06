using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannonButton : MonoBehaviour {

    public Slider cannonTimerBar;

    float cannonTimer = 0f;

    void Update()
    {
        if (cannonTimer > 0f)
        {
            cannonTimer -= Time.deltaTime;
            cannonTimerBar.value = 1 - (cannonTimer / 3f);
        }

        cannonTimer = Mathf.Clamp(cannonTimer, 0f, 3f);
    }

    public void FirePort()
    {
        if (cannonTimer == 0f)
        {
            foreach (Player player in GameObject.FindObjectsOfType<Player>())
            {
                if (player.isLocalPlayer)
                {
                    player.CmdFirePort();
                    cannonTimer = 3f;
                }
            }
        }
    }

    public void FireStbd()
    {
        if (cannonTimer == 0)
        {
            foreach (Player player in GameObject.FindObjectsOfType<Player>())
            {
                if (player.isLocalPlayer)
                {
                    player.CmdFireStarboard();
                    cannonTimer = 3f;
                }
            }
        }
    }
}