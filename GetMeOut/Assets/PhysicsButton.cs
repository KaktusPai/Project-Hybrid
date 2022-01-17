using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshHold = .1f;
    [SerializeField] private float deadZone = 0.025f;

    private bool isPressed = false;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    public UnityEvent onPressed, onReleased;
    void Start()
    {
        startPos = transform.position;
        joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        //Debug.Log(GetValue());
        if (!isPressed && GetValue() + threshHold >= 1) 
        {
            Pressed();
        }
        if(!isPressed && GetValue() - threshHold <= 0) 
        {
            Released();
        }
    }

    private float GetValue() 
    {
        var value = Vector3.Distance(startPos, transform.position) / joint.linearLimit.limit;
        Debug.Log(startPos + " " + transform.position + " " + Vector3.Distance(startPos, transform.position) / joint.linearLimit.limit);

        if (Math.Abs(value) < deadZone) 
        {
            value = 0;
            Debug.Log("Zeroed");
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed() 
    {
        isPressed = true;
        onPressed.Invoke();
        Debug.Log("Pressed " + gameObject.name);
    }

    private void Released() 
    {
        isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released " + gameObject.name);
    }
}
