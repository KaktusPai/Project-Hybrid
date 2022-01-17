using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> doorSounds = new List<AudioClip>();
    private List<Animator> mechanicalDoors = new List<Animator>();
    private GameObject[] doorObjects;
    private List<bool> closedDoors = new List<bool>();
    private EnemyAgent enemy;

    //Endings UI
    [SerializeField] private Image deathBackgroundPanel;
    [SerializeField] private Image deathPanel;
    [SerializeField] private TextMeshProUGUI deathText;

    [SerializeField] private Image survivedBackgroundPanel;
    [SerializeField] private Image survivedPanel;
    [SerializeField] private TextMeshProUGUI survivedText;

    private SceneController sceneManager = new SceneController();

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

    public void PlayerDied()
    {
        deathBackgroundPanel.gameObject.SetActive(true);
        deathPanel.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);

        StartCoroutine(FadeDeath());
    }

    public void PlayerSurvived()
    {
        survivedBackgroundPanel.gameObject.SetActive(true);
        survivedPanel.gameObject.SetActive(true);
        survivedText.gameObject.SetActive(true);

        StartCoroutine(FadeSurvived());
    }

    private IEnumerator FadeSurvived()
    {
        // loop over 3 second
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            survivedBackgroundPanel.color = new Color(survivedBackgroundPanel.color.r, survivedBackgroundPanel.color.g, survivedBackgroundPanel.color.b, i / 3);
            survivedPanel.color = new Color(survivedPanel.color.r, survivedPanel.color.g, survivedPanel.color.b, i / 1.5f);
            survivedText.color = new Color(survivedText.color.r, survivedText.color.g, survivedText.color.b, i / 1.5f);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        sceneManager.loadMainScene();
    }

    private IEnumerator FadeDeath()
    {
        // loop over 3 second
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            deathBackgroundPanel.color = new Color(deathBackgroundPanel.color.r, deathBackgroundPanel.color.g, deathBackgroundPanel.color.b, i/3);
            deathPanel.color = new Color(deathPanel.color.r, deathPanel.color.g, deathPanel.color.b, i / 1.5f);
            deathText.color = new Color(deathText.color.r, deathText.color.g, deathText.color.b, i / 1.5f);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        sceneManager.loadMainScene();
    }

}
