using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

public class FallingElevatorCart : MonoBehaviour
{
    public SlidingDoor slidingDoor;

    public void OpenDoor()
    {
        print("ding");
        slidingDoor.doorState = SlidingDoor.DoorState.Open;
    }

    public void Fall()
    {
        GetComponent<Animator>().Play("FallingElevatorCart");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }


    public ShakePreset shake;
    private ShakeInstance shakeInstance;
    public void StartShake()
    {
        shakeInstance = Shaker.ShakeAll(shake);
    }

    private void EndShake()
    {
        shakeInstance.Stop(0, true);
    }

    public ShakePreset explosion;
    public void Crash()
    {
        EndShake();
        Shaker.ShakeAll(explosion);
    }
}