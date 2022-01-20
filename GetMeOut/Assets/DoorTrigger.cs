using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameManager gm;
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.gameObject.tag);
        gm.playerNextToDoor = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        gm.playerNextToDoor = false;
        Debug.Log("leve");
    }
}
