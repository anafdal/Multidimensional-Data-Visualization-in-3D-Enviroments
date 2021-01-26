using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using TMPro;
//Used for the training trial



public class Training : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string[] texts = null; // Specify your texts in the inspector
    private int index = 0;

    public TMP_Text panel;//the panel that holds the explanation

    public XRController leftDevice;

    //the buttons we neeed
    public InputHelpers.Button triggerButton;//type of button
    public InputHelpers.Button primaryButton;
    public float activationOne = 0.1f;//threshhold
    public float activationTwo = 0.1f;

    Coroutine restart;
    ///public bool startTrail = false;
    private bool stopTrial = false;//so it doesn't go through whole loop

    void Awake()
    {
        panel.text = texts[0];

    }

    // Update is called once per frame
    void Update() {
       
       if ((CheckIfActivated1(leftDevice, primaryButton) || CheckIfActivated1(leftDevice, triggerButton)) && index < texts.Length - 1 && stopTrial == false)//got to next sentence
        {
            stopTrial = true;
            NextSentence();
            restart = StartCoroutine(MyCoroutine(0.3f));
        }

    }

    IEnumerator MyCoroutine(float time)//put this here or it will go through whole loop
    {
        while (CheckIfActivated2(leftDevice, primaryButton) == true || CheckIfActivated1(leftDevice, triggerButton)== true)
        {

            yield return new WaitForSeconds(time);//Wait one frame

        }
        stopTrial = false;

    }


    public void NextSentence()
    {
        
        index++;
        panel.text = texts[index];
        
    }

    public bool CheckIfActivated1(XRController controller, InputHelpers.Button triggerButton)
    {
        InputHelpers.IsPressed(controller.inputDevice, triggerButton, out bool isActive1, activationOne);
        return isActive1;

    }

    public bool CheckIfActivated2(XRController controller, InputHelpers.Button primaryButton)
    {
        InputHelpers.IsPressed(controller.inputDevice, primaryButton, out bool isActive2, activationTwo);
        return isActive2;
    }
}
