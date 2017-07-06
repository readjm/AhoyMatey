using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {

    public SkinnedMeshRenderer currentFlag;
    public Material[] flags;
        
    public void SetFlag(int flag)
    {
        Debug.Log("Setting flag. Index: " + flag);
        currentFlag.material = flags[flag]; 
    }
}
