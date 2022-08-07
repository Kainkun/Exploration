using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollectable : YarnCollectable
{
    public override void Collect()
    {
        print("trash");
        base.Collect();
    }
}