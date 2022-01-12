using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Camera mainCam;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float moveSpeed = 3;
    private CharacterController playerController;
    private float vert = 0;
    private float hor = 0;
    private Vector3 moveDirection;

    private float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle lock cursor with B - Jesse
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = !Cursor.visible;
        }
        Move();
        CameraRotation();
    }

    private void Move()
    {
        vert = Input.GetAxis("Vertical");
        hor = Input.GetAxis("Horizontal");
        Vector3 forwardDirection = Vector3.Scale(new Vector3(1, 0, 1), mainCam.transform.forward);
        Vector3 rightDirection = Vector3.Cross(Vector3.up, forwardDirection.normalized);
        moveDirection = forwardDirection.normalized * vert + rightDirection.normalized * hor;
        playerController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

        bool isMoving = hor != 0 || vert != 0;
    }

    private void CameraRotation()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        mainCam.transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);
    }

    private void FixedUpdate()
    {

    }
}
