using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

    public GameObject needle;

    private GameObject desiredHeading;
    private Player localPlayer;
    private float heading;

    void Start()
    {
        needle = GameObject.Find("Needle");
        desiredHeading = GameObject.Find("Desired Heading");

        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player;
            }
        }
    }

    void Update()
    {
        needle.transform.rotation = Quaternion.Euler(0, 0, -localPlayer.transform.eulerAngles.y);
    }

    public void SetCompass()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 compassCenter = new Vector2(transform.position.x, transform.position.y);
        Vector2 vectorDifference = mousePosition - compassCenter;
        heading = (Mathf.Atan2(-vectorDifference.y, vectorDifference.x) * Mathf.Rad2Deg) + 90;

        if (heading < 0)
        {
            heading += 360;
        }

        desiredHeading.transform.rotation = Quaternion.Euler(0f,0f,-heading);        
    }

    public float GetHeading()
    {
        return heading;
    }
}
