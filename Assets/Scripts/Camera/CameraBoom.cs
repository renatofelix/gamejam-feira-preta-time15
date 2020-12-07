using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoom : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10;

    [SerializeField]
    private float zoomSpeed = 300;

    [SerializeField]
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetMouseButton(Mouse.MIDDLE_BUTTON)) {
            direction = GetMouseDirection();
        } else {
            direction = GetKeyboardDirection();
        }

        Move(direction);
        Zoom(Input.GetAxis(Mouse.SCROLL));
    }

    private Vector3 GetMouseDirection() {
        return GetMouseVerticalDirection() + GetMouseHorizontalDirection();
    }

    private Vector3 GetMouseVerticalDirection() {
        Vector3 verticalDirection = Vector3.zero;
        float verticalAxis = Input.GetAxis(Mouse.VERTICAL_AXIS);

        if (verticalAxis > 0) {                
            verticalDirection = Vector3.back;
        } else if (verticalAxis < 0) {
            verticalDirection = Vector3.forward;
        } else {
            verticalDirection = Vector3.zero;
        }

        return verticalDirection;
    }

    private Vector3 GetMouseHorizontalDirection() {
        float horizontalAxis = Input.GetAxis(Mouse.HORIZONTAL_AXIS);   
        Vector3 horizontalDirection = Vector3.zero;

        if (horizontalAxis > 0) {
            horizontalDirection = Vector3.left;                
        } else if (horizontalAxis < 0) {
            horizontalDirection = Vector3.right;
        } else {
            horizontalDirection = Vector3.zero;
        }

        return horizontalDirection;
    }

    private Vector3 GetKeyboardDirection() {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            direction = Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S)) {
            direction = Vector3.back;
        }

        if (Input.GetKey(KeyCode.A)) {
            direction  = Vector3.left;
        }

        if (Input.GetKey(KeyCode.D)) {
            direction = Vector3.right;
        }

        return direction;
    }

    private void Move(Vector3 direction) {
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }

    private void Zoom(float level) {
        mainCamera.orthographicSize -= level * zoomSpeed * Time.deltaTime;
    }
}
