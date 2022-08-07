using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Buffer
{
    public delegate void Action();

    public delegate bool Check();

    private Action action;
    private Check check;

    private bool queued;
    public float timeSinceQueue;
    private float maxTime;

    public Buffer(Action action, Check check, float maxTime)
    {
        this.action = action;
        this.check = check;
        this.maxTime = maxTime;
    }

    public void Queue()
    {
        queued = true;
        timeSinceQueue = 0;
    }

    public void Tick(float deltaTime)
    {
        if (queued && check.Invoke())
        {
            action.Invoke();
            queued = false;
        }

        timeSinceQueue += deltaTime;
        if (timeSinceQueue > maxTime)
            queued = false;
    }
}