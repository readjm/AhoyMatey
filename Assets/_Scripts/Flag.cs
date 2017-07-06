using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(Cloth))]

public class Flag : MonoBehaviour
{
    public void SetFlag(Material flag)
    {
        GetComponent<SkinnedMeshRenderer>().material = flag;
    }
}
