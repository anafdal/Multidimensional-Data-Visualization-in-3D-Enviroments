using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float speedH = 100.0f;//mouse sensitivity
    public float speedV = 100.0f;

    public Transform body;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start()
    {
        //.lockState = CursorLockMode.Locked;//can lock cursor; probably will not need this
    }
    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X")*Time.deltaTime;
        pitch -= speedV * Input.GetAxis("Mouse Y")*Time.deltaTime;

        transform.eulerAngles = new Vector3(Mathf.Clamp(pitch,-90f,90f), yaw, 0.0f);
        //clamp pitch so it doesn't over-rotate

        body.transform.rotation = Quaternion.Euler(transform.eulerAngles);
    
    
      //close project
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
