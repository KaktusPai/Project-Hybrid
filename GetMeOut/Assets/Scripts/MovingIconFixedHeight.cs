using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingIconFixedHeight : MonoBehaviour
{
    public Transform targetToFloatAbove;
    public float fixedHeight = 12f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(targetToFloatAbove.position.x, fixedHeight, targetToFloatAbove.position.z);
        transform.eulerAngles = new Vector3(-90f, 0f, targetToFloatAbove.eulerAngles.y);
    }
}
