using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnSingletonCommands : SystemSingleton<YarnSingletonCommands>
{
    [YarnCommand("SetActiveFind")]
    public void SetActive(GameObject g, bool active = true)
    {
        g.SetActive(active);
    }

    public static Action<float> onEarnJobToken;

    [YarnCommand("EarnJobToken")]
    public void EarnJobToken(float amount)
    {
        YarnAccess.AddValue("jobTokenCount", amount);
        onEarnJobToken?.Invoke(amount);
    }
}