using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//hoovering over points and getting data


public class HooverData : MonoBehaviour
{

    public static string data;//textual information
    public static string emmissionLevel;
    


    private Material m_Material;//material to get shader info from
    private Color startColor;//save original color
 

 
    void Start()
    {
        //data = "Click For Data";

        //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        UItext.visibility = 0;
        
    }

 
    public void OnMouseDown()
    {

        //Debug.Log(this.name);
        UItext.visibility = 1;

        if (this.transform.CompareTag("ColorScale"))//Color Reference
        {
            data = "Color Scale Reference";
        }
        else if (this.transform.CompareTag("SizeScale")) {//Size Reference

            data = "Size Scale Reference";

        }
        else if (this.transform.CompareTag("Points"))//Data points
        {

            string[] arr = name.Split('\n');
            data = arr[0] + arr[1];//just want month and date
            emmissionLevel = this.name;

            startColor = this.m_Material.color;
            this.m_Material.color = this.m_Material.color.gamma * 3;

            Questions.now = true;//you need only one asnwer and you only pick it when hovering
        }
        else//error
        {
            Debug.Log("Error!");
        }
        
    }


    public void OnMouseUp()
    {

        //Debug.Log(this.name);
        //data = "Click For Data";
        UItext.visibility = 0;

        if (this.transform.CompareTag("Points"))//only data points will change color
        {
            this.m_Material.color = startColor;
        }

        Questions.now = false;////you need only one answer and you only pick it when hovering
    }
 
    
  

}
