using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//user movement

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 4000.0f;
  
                    
    private float y;
    //private float x;
    //private float z;

    void Start()
    {
        y = controller.transform.position.y;
    }


    void Update()
    {
       // isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);
       // Vector3 temp = controller.transform.position;

        float mouseX = Input.GetAxis("Horizontal");
        float mousez = Input.GetAxis("Vertical");
        Vector3 move = transform.right * mouseX + transform.forward * mousez;
        //direct it in the right direction; tarnsform.rigth and forward takes direction the player is facing

        controller.Move(move * speed * Time.deltaTime);
       

        /*
        if (Input.GetKey("up"))
        {

            player.transform.position += new Vector3(0, 0, 0.2f);
        }
        if (Input.GetKey("down"))
        {
          player.transform.position -= new Vector3(0, 0, 0.2f);
        }

        if (Input.GetKey("left"))
        {

            player.transform.position -= new Vector3(0.2f, 0, 0);
        }
        if (Input.GetKey("right"))
        {
            player.transform.position += new Vector3(0.2f, 0, 0);
        }*/

    }
}
