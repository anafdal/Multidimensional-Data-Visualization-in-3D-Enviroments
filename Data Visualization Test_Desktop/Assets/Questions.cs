using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Questions : MonoBehaviour
{
    [SerializeField]
    private string [] texts=null; // Specify your texts in the inspector

    public TMP_Text panel;//the panel that holds the questions
    public static bool startTrail = false;
    public string dataset;//dataset type
    public string goal;
  

    //data
    private float levelTime = 0.0f;//level of time
    private bool recordTime = true;

    public static int indexQuestion = 0;//index of questions asked
    public static bool now = false;//make sure you only click answer once

    //timestamp
    private string startTrialString;

    //output
    private ArrayList finalOutput=new ArrayList(); 
    //private int indexer=0;

    // Start is called before the first frame update
    void Awake()
    {
        panel.text = "This is data of the pollution emission of 12 different cities in 2012. "+goal+
            "To start the trials, press spacebar";

        //Find the date and time when the game was run
        var startTrial = System.DateTime.Now;
        startTrialString = startTrial.ToString("dd-mm-yyyy-hh-mm-ss");


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
            recordTime = false;//start recording

            indexQuestion = 1;//set it for next question
        }
        else if(Input.GetKeyDown(KeyCode.Return) && startTrail == true && now == true)//trials is in session
        {
            CreateTextFile(HooverData.emmissionLevel, levelTime);//get data
            CreateFinalOutput(HooverData.emmissionLevel, levelTime);
            //indexer = +1;
            //Debug.Log(HooverData.data);
            //Debug.Log(levelTime);

            if (indexQuestion < 3)//there are only three questions to answer
            {

                panel.text = texts[indexQuestion];//next question

                indexQuestion += 1;//increase index
                recordTime = false;//restart time
                
            }
            else
            {
                panel.text = "Record this down:"+"\n"+finalOutput[0]+finalOutput[1]+finalOutput[2];//trial ends
                Debug.Log("Answer: "+ finalOutput[0] + finalOutput[1] + finalOutput[2]);
                //put answer here
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

    
    public void CreateTextFile(string data, float levelTime)
    {
        //location of file
        string txtDocumentName = Application.streamingAssetsPath + "/Data_Logs/" + "Data" + ".txt";

        //create the file
        if (!File.Exists(txtDocumentName))
        {
            //add a heading inside that .txt file for this date
            File.WriteAllText(txtDocumentName, "DATA LOG "+ "\n\n");

        }

        
        File.AppendAllText(txtDocumentName, data + "\n" + "time:  "+levelTime + "\n"+"timestamp: "+startTrialString+"\n"+"dataset: "+dataset+"\n\n");

    
    }

    //create a structure to store everything
    public void CreateFinalOutput(string data, float levelTime)
    {
        //need only month, city, time and dataset type
        string[] arr = data.Split('\n',':');
        //data = arr[1] + arr[3];//just want month and date

        float time = Mathf.Round(levelTime * 100.0f) * 0.01f;//round two decimal places

        finalOutput.Add(arr[1]+arr[3]+" "+ time +" "+ dataset+'\n');
       
    }
}
