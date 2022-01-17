using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTesting : MonoBehaviour
{
    [SerializeField] private List<Animator> animators = new List<Animator>();
    private List<bool> doorsClosed = new List<bool>();
    [SerializeField] private EnemyAgent enemy;
    [SerializeField] private List<GameObject> doors = new List<GameObject>();

    [SerializeField] private List<AudioClip> doorSounds = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Animator anim in animators)
        {
            doorsClosed.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (doorsClosed[0]) //its true (closed)
            {
                animators[0].Play("Open Door");

                int randomInt = Random.Range(0, doorSounds.Count);

                doors[0].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);

                doorsClosed[0] = false;
            }
            else
            {
                animators[0].Play("Close Door");

                int randomInt = Random.Range(0, doorSounds.Count);

                doors[0].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);

                enemy.AlertDoorClosed(doors[0].transform);
                doorsClosed[0] = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (doorsClosed[1]) //its true (closed)
            {
                animators[1].Play("Open Door");

                int randomInt = Random.Range(0, doorSounds.Count);

                doors[1].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);

                doorsClosed[1] = false;
            }
            else
            {
                animators[1].Play("Close Door");

                int randomInt = Random.Range(0, doorSounds.Count);

                doors[1].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);

                enemy.AlertDoorClosed(doors[1].transform);
                doorsClosed[1] = true;
            }
        }
    }
}
