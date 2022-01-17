using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> doorSounds = new List<AudioClip>();
    private List<Animator> mechanicalDoors = new List<Animator>();
    private GameObject[] doorObjects;
    private List<bool> closedDoors = new List<bool>();
    private EnemyAgent enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAgent>();

        doorObjects = GameObject.FindGameObjectsWithTag("MetalDoor");
        
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

                int randomInt = Random.Range(0, doorSounds.Count);

                doorObjects[ID].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);
            }
            else
            {
                mechanicalDoors[ID].Play("Close Door");
                enemy.AlertDoorClosed(doorObjects[ID].transform); //enemy detects that a door has been closed
                closedDoors[ID] = true;

                int randomInt = Random.Range(0, doorSounds.Count);

                doorObjects[ID].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);
            }
        }
    }
}
