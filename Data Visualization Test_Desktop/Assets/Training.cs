using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
//This is only used for training


public class Training : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string[] texts = null; // Specify your texts in the inspector
    private int index = 0;

    public TMP_Text panel;//the panel that holds the explanation
   



    void Awake()
    {
        panel.text = texts[0];
            
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && index<texts.Length-1)//got to next sentence
        {
            NextSentence();
        }

    }

    public void NextSentence()
    {
        index++;

        panel.text = texts[index];
    }
    
}
