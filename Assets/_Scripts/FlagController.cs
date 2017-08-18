using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    //Assign variables
    public bool updateOnStart = false;
    public SkinnedMeshRenderer currentFlag;
    public Material[] flags;

    void Start()
    {
        if (updateOnStart)
        {
            UpdateFlag();
        }
    }

    //public void SetFlag(int flag)
    //{
    //    Debug.Log("Setting flag. Index: " + flag);
    //    currentFlag.material = flags[flag];
    //}

    
    public void UpdateFlag()
    {
        int playerFlag = PlayerPrefsManager.GetPlayerFlag();
        currentFlag.material = flags[playerFlag];
    }
}
