using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRPuzzle : MonoBehaviour
{
    public GameManager gm;
    public bool rightButtonPressed = false;
    public int wrongButtonsPressed = 0;
    public int lampID;
    public int puzzleType = 0;
    public bool pressOnce = false;
    public int number = 0;
    public int correctNumber = 4;
    public TMP_Text textAsset;

    void Start() 
    {
        gm = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    void Update() 
    {
        if (puzzleType == 0) 
        {
            if (rightButtonPressed == true && wrongButtonsPressed == 0 && pressOnce == false) 
            {
                Debug.Log("You win puzzle");
                gm.ButtonPressed(lampID);
                pressOnce = true;
            } else if (rightButtonPressed == false && pressOnce == true) 
            {
                Debug.Log("Nevamind");
                gm.ButtonPressed(lampID);
                pressOnce = false;
            } else if (wrongButtonsPressed > 0 && pressOnce == true) {
                Debug.Log("Nevamind");
                gm.ButtonPressed(lampID);
                pressOnce = false;
            }
        } else if (puzzleType == 1) 
        {
            if (number == correctNumber && pressOnce == false) {
                Debug.Log("You win puzzle");
                gm.ButtonPressed(lampID);
                pressOnce = true;
            } else if (number != correctNumber && pressOnce == true)
            {
                gm.ButtonPressed(lampID);
                Debug.Log("Nevamind");
                pressOnce = false;
            }
            textAsset.text = "= " + number.ToString();
        }
    }
}
