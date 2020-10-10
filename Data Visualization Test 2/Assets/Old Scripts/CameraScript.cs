﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 1000;


    // Update is called once per frame
    void Update()
    {



        /*   if(Input.GetAxis("Mouse X") != 0)
           {
               transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 
                   0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
           }*/

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position -= new Vector3(0, 0, 1 * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, 1 * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(1 * Time.deltaTime * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1 * Time.deltaTime * speed, 0, 0);
        }

    }
}