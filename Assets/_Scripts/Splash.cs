using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	
	void Start ()
    {
        Invoke("Destroy", 1f);
	}
	
    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
