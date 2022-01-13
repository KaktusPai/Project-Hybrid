using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public GameObject chair;
    public float fixedHeight = 1f;

    void Update()
    {
        transform.position = new Vector3(chair.transform.position.x, fixedHeight, chair.transform.position.z);
    }
}
