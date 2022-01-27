using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> doorSounds = new List<AudioClip>();
    private List<Animator> mechanicalDoors = new List<Animator>();
    [SerializeField] private GameObject[] doorObjects;
    [SerializeField] private List<bool> closedDoors = new List<bool>();
    private EnemyAgent enemy;

    private float maxOpenDoorTime = 15f;
    [SerializeField] private AudioClip doorWarning;

    //Endings UI
    [SerializeField] private Image deathBackgroundPanel;
    [SerializeField] private Image deathPanel;
    [SerializeField] private TextMeshProUGUI deathText;

    [SerializeField] private Image survivedBackgroundPanel;
    [SerializeField] private Image survivedPanel;
    [SerializeField] private TextMeshProUGUI survivedText;

    private SceneController sceneManager = new SceneController();

    [SerializeField] private List<MeshRenderer> doorLights = new List<MeshRenderer>();
    [SerializeField] private Material lightOn;
    [SerializeField] private Material lightOff;
    [SerializeField] private List<bool> buttonsPressed = new List<bool>();
    [SerializeField] private int numberOfButtons = 6;

    [SerializeField] private SphereCollider exitDoorTrigger;
    public bool playerAimingAtButton;
    public bool surviveOnce = false;
    public bool playerNextToDoor = false;
    public TMP_Text lightText;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAgent>();
        //Debug.Log(numberOfButtons);
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
        
        for(int i = 0; i < numberOfButtons; i++)
        {
            buttonsPressed.Add(false);
        }
        exitDoorTrigger.enabled = false;
        lightText.text = "Puzzels solved: 0";
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
                Debug.Log("Opened door: " + doorObjects[ID]);
            }
            else
            {
                mechanicalDoors[ID].Play("Close Door");
                enemy.AlertDoorClosed(doorObjects[ID].transform); //enemy detects that a door has been closed
                closedDoors[ID] = true;

                int randomInt = Random.Range(0, doorSounds.Count);

                doorObjects[ID].GetComponent<AudioSource>().PlayOneShot(doorSounds[randomInt]);

                StartCoroutine(DeactivateDoor(ID));
                Debug.Log("Closed door: " + doorObjects[ID]);
            }
        }
    }

    private IEnumerator DeactivateDoor(int ID)
    {
        yield return new WaitForSeconds(maxOpenDoorTime/3);
        doorObjects[ID].GetComponent<AudioSource>().PlayOneShot(doorWarning);
        yield return new WaitForSeconds(maxOpenDoorTime/3);
        doorObjects[ID].GetComponent<AudioSource>().PlayOneShot(doorWarning);
        yield return new WaitForSeconds(maxOpenDoorTime/3);
        ActivateDoor(ID);
    }

    public void ButtonPressed(int ID)
    {
        if (buttonsPressed[ID])
        {
            buttonsPressed[ID] = false;
        }
        else
        {
            buttonsPressed[ID] = true;
        }

        Debug.Log("Lamp" + ID + "=" + buttonsPressed[ID]);
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        int amountOfLightsOn = 0;

        for(int i = 0; i < numberOfButtons; i++)
        {
            if (buttonsPressed[i])
            {
                doorLights[i].material = lightOn;
                amountOfLightsOn++;
            }
            else
            {
                doorLights[i].material = lightOff;
            }
        }

        if(amountOfLightsOn == numberOfButtons)
        {
            exitDoorTrigger.enabled = true;
        }
        else if(exitDoorTrigger.enabled)
        {
            exitDoorTrigger.enabled = false;
        }

        lightText.text = "Puzzels solved: " + amountOfLightsOn;
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
    void Update() 
    {
        if (IsPuzzleComplete() == true && playerNextToDoor == true && !surviveOnce) 
        {
            Debug.Log("YEEEE");
            PlayerSurvived();
            surviveOnce = true;
        }

        if (Input.GetKeyDown(KeyCode.L)) 
        {
            for(int i = 0; i < buttonsPressed.Count; i++) 
            {
                if (buttonsPressed[i] == false) 
                {
                    ButtonPressed(i);
                }
            }
        }
    }
    private bool IsPuzzleComplete() 
    {
    for(int i = 0; i < buttonsPressed.Count; ++i) 
        {
            if (buttonsPressed[i] == false) 
            {
                return false;
            }
        }
        return true;
    }
}
