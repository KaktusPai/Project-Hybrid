using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Animator> mechanicalDoors = new List<Animator>();
    private List<bool> closedDoors = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] doorObjects = GameObject.FindGameObjectsWithTag("MetalDoor");
        
        if(doorObjects != null)
        {
            foreach(GameObject door in doorObjects)
            {
                mechanicalDoors.Add(door.GetComponent<Animator>());
            }
        }

        if(mechanicalDoors != null)
        {
            foreach(Animator dooranim in mechanicalDoors)
            {
                closedDoors.Add(false);
            }
        }
    }

    //Open or close door based on ID in list
    public void ActivateDoor(int ID)
    {
        //Check if the doors actually exist to prevent crashing.
        if(mechanicalDoors != null && closedDoors != null)
        {
            //if the door is closed
            if (closedDoors[ID])
            {
                mechanicalDoors[ID].Play("Open Door");
                closedDoors[ID] = false;
            }
            else
            {
                mechanicalDoors[ID].Play("Close Door");
                closedDoors[ID] = true;
            }
        }
    }
}