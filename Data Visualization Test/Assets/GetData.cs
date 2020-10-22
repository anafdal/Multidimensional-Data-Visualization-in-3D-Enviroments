using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class GetData : MonoBehaviour
{

    public static string data;//textual information
    

    private Material m_Material;//material to get shader info from
    private Color startColor;//save original color
    private XRSimpleInteractable simpleInteractible = null;

    private void Awake()
    {
        //data = "Click For Data";
       
       //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        UItext.visibility = 0;

        simpleInteractible = GetComponent<XRSimpleInteractable>();


       simpleInteractible.onSelectEnter.AddListener(SetEnter);
       simpleInteractible.onSelectExit.AddListener(SetExit);

    }

    private void SetEnter(XRBaseInteractor arg0)
    {
        //throw new NotImplementedException();
        UItext.visibility = 1;

        data = this.name;



        startColor = this.m_Material.color;
        this.m_Material.color = this.m_Material.color.gamma * -10;
    }

    private void SetExit(XRBaseInteractor arg0)
    {
        //throw new NotImplementedException();

        UItext.visibility = 0;
       this.m_Material.color = startColor;
    }

    private void OnDestroy()
    {
        simpleInteractible.onSelectEnter.RemoveListener(SetEnter);
        simpleInteractible.onSelectExit.RemoveListener(SetExit);
    }

}
