using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetData : MonoBehaviour
{

    public static string data;//textual information


    private Material m_Material;//material to get shader info from
    private Color startColor;//save original color

    private bool now = false;//make sure you only click answer once

    void Start()
    {
        //data = "Click For Data";

        //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        UItext.visibility = 0;

    }

    void Update()//get data from here
    {

        if (Input.GetKeyDown(KeyCode.Return) && now==true)//press enter
        {
            
            Debug.Log(data);

            //put data in text file, restart recording time and present next question
        }

        
    }
    public void OnMouseDown()
    {

        //Debug.Log(this.name);
        UItext.visibility = 1;

        data = this.name;

        startColor = this.m_Material.color;
        this.m_Material.color = this.m_Material.color.gamma * 3;

        now = true;
        Update();//this works here
    }


    public void OnMouseUp()
    {

        //Debug.Log(this.name);
        //data = "Click For Data";
        UItext.visibility = 0;

        this.m_Material.color = startColor;

        now = false;
    }


}
