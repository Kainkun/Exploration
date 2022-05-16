using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnSimpleCommands : MonoBehaviour
{
    public GameObject toSetActive;
    
    [YarnCommand("SetActiveTrue")]
    public void SetActiveTrue()
    {
        toSetActive.SetActive(true);
    }
    
    [YarnCommand("SetActiveFalse")]
    public void SetActiveFalse()
    {
        toSetActive.SetActive(false);
    }
}
