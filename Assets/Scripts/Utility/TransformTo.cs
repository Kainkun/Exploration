using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTo : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    
    public void Transform()
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
        transform.localScale = scale;
    }
}
