using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 
using Valve.VR.InteractionSystem; 

public class PressPuzzleButton : MonoBehaviour 
{
    public Camera VRCamera;
    public GameManager gm;
    public float minimalDistance = 3f;
    void Update() 
    {
        RaycastHit hit;
        Ray forwardRay = new Ray(VRCamera.transform.position, VRCamera.transform.forward);
        Debug.DrawRay(VRCamera.transform.position, VRCamera.transform.forward, Color.green);

        if(Physics.Raycast(forwardRay, out hit, 25f))
        {
            //Debug.Log(hit.transform.gameObject.tag + "  " + hit.distance);
            Debug.DrawRay(VRCamera.transform.position, VRCamera.transform.forward, Color.green);
            if(hit.transform.gameObject.tag == "WorldSpaceButton" && hit.distance <= minimalDistance)
            {
                //Debug.Log("Got " + hit.transform.gameObject.name);
                hit.transform.gameObject.GetComponent<PuzzleButton>().AimingAt();
                if(Input.GetButtonDown("Jump"))
                {
                    hit.transform.gameObject.GetComponent<PuzzleButton>().TogglePress();
                }
                gm.playerAimingAtButton = true;
            }
        } else
        {
            gm.playerAimingAtButton = false;
        }
    }
}

