using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class LibraryMachine : MonoBehaviour
{
    LibraryMachineRing[] rings;

    void Start()
    {
        rings = new[]
        {
            new LibraryMachineRing(transform.GetChild(0).Find("RadialClonerInner"), 0),
            new LibraryMachineRing(transform.GetChild(0).Find("RadialClonerOuter"), 1)
        };

        print(Print());
    }

    [YarnCommand("SetPreset")]
    public void SetPreset(string presetName)
    {
        switch (presetName)
        {
            case "allDown":
                SetState("0:0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\n"
                         + "10:0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\n");
                break;
            case "allUp":
                SetState("10:9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9\n"
                         + "0:9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9\n");
                break;
            case "pretty":
                SetState("6:3,2,3,2,3,2,0,0,2,3,3,2,0,0,2,3,2,3,2,3\n"
                         + "2:6,8,6,8,6,8,6,0,6,8,6,8,6,0,6,8,6,8,6,8\n");
                break;
            case "climbable":
                SetState("4:2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2\n" +
                         "7:2,2,2,5,5,6,7,7,2,2,2,2,2,2,2,2,2,2,2,2\n");
                break;
            case "wave":
                SetState("0:1,2,3,4,5,6,7,6,5,4,3,2,1,2,3,4,5,6,7,8\n" +
                         "10:1,2,3,4,5,6,7,6,5,4,3,2,1,2,3,4,5,6,7,8\n");
                break;
            
        }
    }

    private void Update()
    {
        foreach (LibraryMachineRing libraryMachineRing in rings)
            libraryMachineRing.Update();
    }

    public void SetState(string s)
    {
        int index = 0;

        for (int x = 0; x < rings.Length; x++)
        {
            string rotation = "";
            while (s[index] != ':' && index < s.Length)
            {
                rotation += s[index];
                index++;
            }

            index++;


            print("rot" + x + ":" + rotation);
            rings[x].SetRotation(int.Parse(rotation));

            for (int y = 0; y < rings[x].shelveStacks.Length; y++)
            {
                string height = "";
                while (s[index] != ',' && s[index] != '\n' && index < s.Length)
                {
                    height += s[index];
                    index++;
                }

                index++;

                rings[x].shelveStacks[y].SetHeight(int.Parse(height));
            }
        }
    }

    public string Print()
    {
        string s = "";
        for (int x = 0; x < rings.Length; x++)
        {
            s += rings[x].targetRotationIndex + ":";
            s = rings[x].shelveStacks.Aggregate(s, (current, stack) => current + (stack.targetHeightIndex + ","));

            s = s.Substring(0, s.Length - 1);
            s += "\n";
        }

        return s;
    }
}

public class LibraryMachineRing
{
    public LibraryMachineShelfStack[] shelveStacks;

    public int targetRotationIndex;

    private Transform transform;
    private float yAxisOffset;
    private const int shelfStackCount = 20;
    private float shelfStackAngle;
    private float yAxisVelocity;
    private float targetYAxis;
    private const float smoothTime = 1f;
    private const float maxSpeed = 25f;

    public LibraryMachineRing(Transform transform, int index)
    {
        shelfStackAngle = 360f / shelfStackCount;

        this.transform = transform;
        shelveStacks = new LibraryMachineShelfStack[transform.childCount];
        for (int i = 0; i < shelveStacks.Length; i++)
            shelveStacks[i] = new LibraryMachineShelfStack(transform.GetChild(i), i);

        yAxisOffset = transform.localEulerAngles.y;

        transform.name = "LibraryMachineRing_" + index;

        SetRotation(Mathf.FloorToInt((transform.eulerAngles.y / (360f / shelfStackCount))));
    }

    public void SetRotation(int rotation)
    {
        targetRotationIndex = rotation % shelfStackCount;
        targetYAxis = (targetRotationIndex * (360f / shelfStackCount)) + yAxisOffset;
    }

    public void Update()
    {
        Vector3 eulerAngles = transform.localEulerAngles;
        float newAngle = Mathf.SmoothDampAngle(eulerAngles.y, targetYAxis, ref yAxisVelocity, smoothTime, maxSpeed,
            Time.deltaTime);
        transform.localRotation = Quaternion.Euler(eulerAngles.x, newAngle, eulerAngles.z);

        foreach (LibraryMachineShelfStack libraryMachineShelfStack in shelveStacks)
            libraryMachineShelfStack.Update();
    }
}

public class LibraryMachineShelfStack
{
    public int targetHeightIndex;

    private Transform transform;
    private Vector3 velocity;
    private float targetHeight;
    private const float shelfHeight = 1.0f;
    private const int shelfCount = 9;
    private const float smoothTime = 1f;
    private const float maxSpeed = 5f;

    public LibraryMachineShelfStack(Transform transform, int index)
    {
        this.transform = transform;

        float currentHeight =
            Utility.Remap(-(shelfHeight * shelfCount), 0, 0, shelfCount, transform.localPosition.y);
        SetHeight(Mathf.FloorToInt(currentHeight));

        transform.name = "LibraryMachineStack_" + index;
    }

    public void SetHeight(int height)
    {
        targetHeightIndex = Mathf.Clamp(height, 0, shelfCount);
        targetHeight = Utility.Remap(0, shelfCount, -(shelfHeight * shelfCount), 0, targetHeightIndex);
    }

    public void Update()
    {
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            new Vector3(0, targetHeight, 0),
            ref velocity, smoothTime, maxSpeed, Time.deltaTime);
    }
}