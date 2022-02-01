using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorButton : MonoBehaviour 
{
    public Button doorButton;
    public Slider doorCooldownBar;
    public TMP_Text doorStatusText;
    public bool doorIsOnCooldown = false;
    public float doorCooldown = 0;
    public float doorMaxCooldown = 45;

    void Start()
    {
        doorButton = GetComponent<Button>();
        doorCooldownBar = GetComponentInChildren<Slider>();
        doorStatusText = GetComponentInChildren<TMP_Text>();
        doorCooldownBar.maxValue = doorMaxCooldown;
        doorCooldown = doorMaxCooldown;
    }

    void Update()
    {
        if(doorIsOnCooldown && doorCooldown > 0) {
            doorCooldown -= Time.deltaTime;
            if (doorCooldown <= (doorMaxCooldown - 15)) { doorStatusText.text = "OPEN"; }
        } else if(doorIsOnCooldown && doorCooldown < 0) {
            doorCooldown = doorMaxCooldown;
            doorIsOnCooldown = false;
            doorButton.interactable = true;
        }
        doorCooldownBar.value = doorCooldown;
    }
    public void OnClicked() 
    {
        doorIsOnCooldown = true;
        doorButton.interactable = false;
        doorStatusText.text = "CLOSED";
        Debug.Log("Clicked door button");
    }
}
