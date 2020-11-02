using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    public float speedH = 100.0f;
    public float speedV = 100.0f;


    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public Transform playerBody;
    float xRotation = 0.0f;//rotate around x
    
   
    // Update is called once per frame
    void Update()
    {
        //MouseAiming();

         yaw += speedH * Input.GetAxis("Mouse X")*Time.deltaTime;
         pitch -= speedV * Input.GetAxis("Mouse Y")*Time.deltaTime;

        float v = Mathf.Clamp(pitch, yaw, 5.0f);
        xRotation = v;//lock this

        Vector3 rotate = new Vector3(xRotation, yaw, 0.0f); 
        transform.eulerAngles = rotate;
        
        Vector3 playerRot = playerBody.transform.rotation.eulerAngles;
        playerRot.y += yaw;//only rotate around y axis

       

        playerBody.rotation = Quaternion.Euler(playerRot);

        
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

