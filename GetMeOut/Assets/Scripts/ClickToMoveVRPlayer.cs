using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickToMoveVRPlayer : MonoBehaviour
{
    public Camera PCPlayerCamera;
    public Slider clickCooldownBar; 
    public float clickCooldown = 0f;
    public float maxClickCooldown = 5f;
    [SerializeField] private bool clickIsOnCooldown = false;
    public Transform PCPlayerWaypoint;
    public GameObject overlayCanvas;
    private void Start()
    {
        clickCooldown = maxClickCooldown;
    }
    void Update()
    {
        // Disable Overlay Canvas toggle
        if (Input.GetKeyDown(KeyCode.N))
        {
            overlayCanvas.SetActive(!overlayCanvas.activeSelf);
        }
        // Clicking logic
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = PCPlayerCamera.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log("Clicked object with tag " + hit.transform.gameObject.tag);
                if (hit.transform.gameObject.tag == "Floor" && clickIsOnCooldown == false) 
                {
                    clickIsOnCooldown = true;
                    PCPlayerWaypoint.position = hit.point;
                    Debug.Log("Clicked floor");
                }
            }
        }
        // Cooldown and UI
        if (clickIsOnCooldown && clickCooldown > 0)
        {
            clickCooldown -= Time.deltaTime;
        }
        else if (clickIsOnCooldown && clickCooldown < 0)
        {
            clickCooldown = maxClickCooldown;
            clickIsOnCooldown = false;
        }
        clickCooldownBar.value = clickCooldown;
        clickCooldownBar.maxValue = maxClickCooldown;
    }


}
