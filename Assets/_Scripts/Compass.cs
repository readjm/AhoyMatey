using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

    GameObject needle;
    float heading;

    void Start()
    {
        needle = GameObject.Find("Needle");
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

        needle.transform.rotation = Quaternion.Euler(0f,0f,-heading);

        
    }

    public float GetHeading()
    {
        return heading;
    }
}
