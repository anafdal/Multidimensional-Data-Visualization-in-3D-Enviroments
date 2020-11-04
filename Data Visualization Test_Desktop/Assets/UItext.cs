using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class UItext : MonoBehaviour
{

    public TMP_Text data;
    public Canvas info;
    public static int visibility;

    void Start()
    {
        //info = GetComponent<Canvas>();
        info.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        data.text = HooverData.data;


        if (visibility == 1)
        {
            info.enabled = true;
        }
        else if(visibility==0)
        {
            info.enabled = false;
        }
        else
        {
            Debug.Log("There has been an error");
        }



    }

 
}
