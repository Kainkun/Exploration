using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnSimpleCommands : MonoBehaviour
{
    public GameObject[] gameObjects;
    public UnityEvent unityEvent;

    [YarnCommand("SetActive")]
    public void SetActive(bool active = true)
    {
        gameObjects[0].SetActive(active);
    }

    [YarnCommand("SetActiveIndex")]
    public void SetActive(int n, bool active = true)
    {
        gameObjects[n].SetActive(active);
    }

    [YarnCommand("SetActiveName")]
    public void SetActive(string name, bool active = true)
    {
        foreach (GameObject g in gameObjects)
        {
            if (g.name == name)
                g.SetActive(active);
        }
    }

    [YarnCommand("SetActiveFind")]
    public void SetActive(GameObject g, bool active = true)
    {
        g.SetActive(active);
    }

    [YarnCommand("InvokeEvent")]
    public void InvokeEvent()
    {
        unityEvent?.Invoke();
    }
}