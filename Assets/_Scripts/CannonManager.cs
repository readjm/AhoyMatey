using UnityEngine;
using System.Collections;

public class CannonManager : MonoBehaviour {

    public Cannon[] cannons;

    // Use this for initialization
    void Start()
    {
        cannons = GetComponentsInChildren<Cannon>();
    }
}
