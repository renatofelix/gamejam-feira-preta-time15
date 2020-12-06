using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxValueX;
    [SerializeField] private float minValueX;
    [SerializeField] private float maxValueY;
    [SerializeField] private float minValueY;
    [SerializeField] private float minValueZ;
    [SerializeField] private float maxValueZ;


    [Header("Setting Camera")]
    [SerializeField] private float speedCamera;


    [SerializeField] private Camera camera;

   

    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forwardMovement = transform.forward * vertical;
        Vector3 rightMovement = transform.right * horizontal;
        transform.position += (forwardMovement + rightMovement).normalized * Time.deltaTime * speedCamera;

        if (Input.GetKey(KeyCode.Space))
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        Debug.Log("estou rotacionando a camera");

        camera.transform.RotateAround( Vector3.up, speedCamera * Time.deltaTime);

        //camera.transform.rotation = 
    }
}
