using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetData : MonoBehaviour
{

    public static string data;//textual information
    

    private Material m_Material;//material to get shader info from
    private Color startColor;//save original color

    void Start()
    {
        //data = "Click For Data";
       
       //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        UItext.visibility = 0;
       
    }
    private void OnMouseDown()
    {

        //Debug.Log(this.name);
        UItext.visibility = 1;

         data = this.name;

      

        startColor = this.m_Material.color;
        this.m_Material.color = this.m_Material.color.gamma;
       


    }
    private void OnMouseUp()
    {

        //Debug.Log(this.name);
        //data = "Click For Data";
        UItext.visibility = 0;

        this.m_Material.color = startColor;

    }


}
