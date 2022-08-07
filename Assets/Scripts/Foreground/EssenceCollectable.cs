using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceCollectable : YarnCollectable
{
    public override void Collect()
    {
        print("essence");
        base.Collect();
    }
}
