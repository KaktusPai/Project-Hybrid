using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    public MeshRenderer mesh;
    public Material[] materials;
    public VRPuzzle puzzle;
    public bool wrongButton = true;
    public bool rightButton = false;
    public bool pressed = false;
    public bool add = false;
    void Start()
    {
        mesh.material = materials[0];
        puzzle = GetComponentInParent<VRPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pressed && puzzle.gm.playerAimingAtButton == false) {
            mesh.material = materials[0];
        } else if(pressed && puzzle.gm.playerAimingAtButton == false) {
            mesh.material = materials[2];
        }
    }
    public void AimingAt()
    {
        if (pressed) 
        {
            mesh.material = materials[3];
        } else {
            mesh.material = materials[1];
        }
        //Debug.Log("Aiming at " + pressed);
    }

    public void TogglePress() {
        if (puzzle.puzzleType == 0) 
        {
            pressed = !pressed;
            if(pressed) {
                mesh.material = materials[3];
                if(rightButton) {
                    puzzle.rightButtonPressed = true;
                } else if(wrongButton) {
                    puzzle.wrongButtonsPressed += 1;
                }
            } else if(!pressed) {
                mesh.material = materials[1];
                if(rightButton) {
                    puzzle.rightButtonPressed = false;
                } else if(wrongButton) {
                    puzzle.wrongButtonsPressed -= 1;
                }
            }
            Debug.Log("Pressed " + pressed);
        } else if(puzzle.puzzleType == 1) 
        {
            if (add && puzzle.number < 25) 
            {
                puzzle.number += 1;
                Debug.Log("Pressed +");
            } else if (!add && puzzle.number > -25) 
            {
                puzzle.number -= 1;
                Debug.Log("Pressed -");
            }
        }
    }
}
