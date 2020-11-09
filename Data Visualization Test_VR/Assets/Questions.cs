using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using TMPro;
using UnityEditorInternal;


//getting the controllers in so they can be used
public class Questions : MonoBehaviour
{

    [SerializeField]
    private string[] texts; // Specify your texts in the inspector

    public TMP_Text panel;//the panel that holds the questions
    public static bool startTrail = false;
    public static bool stopTrial = false;//so it doesn't go through whole loop


    //data
    private float levelTime = 0.0f;//level of time
    private bool recordTime = true;

    public static int indexQuestion = 0;//index of questions asked
 

    //the buttons we neeed
    public InputHelpers.Button startTrials;//type of button
    public InputHelpers.Button nextQuestion;
    public float activationOne=0.1f;//threshhold
    public float activationTwo=0.1f;
   
    Coroutine restart;


    public XRController leftDevice;

    void Awake()
    {
        panel.text = "To start the trials, press Menu Button on left handle";

        //create streaming asset folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data_Logs/");
    }


    // Update is called once per frame
    void Update()
    {
        if (CheckIfActivated1(leftDevice, startTrials) && startTrail == false && indexQuestion == 0)
        {
            startTrail = true;//trials has started
            panel.text = texts[indexQuestion];
            recordTime = false;

            indexQuestion = 1;//set it for next question

        }
        else if (CheckIfActivated2(leftDevice, nextQuestion) && startTrail == true && stopTrial== false && HooverData.now == true)//trials is in session
        {
            CreateTextFile(HooverData.emmissionLevel, levelTime);//get data
           
            if (indexQuestion < 3)//there are only three questions to answer
            {

                panel.text = texts[indexQuestion];//next question
                recordTime = false;//restart time
               //Debug.Log(CheckIfActivated2(leftDevice, nextQuestion));

            }


            stopTrial = true;
            restart = StartCoroutine(MyCoroutine(1));
            indexQuestion += 1;//increase index

        }
        else if(indexQuestion>=4)
        {
            panel.text = "Trial has ended! Please start next trial!";//trial ends
            startTrail = false;//trial ends
            stopTrial = true;
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


    public bool CheckIfActivated1(XRController controller, InputHelpers.Button startTrials)
    {
        InputHelpers.IsPressed(controller.inputDevice, startTrials, out bool isActive1, activationOne);
        return isActive1;

    }

    public bool CheckIfActivated2(XRController controller, InputHelpers.Button nextQuestion)
    {
        InputHelpers.IsPressed(controller.inputDevice, nextQuestion, out bool isActive2, activationTwo);
         return isActive2;
    }


    IEnumerator MyCoroutine(float time)
    {
        while (CheckIfActivated2(leftDevice, nextQuestion) == true)
        {

            yield return new WaitForSeconds(time);//Wait one frame
            
        }
        stopTrial = false;


    }

    void CreateTextFile(string data, float levelTime)
    {
        //location of file
        string txtDocumentName = Application.streamingAssetsPath + "/Data_Logs/" + "Data" + ".txt";

        //create the file
        if (!File.Exists(txtDocumentName))
        {
            //add a heading inside that .txt file for this date
            File.WriteAllText(txtDocumentName, "TITLE OF DATA LOG \n\n");

        }


        File.AppendAllText(txtDocumentName, data + "\n" + levelTime + "\n");
    }
}
