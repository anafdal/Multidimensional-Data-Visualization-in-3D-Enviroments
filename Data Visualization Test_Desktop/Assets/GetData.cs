using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class GetData : MonoBehaviour
{

    public static string data;//textual information


    private Material m_Material;//material to get shader info from
    private Color startColor;//save original color

    private bool now = false;//make sure you only click answer once


    //data
    private float levelTime = 0.0f;
    private bool recordTime = true;

 
    void Start()
    {
        //data = "Click For Data";

        //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        UItext.visibility = 0;

        //create streaming asset folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data_Logs/");
        
    }

    void Update()//get data from here
    {
        

        if (Input.GetKeyDown(KeyCode.Return) && now==true)//press enter
        {
            
            Debug.Log(data);
            Debug.Log(levelTime);
            CreateTextFile(data,levelTime);

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
 
    
    public void CreateTextFile(string data,float levelTime)
    {
        //location of file
        string txtDocumentName = Application.streamingAssetsPath + "/Data_Logs/" + "Data" + ".txt";

        //create the file
        if (!File.Exists(txtDocumentName))
        {
            //add a heading inside that .txt file for this date
            File.WriteAllText(txtDocumentName, "TITLE OF DATA LOG \n\n");

        }


        File.AppendAllText(txtDocumentName, data+"\n\n"+levelTime + "\n");
    }

}
