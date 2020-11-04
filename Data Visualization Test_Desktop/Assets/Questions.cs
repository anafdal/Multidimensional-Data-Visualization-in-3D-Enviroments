using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Questions : MonoBehaviour
{
    [SerializeField]
    private string [] texts; // Specify your texts in the inspector

    public TMP_Text panel;//the panel that holds the questions
    public static bool startTrail = false;
  

    //data
    private float levelTime = 0.0f;//level of time
    private bool recordTime = true;

    public static int indexQuestion = 0;//index of questions asked
    public static bool now = false;//make sure you only click answer once

    // Start is called before the first frame update
    void Start()
    {
        panel.text = "To start the trials, press spacebar";

        //create streaming asset folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data_Logs/");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && startTrail==false && indexQuestion==0)//start trials
        {
            startTrail = true;//trials has started
            panel.text = texts[indexQuestion];


            indexQuestion = 1;//set it for next question
        }
        else if(Input.GetKeyDown(KeyCode.Return) && startTrail == true && now == true)//trials is in session
        {
            CreateTextFile(HooverData.data, levelTime, HooverData.emmissionLevel);//get data
            Debug.Log(HooverData.data);
            Debug.Log(levelTime);

            if (indexQuestion < 3)//there are only three questions to answer
            {

                panel.text = texts[indexQuestion];//next question

                indexQuestion += 1;//increase index
                recordTime = false;//restart time
                
            }
            else
            {
                panel.text = "Trial has ended! Please start next trial!";//trial ends
                startTrail = false;//trial ends

            }

            
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

    public void CreateTextFile(string data, float levelTime, string emmissionData)
    {
        //location of file
        string txtDocumentName = Application.streamingAssetsPath + "/Data_Logs/" + "Data" + ".txt";

        //create the file
        if (!File.Exists(txtDocumentName))
        {
            //add a heading inside that .txt file for this date
            File.WriteAllText(txtDocumentName, "TITLE OF DATA LOG \n\n");

        }


        File.AppendAllText(txtDocumentName, data + "\n" + levelTime + "\n"+ emmissionData + "\n");
    }
}
