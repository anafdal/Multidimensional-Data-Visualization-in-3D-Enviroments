using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


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

    //might not need this anymore
    void Update()//get data from here
    {
        

       /* if (Input.GetKeyDown(KeyCode.Return) && now==true && Questions.startTrail==true && indexQuestion<3)//press enter; ttrials needs to be started for this to work; activated when pressed enter for first questions
        {
            
            Debug.Log(data);
            Debug.Log(levelTime);
            CreateTextFile(data,levelTime);

            //indexQuestion += 1;
            recordTime = false;

            //put data in text file, restart recording time and present next question
        }


        //record time while in level
        if (recordTime == true)
        {
            levelTime += Time.deltaTime;
        }
        else//once eneter key is pressed get time and restart again counting time
        {
            levelTime = 0.0f;
            //Debug.Log(levelTime + " enter key used");
            recordTime = true;
        }*/
       

    }

    public void OnMouseDown()
    {

        //Debug.Log(this.name);
        UItext.visibility = 1;

        string[] arr = name.Split('\n');
        data = arr[0] + arr[1];//just want month and date
        emmissionLevel = this.name;

        startColor = this.m_Material.color;
        this.m_Material.color = this.m_Material.color.gamma * 3;

        Questions.now = true;//you need only one asnwer and you only pick it when hovering
        //Update();//this works here;might not need this
    }


    public void OnMouseUp()
    {

        //Debug.Log(this.name);
        //data = "Click For Data";
        UItext.visibility = 0;

        this.m_Material.color = startColor;

        Questions.now = false;////you need only one asnwer and you only oick it when hovering
    }
 
    
  

}
