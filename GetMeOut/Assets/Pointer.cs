using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5.0f;
    public GameObject dot;
    public VRInputModule inputModule;

    private LineRenderer lineRenderer = null;
    private void Awake() {
        
    }

    void Update()
    {
        
    }

    void UpdateLine() {
        // Use default or distance
        float targetLength = defaultLength;

        // Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        // Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        // Or based on hit
        if (hit.collider != null) {
            endPosition = hit.point;
        }

        // Set position of the dot
        dot.transform.position = endPosition;

        // Set linerenderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

    }

    private RaycastHit CreateRaycast(float length) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
