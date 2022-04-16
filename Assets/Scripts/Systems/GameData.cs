using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class GameData
{
    public static LayerMask playerMask;
    public static LayerMask defaultGroundMask;
    public static LayerMask platformMask;
    public static LayerMask traversableMask;
    
    public void SetData()
    {
        playerMask = LayerMask.GetMask("Player");
        defaultGroundMask = LayerMask.GetMask("Default");
        platformMask = LayerMask.GetMask("Platform");
        traversableMask =  defaultGroundMask | platformMask;
    }
}