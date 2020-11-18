using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float speedH = 200.0f;//mouse sensitivity
    public float speedV = 200.0f;

    public Transform body;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Vector3 offset = new Vector3(0.0f, 100.0f, 0.0f);

    //private bool turnCamera = false;

    //private Vector3 saveSpot;//save spot

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//can lock cursor;
    }
    void Update()
    {
        yaw += speedH * Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        pitch -= speedV * Input.GetAxisRaw("Mouse Y") * Time.deltaTime;

        transform.eulerAngles = new Vector3(Mathf.Clamp(pitch, -90f, 90f), yaw, 0.0f);

        if (Input.GetKeyDown(KeyCode.Mouse1)) //Pressed right click
        {
            //turnCamera = !turnCamera;

            //if(turnCamera)
            {

                Debug.Log("Right-mouse click ");
                yaw += 100;
                //turnCamera = !turnCamera;

                //body.transform.Rotate(-Vector3.up * speedV * Time.deltaTime);

                //transform.eulerAngles = new Vector3(body.transform.localRotation.x, body.transform.localRotation.y+180, body.transform.localRotation.z);
                transform.eulerAngles += offset*Time.deltaTime;

                ////saveSpot = transform.eulerAngles;
            }
        }

        body.transform.localRotation = Quaternion.Euler(transform.eulerAngles);

        ////close project
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
               UnityEditor.EditorApplication.isPlaying = false;
           #else
              Application.Quit();
           #endif

       }
    }


}
